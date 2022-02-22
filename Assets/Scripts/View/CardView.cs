using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using System.Linq;
using Goons.Model;
using DG.Tweening;
using Goons.Config;
using Goons.UI;

namespace Goons.View
{
    public class CardView : MonoBehaviour, IDragHandler, IDropHandler, IPointerClickHandler
    {

        public int CardId => CardModel.CardId;
        public bool IsMine => CardModel.IsMine;

        public CardModel CardModel { get; private set; }

        public bool IsPlayer { get; set; }

        [Inject]
        private GameView _gameView;

        [Inject]
        private IUiSystem _uiSystem;

        [Inject]
        private IGameModel _gameModel;

        [Inject]
        private ICardsConfig _cardsConfig;

        private bool _isExpanded;

        [SerializeField]
        private GameObject _sleepEffect;

        [SerializeField]
        private SpriteRenderer _frameImage;

        [SerializeField]
        private SpriteRenderer _artImage;

        public async void Init(CardModel cardModel)
        {
            CardModel = cardModel;
            if (!cardModel.IsMine)
            {
 //               _frameImage.sprite = await _cardsConfig.GetCardBackside();
            }
            else
            {
                //var frameSprite = await _cardsConfig.GetFrameSprite(CardModel.CardId);
                //if (frameSprite != null)
                //    _frameImage.sprite = frameSprite;
                _artImage.sprite = await _cardsConfig.GetArtSprite(cardModel.CardId);
            }
        }

        private void Start()
        {
            UpdateView();
            CardModel.Changed += UpdateView;
        }

        public async void UpdateView()
        {
            if(CardModel.State != CardStates.Hand)
            {
                //var frameSprite = await _cardsConfig.GetFrameSprite(CardModel.CardId);
                //if(frameSprite != null)
                //    _frameImage.sprite = frameSprite;

                _artImage.sprite = await _cardsConfig.GetArtSprite(CardModel.CardId);
            }
            _sleepEffect.SetActive(CardModel.CardEffects.Any(e => e is CardSleepEffect));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (transform.GetComponentInParent<CardsOnFieldView>() == _gameView.PlayerBoard)
            {
                if (eventData.pointerEnter != null)
                {
                    var enemyCardView = eventData.pointerEnter.GetComponent<CardView>();
                    var enemyPlayerView = eventData.pointerEnter.GetComponent<PlayerView>();
                    if (enemyCardView != null && enemyCardView.IsMine == false)
                    {
                        _gameView.DrawAttackTarget(this, enemyCardView.transform);
                        return;
                    }
                    else if (enemyPlayerView != null && enemyPlayerView.IsMine == false)
                    {
                        _gameView.DrawAttackTarget(this, enemyPlayerView.transform);
                        return;
                    }
                }
                _gameView.StopDrawingAttackTarget();
                return;
            }
            _gameView.DraggingCard = this;
            var pos = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position = pos;
        }

        public void Clear()
        {

        }

        public async void OnDrop(PointerEventData eventData)
        {
            _gameView.StopDrawingAttackTarget();
            if (eventData.pointerDrag.gameObject != null)
            {
                var playerCardView = eventData.pointerDrag.gameObject.GetComponent<CardView>();
                if (playerCardView != null && playerCardView.IsMine && playerCardView.CardModel.State == CardStates.Desk)
                {
                    await _gameModel.Attack(playerCardView.CardModel, CardModel);
                    return;
                }
            }


            var bf = _gameView.GetComponentInChildren<BattleField>();
            if (_gameView.IsOverBattleField(this))
            {
                var field = bf.GetComponent<CardsOnFieldView>();
                if (field != null)
                {
                    if (!await _gameModel.PlaceCardByPlayer(CardId))
                    {
                        transform.DOLocalMove(Vector3.zero, 0.3f);
                    }
                    //               field.PlaceCard(this);
                }
            }
            else
                transform.localPosition = Vector3.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
                return;

            if (CardModel.State == CardStates.Hand)
            {
                if (CardModel.IsMine)
                {
                    if (_isExpanded)
                    {
                        HideInfo();
                        _isExpanded = false;
                    }
                    else
                    {
                        ShowInfo();
                        _isExpanded = true;
                    }
                }
            }
            else if (CardModel.State == CardStates.Desk)
            {
                if (!_isExpanded)
                {
                    _cardInfoWindow = _uiSystem.ShowCardInfoWindow(CardModel);
                    _isExpanded = true;
                }
                else
                {
                    _cardInfoWindow.Hide();
                    _cardInfoWindow = null;
                    _isExpanded = false;
                }

            }
        }

        private IHidableWindow _cardInfoWindow;

        private void ShowInfo()
        {
            transform.DOLocalMoveY(1, 0.5f);
            _cardInfoWindow = _uiSystem.ShowCardInfoWindow(CardModel);
        }
        private void HideInfo()
        {
            transform.DOLocalMove(Vector3.zero, 0.5f);
            _cardInfoWindow.Hide();
            _cardInfoWindow = null;
        }

        public class Pool : MemoryPool<CardView>
        {
            protected override void OnDespawned(CardView item)
            {
                item.transform.SetParent(null);
                item.transform.position = Vector3.right * 100;
            }
        }
    }
}