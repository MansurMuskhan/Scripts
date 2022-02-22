using DG.Tweening;
using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
namespace Goons.View
{
    public class GameView : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerCordPosition;

        [SerializeField]
        private Transform _opponentCordPosition;

        [SerializeField]
        private CardsOnFieldView _playerHandCards;

        [SerializeField]
        private CardsOnFieldView _opponentHand;

        [SerializeField]
        private CardsOnFieldView _playerBoard;

        public CardsOnFieldView PlayerBoard => _playerBoard;

        [SerializeField]
        private CardsOnFieldView _opponentBoard;

        [SerializeField]
        private BoxCollider2D _battleFieldCollider;

        [SerializeField]
        private Transform _playerGraveyard, _opponentGraveyard;

        [SerializeField]
        private Material _attackTargetMaterial;

        [SerializeField]
        private Transform _opponentIcon;

        [SerializeField]
        private Transform _playerIcon;

        internal Dictionary<int, CardView> SpawnedCards = new Dictionary<int, CardView>();

        public CardView DraggingCard { get; set; }

        public Transform PlayerCordPosition => _playerCordPosition;

        [Inject]
        private CardView.Pool _cardViewPrefabsPool;

        [Inject]
        private IGameModel _gameModel;

        private LineRenderer _lineRenderer;


        private List<CardView> _spawnedCards = new List<CardView>();
        // Start is called before the first frame update

        public bool IsOverBattleField(CardView cardView)
        {
            return (_battleFieldCollider.bounds.Contains(cardView.transform.position));
        }

        private void OnDestroy()
        {
            foreach (var c in _spawnedCards)
            {
                _cardViewPrefabsPool.Despawn(c);
            }
        }

        public void DrawAttackTarget(CardView from, Transform to)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, from.transform.position - Vector3.forward);
            _lineRenderer.SetPosition(1, to.transform.position - Vector3.forward);
        }

        public void StopDrawingAttackTarget()
        {
            _lineRenderer.enabled = false;
        }



        void Start()
        {
            _gameModel.GameState.GameStateChanged += GameStateChanged;

            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.startColor = Color.black;
            _lineRenderer.endColor = Color.red;
            _lineRenderer.startWidth = 1f;
            _lineRenderer.endWidth = 0f;
            _lineRenderer.material = _attackTargetMaterial;

            _gameModel.CardMovedFromGraveyardToDesk += cardId =>
            {
                var CardView = SpawnedCards[cardId];
                var boardSlot = CardView.CardModel.IsMine ? _playerBoard.GetFreeSlot().transform : _opponentBoard.GetFreeSlot().transform;
                CardView.transform.SetParent(boardSlot);
                CardView.transform.DOLocalMove(Vector3.zero, 0.5f);
            };

            _gameModel.CardMovedFromGraveyardToHand += cardId =>
            {
                var CardView = SpawnedCards[cardId];

                var handSlot = CardView.CardModel.IsMine ? _playerHandCards.GetFreeSlot().transform : _opponentHand.GetFreeSlot().transform;
                CardView.transform.SetParent(handSlot);
                CardView.transform.DOLocalMove(Vector3.zero, 0.5f);
            };

            _gameModel.OpponentAttacked += (attacker) =>
            {
                var attackerCardView = SpawnedCards[attacker];
                attackerCardView.transform.DOMove(_opponentIcon.transform.position, 0.2f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
            };

            _gameModel.Attacked += (attacker, target) =>
            {
                var attackerCardView = SpawnedCards[attacker];
                var targetCardView = SpawnedCards[target];
                attackerCardView.transform.DOMove(targetCardView.transform.position, 0.2f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
            };

            _gameModel.CardPlacedByPlayer += cardId =>
            {
                var cardView = SpawnedCards[cardId];
                var slot = _playerBoard.GetFreeSlot();
                cardView.transform.SetParent(slot.transform);
                cardView.transform.localScale = Vector3.one;
                cardView.transform.DOLocalMove(Vector3.zero, 0.5f);
            };

            _gameModel.CardPlacedByOpponent += cardId =>
            {
                var cardView = SpawnedCards[cardId];
                var slot = _opponentBoard.GetFreeSlot();

                cardView.transform.SetParent(slot.transform);
                cardView.transform.localScale = Vector3.one;
                cardView.transform.DOLocalMove(Vector3.zero, 0.5f);
            };

            _gameModel.CardMovedToGraveyard += cardId =>
            {
                var card = SpawnedCards[cardId];
                Transform parent = _opponentGraveyard.transform;

                if (card.IsMine)
                    parent = (_playerGraveyard.transform);

                card.transform.SetParent(parent);
                card.transform.localScale = Vector3.one;
                card.transform.DOLocalMove(Vector3.zero, 0.5f);
            };

            _gameModel.CardTakenByPlayer += cardModel =>
            {
                var slot = _playerHandCards.GetFreeSlot();

                var card = _cardViewPrefabsPool.Spawn();
                card.name = "playerCard";
                card.Init(cardModel);
                SpawnedCards.Add(cardModel.CardId, card);
                card.transform.position = PlayerCordPosition.position;
                card.transform.SetParent(slot.transform);
                card.transform.localScale = Vector3.one;
                card.transform.DOLocalMove(Vector3.zero, 0.5f);
            };// _playerHandCards.DrawCard;
            _gameModel.CardTakenByOpponent += cardModel =>
            {
                var slot = _opponentHand.GetFreeSlot();

                var card = _cardViewPrefabsPool.Spawn();
                card.name = "enemyCard";
                card.Init(cardModel);
                SpawnedCards.Add(cardModel.CardId, card);
                card.transform.position = _opponentCordPosition.position;
                card.transform.SetParent(slot.transform);
                card.transform.localScale = Vector3.one;
                card.transform.DOLocalMove(Vector3.zero, 0.5f);
            };
            //_opponentHand.DrawCard;

            _gameModel.ModelChanged += GameModelStateChanged;
            _gameModel.ModelInitialized += ModelInitialized;


        }

        private void GameStateChanged()
        {
            foreach (var card in SpawnedCards.Values)
                _cardViewPrefabsPool.Despawn(card);
            SpawnedCards.Clear();

            foreach (var cardKey in _gameModel.GameState.Cards.Keys)
            {
                var cardModel = _gameModel.GameState.Cards[cardKey];
                var newCard = _cardViewPrefabsPool.Spawn();

                if (cardModel.IsMine)
                {
                    newCard.name = "playerCard";
                    if (cardModel.State == CardStates.Hand)
                    {
                        newCard.transform.SetParent(_playerHandCards.GetFreeSlot().transform);
                    }
                    else if (cardModel.State == CardStates.Desk)
                    {
                        newCard.transform.SetParent(_playerBoard.GetFreeSlot().transform);

                    }
                }
                else
                {
                    newCard.name = "enemyCard";
                    if (cardModel.State == CardStates.Hand)
                    {
                        newCard.transform.SetParent(_opponentHand.GetFreeSlot().transform);
                    }
                    else if (cardModel.State == CardStates.Desk)
                    {
                        newCard.transform.SetParent(_opponentBoard.GetFreeSlot().transform);

                    }
                }
                newCard.transform.localPosition = Vector3.zero;
                SpawnedCards.Add(cardKey, newCard);
            }
        }

        private void CardPlacedByOpponent(CardModel obj)
        {
            throw new System.NotImplementedException();
        }

        private void CardTakenByOpponent(CardModel obj)
        {
            throw new System.NotImplementedException();
        }

        private void ModelInitialized()
        {
        }

        private void GameModelStateChanged()
        {
            var state = _gameModel.GameState;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}