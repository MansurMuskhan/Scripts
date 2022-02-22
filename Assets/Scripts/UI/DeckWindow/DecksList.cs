using Goons.Model;
using Goons.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

namespace Goons.UI
{
    public class DecksList : MonoBehaviour
    {
        [Inject]
        private IPrematch _prematch;

        [Inject]
        private DeckButton.Pool _deckButtonsPool;

        [SerializeField]
        private DeckButton[] _deckButtons;

        [SerializeField]
        private Transform _deckButtonsRoot;

        private DeckWindow _deckWindow;
        private List<DeckButton> _spawnedDeckButtons = new List<DeckButton>();

        private void Awake()
        {
            _deckWindow = GetComponentInParent<DeckWindow>();
        }

        private void Start()
        {
            _prematch.DecksListChanged += DecksListChanged;
            foreach (var deckB in _deckButtons)
            {
                deckB.Selected += DeckButton_Selected;
            }
            InitDecks();
        }

        private async void InitDecks()
        {

            var decks = await _prematch.GetAllDecks();
            for(var i =0; i<5; ++i)
            //foreach (var deck in decks.OrderBy(d => d.order))
            {
                var deckButton = _deckButtons[i];


                if (decks.Length <= i)
                    continue;
                    


                if (decks[i] == null)
                {


                }
                else
                {
                    var deck = decks[i];

                    deckButton.Init(deck);

                }
            }
        }

        private void DecksListChanged(List<DeckModel> obj)
        {
            InitDecks();
        }

        public void AddDeckButtonClicked()
        {
            _deckWindow.CreateDeck();
        }

        private void DeckButton_Selected(DeckModel deckModel)
        {
            if (deckModel == null)
            {
                _deckWindow.CreateDeck();
            }
            else
            {
                _deckWindow.SelectDeck(deckModel);
            }
        }

        private void OnDestroy()
        {
            _prematch.DecksListChanged -= DecksListChanged;
            foreach (var db in _spawnedDeckButtons)
            {
                db.Selected -= DeckButton_Selected;
            }
        }
    }
}