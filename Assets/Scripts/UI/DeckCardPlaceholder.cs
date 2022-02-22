using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using Goons.Config;
using UnityEngine.EventSystems;

namespace Goons.UI
{
    public class DeckCardPlaceholder : BaseCardPlaceholder
    {
        [SerializeField]
        private Image _art;
        [SerializeField]
        private TMP_Text _cardName;
        [SerializeField]
        private GameObject _quantityPanel;
        [SerializeField]
        private Image _rarityImage;
        [SerializeField]
        private Image _cardType;

        [Inject]
        private IRarityGradientsConfig _rarityGradientsConfig;

        [Inject]
        private DeckWindow _deckWindow;

        [Inject]
        private ISpritesConfig _spritesConfig;

        public override void Reset()
        {
            Quantity = 0;
            transform.SetParent(null);
            gameObject.SetActive(false);
        }

        protected override void SetQuantity()
        {
            _quantityPanel.SetActive(Quantity > 1);
        }

        public override async void Init(CardConfig cardConfig)
        {
            base.Init(cardConfig);
            _art.sprite = await _cardsConfig.GetArtSprite(_cardConfig.id);
            _cardType.sprite = _spritesConfig.GetElementSprite(_cardConfig.element);
            _cardName.text = _cardConfig.name;
            _rarityImage.color = _rarityGradientsConfig.GetColor(cardConfig.Rarity);
        }

        protected override bool CheckDrag(Vector2 delta)
        {
            return Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
        }

        protected override void ProcessClick()
        {
            _deckWindow.RemoveCard(_cardConfig.id);
        }

        public class Pool : MemoryPool<DeckCardPlaceholder> {
            protected override void OnDespawned(DeckCardPlaceholder item)
            {
                item.Reset();
            }
            protected override void OnSpawned(DeckCardPlaceholder item)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}