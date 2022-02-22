using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Goons.Config;
using UnityEngine.UI;
using TMPro;
using Goons.Network;
using Goons.Model;
using System.Linq;
using UnityEngine.EventSystems;
namespace Goons.UI
{

    public class CardPlaceholder : BaseCardPlaceholder
    {
        [SerializeField]
        private Image _cardFrameImage;
        [SerializeField]
        private Image _cardArtImage;
        [SerializeField]
        private TMP_Text _cardName;
        [SerializeField]
        private TMP_Text _cardDescription;
        [SerializeField]
        private TMP_Text _cardHp;
        [SerializeField]
        private TMP_Text _cardAttack;
        [SerializeField]
        private TMP_Text _quantity;
        [SerializeField]
        private GameObject _notEnough;

        [Inject]
        private IPrematch _prematch;

        [Inject]
        private DeckWindow _deckWindow;

        private DeckModel _previouslySelectedDeck;


        protected override void SetQuantity()
        {

            _notEnough.SetActive(Quantity <= 0);

        }

        protected override bool CheckDrag(Vector2 delta)
        {
            return Quantity>0 && Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
        }



        public override async void Init(CardConfig cardConfig)
        {
            base.Init(cardConfig);
            _cardFrameImage.sprite = await _cardsConfig.GetFrameSprite(_cardConfig.id);
            _cardArtImage.sprite =  await _cardsConfig.GetArtSprite(_cardConfig.id);

            _cardName.text = _cardConfig.name;
            _cardDescription.text = _cardConfig.description;

            _cardHp.text = _cardConfig.hp.ToString();
            _cardAttack.gameObject.SetActive(_cardConfig.CardType == CardType.Attacker);
            _cardHp.gameObject.SetActive(_cardConfig.CardType == CardType.Attacker);
            _cardAttack.text = _cardConfig.attack.ToString();
            Quantity = _cardConfig.quantity;
            _quantity.text = $"x{Quantity}";

            _prematch.DeckSelected += DeckSelected;
            _prematch.DeckDeselected += DeckDeselected;
        }

        private void DeckDeselected()
        {
            _prematch.DeckChanged -= DeckChanged;
            Quantity = _cardConfig.quantity;
            SetQuantity();
        }

        private void DeckSelected(DeckModel deck)
        {
            _previouslySelectedDeck = deck;
            _prematch.DeckChanged += DeckChanged;
            Quantity = _cardConfig.quantity - deck.cards.Count(cardId=> cardId == _cardConfig.id);
            SetQuantity();
        }

        private void DeckChanged(DeckModel deck)
        {
            Quantity = _cardConfig.quantity - deck.cards.Count(cardId => cardId == _cardConfig.id);
            SetQuantity();
        }
        protected override void ProcessClick()
        {
            _deckWindow.AddCard(_cardConfig.id);
        }

        public class Factory : PlaceholderFactory<CardPlaceholder> { }
    }
}