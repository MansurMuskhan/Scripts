using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using Goons.Model;
using Zenject;
using Goons.Config;
using System;
using TMPro;
using DG.Tweening;

namespace Goons.UI
{

    public class CardUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField]
        private Image _cardFrameImage;
        [SerializeField]
        private Image _cardArtImage;
        [SerializeField]
        private Image _minimizedCardArtImage;

        [SerializeField]
        private TMP_Text _cardName;

        [SerializeField]
        private TMP_Text _minimizedCardName;

        [SerializeField]
        private TMP_Text _cardDescription;

        [SerializeField]
        private TMP_Text _cardHp;

        [SerializeField]
        private TMP_Text _cardAttack;

        [SerializeField]
        private Image _minimizedElementIcon;

        [SerializeField]
        private Transform _minimized;
        [SerializeField]
        private Transform _maximized;

        [SerializeField]
        private Image _rarityImage;

        [SerializeField]
        private GameObject _minimizedQuantityPanel;

        [Inject]
        private IRarityGradientsConfig _rarityGradientsConfig;

        public int QuantityInDeck { get; private set; }

        private void SetQuantityInDeck()
        {
            _minimizedQuantityPanel.SetActive(QuantityInDeck > 1);
        }

        public void IncreaseQuantityInDeck()
        {
            ++QuantityInDeck;
            SetQuantityInDeck();
        }
        public void DecreaseQuantityInDeck()
        {
            --QuantityInDeck;
            SetQuantityInDeck();

        }

        private Animator _animator;

        [Inject]
        private ICardsConfig _cardsConfig;

        [Inject]
        private ISpritesConfig _spritesConfig;

        [Inject]
        private Pool _cadrsPool;

        private DeckWindow _deckWindow;
        private CanvasGroup _canvasGroup;

        private CardConfig _cardModel;
        public CardConfig CardConfig => _cardModel;

        public void DisableBlockRaycast()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void EnableBlockRaycast()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        public async void Init(CardConfig cardModel,  bool isMinimized)
        {
            _cardModel = cardModel;

            _cardFrameImage.sprite = await _cardsConfig.GetFrameSprite(cardModel.id);
            _cardArtImage.sprite = _minimizedCardArtImage.sprite = await _cardsConfig.GetArtSprite(cardModel.id);

            _cardName.text = _minimizedCardName.text = cardModel.name;
            _cardDescription.text = cardModel.description;

            _cardHp.text = cardModel.hp.ToString();
            _cardAttack.gameObject.SetActive(cardModel.type == "minion");
            _cardHp.gameObject.SetActive(cardModel.type == "minion");
            _cardAttack.text = cardModel.attack.ToString();

            var s = _spritesConfig.GetElementSprite(cardModel.element);
            _minimizedElementIcon.sprite = s;

            _rarityImage.color = _rarityGradientsConfig.GetColor(cardModel.Rarity);
            if (isMinimized)
                Minimize();
            else 
                Maximize();
        }

        public void MinimizeImmediately()
        {
            _animator.SetBool("IsMinimized", true);
            _minimized.gameObject.SetActive(true);
            _maximized.gameObject.SetActive(false);
        }

        public void Minimize()
        {
       //     _animator.SetBool("IsMinimized", true);
            _minimized.gameObject.SetActive(true);
            _maximized.gameObject.SetActive(false);
        }

        public void Maximize()
        {
        //    _animator.SetBool("IsMinimized", false);
            _minimized.gameObject.SetActive(false);
            _maximized.gameObject.SetActive(true);
        }


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            _deckWindow = GetComponentInParent<DeckWindow>();
        }

        private ScrollRect _cardsUiPanel;
        public bool IsDragging { get; private set; }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //if (!CheckDrag(eventData.delta))
            //{
            //    //_canvasGroup.blocksRaycasts = false;
            //    _cardsUiPanel = GetComponentInParent<ScrollRect>();
            //    if (_cardsUiPanel != null)
            //        _cardsUiPanel.OnBeginDrag(eventData);
            //    return;
            //}

            //IsDragging = true;

            //_deckWindow.DisableAllCardsBlockRaycast();
            //transform.SetParent(GetComponentInParent<DeckWindow>().transform);
            _canvasGroup.blocksRaycasts = false;
            ////        transform.SetParent(_deckWindow.transform);
        }


        public void OnDrag(PointerEventData eventData)
        {
            //if (!IsDragging)
            //{

            //    if (_cardsUiPanel != null)
            //        _cardsUiPanel.OnDrag(eventData);
            //    return;
            //}
            transform.position = eventData.position;
        }

        private bool CheckDrag(Vector2 delta)
        {
            return Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                transform.localPosition = Vector3.zero;
            }
            else
            {
                if (_cardsUiPanel != null)
                    _cardsUiPanel.OnEndDrag(eventData);
            }
            
            IsDragging = false;

            _deckWindow.EnableAllCardsBlockRaycast();
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
                return;
            _deckWindow.AddCard(this);
        }

        public class Pool : MemoryPool<CardUI> {
            public event Action<CardUI> CardSpawned;
            public event Action<CardUI> CardDespawned;

            protected override void OnDespawned(CardUI item)
            {
                CardDespawned?.Invoke(item);
                item.transform.SetParent(null);
                item.gameObject.SetActive(false);
            }

            protected override void OnSpawned(CardUI item)
            {
                CardSpawned?.Invoke(item);
                item.gameObject.SetActive(true);
            }
        }
    }
}