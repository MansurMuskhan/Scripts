using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Goons.Network;

namespace Goons.UI
{
    public class DeckCardsPanel : CardsUiPanel
    {
        [SerializeField]
        private TMP_InputField _deckNameField;

        [Inject]
        private IPrematch _prematch;

        protected override void Start()
        {
            base.Start();
            _prematch.DeckChanged += DeckChanged;

        }

        private void DeckChanged(Model.DeckModel obj)
        {
            if (_prematch.CurrentDeck != null)
                _deckNameField.text = _prematch.CurrentDeck.name;
        }

        private void OnDestroy()
        {
            _prematch.DeckChanged -= DeckChanged;
        }

        public void DeckNameTextChanged()
        {
            _prematch.CurrentDeck.name = _deckNameField.text;
        }

        public void DeckNameEditCompleted()
        {
            _prematch.RenameDeck(_prematch.CurrentDeck);
        }

        public void DeckNameTextFieldSelected()
        {
            _deckNameField.caretPosition = _deckNameField.text.Length;
        }

        protected override void ProcessDrop(CardUI card)
        {
            DeckWindow.AddCard(card);
        }

        protected override void ProcessPointerEnter(CardUI card)
        {
            card.Minimize();
        }
    }
}