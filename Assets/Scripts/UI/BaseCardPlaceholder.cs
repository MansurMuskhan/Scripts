using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;
using Goons.Config;
using UnityEngine.UI;

namespace Goons.UI
{
    public abstract class BaseCardPlaceholder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private ScrollRect _scrollContainer;
        private CardUI _newCard;
        private bool _isScrolling;
        private CanvasGroup _canvasGroup;

        public CardUI Card => _newCard;

        [Inject]
        private CardUI.Pool _cardsPool;
        [Inject]
        protected ICardsConfig _cardsConfig;

        public int Quantity { get; protected set; }

        protected abstract void SetQuantity();


        public virtual void Reset()
        {
            
        }

        protected CardConfig _cardConfig { get; private set; }

        protected abstract bool CheckDrag(Vector2 delta);

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

        }

        public void DisableBlockRaycast()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void EnableBlockRaycast()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void Init(CardConfig cardConfig)
        {
            _cardConfig = cardConfig;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_scrollContainer == null)
                _scrollContainer = GetComponentInParent<ScrollRect>();

            if (CheckDrag(eventData.delta))
            {
                _isScrolling = false;
                _newCard = _cardsPool.Spawn();
                var isMinimized = eventData.pointerDrag.GetComponent<DeckCardPlaceholder>() != null;
                _newCard.Init(_cardConfig, isMinimized);
                _newCard.transform.SetParent(transform);
                _newCard.transform.localPosition = Vector3.zero;
                _newCard.transform.localScale = Vector3.one;
                _newCard.OnBeginDrag(eventData);

            }
            else
            {
                _isScrolling = true;
                _scrollContainer.OnBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isScrolling)
            {
                _scrollContainer.OnDrag(eventData);
                return;
            }
            if (_newCard != null)
                _newCard.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_newCard == null)
                return;

            _cardsPool.Despawn(_newCard);
            _newCard = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!eventData.dragging)
                ProcessClick();
        }

        protected abstract void ProcessClick();
    }
}