using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Goons.UI
{
    public class CardsUiPanel : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        [SerializeField]
        private Transform _contentRoot;

        protected HorizontalOrVerticalLayoutGroup _layoutGroup;

        private CanvasGroup _canvasGroup;

        protected DeckWindow DeckWindow { get; private set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _layoutGroup = GetComponentInChildren<HorizontalOrVerticalLayoutGroup>();
        }
        protected virtual void Start()
        {
            DeckWindow = GetComponentInParent<DeckWindow>();
        }
        public void Child(CardUI card)
        {
            card.transform.SetParent(_contentRoot);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (this is DeckCardsPanel && eventData.pointerPress.GetComponent<DeckCardPlaceholder>())
                return;
            if (this is PlayersCardsPanel && eventData.pointerPress.GetComponent<CardPlaceholder>())
                return;


            var cardPlaceholder = eventData.pointerDrag.GetComponent<BaseCardPlaceholder>();
            if (cardPlaceholder != null)
            {
                var uiCard = cardPlaceholder.Card;
                if (uiCard != null)
                {
                    ProcessDrop(uiCard);
                }
            }
        }

        protected virtual void ProcessDrop(CardUI card) { }
        protected virtual void ProcessPointerEnter(CardUI card) { }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            var cardPlaceholder = eventData.pointerDrag.GetComponent<BaseCardPlaceholder>();
            if (cardPlaceholder != null)
            {
                var uiCard = cardPlaceholder.Card;
                if (uiCard != null)
                {
                    ProcessPointerEnter(uiCard);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
 //           _canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
 //           _canvasGroup.blocksRaycasts = true;
        }
    }

}