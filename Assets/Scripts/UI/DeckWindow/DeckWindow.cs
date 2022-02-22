using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Goons.Config;
using Goons.Network;
using Goons.Model;
using System.Threading.Tasks;
using System.Linq;

namespace Goons.UI
{
    public class DeckWindow : BaseWindow
    {
        [SerializeField]
        private int _maxElementCount = 10;
        [SerializeField]
        private TMP_Text _textCount;

        [SerializeField]
        private Button _completeButton;

        [SerializeField]
        private TMP_Text _textCommonCount;
        [SerializeField]
        private TMP_Text _textRareCount;
        [SerializeField]
        private TMP_Text _textEpicCount;
        [SerializeField]
        private TMP_Text _textLegendaryCount;

        [SerializeField]
        private Transform _allCardsContentRoot;

        [SerializeField]
        private TMP_Text _decksCounter, _cardsCounter;

        [SerializeField]
        private Transform _deckCardsRoot;

        [SerializeField]
        private Animator _animator;

        private Dictionary<int, CardUI> _spawnedCards = new Dictionary<int, CardUI>();

        private Dictionary<int, DeckCardPlaceholder> _deckCards = new Dictionary<int, DeckCardPlaceholder>();

        [Inject]
        private ICardsConfig _cardsConfig;

        [Inject]
        private CardUI.Pool _cardsPool;

        [Inject]
        private CardPlaceholder.Factory _cardsPlacwholdersFactory;

        [Inject]
        private DeckCardPlaceholder.Pool _deckPlaceholdersPool;

        [Inject]
        private IPrematch _prematch;

        private Dictionary<int,CardPlaceholder> _spawnedPlaceholders = new Dictionary<int, CardPlaceholder>();


        private string _searchString;
        private CardElements _elementFilter;
        private Rarity _rarityFilter;
        private CardType _typeFilter;

        public bool IsDeckSelected { get; private set; }

        public void SelectDeck(DeckModel deckModel)
        {
            _prematch.SetCurrentDeck(deckModel);
            IsDeckSelected = true;
            EnableAllCardsBlockRaycast();
            _animator.SetBool("ShowDecks", false);
        }

        public bool SerfToggle 
        { 
            set
            {
                if (value)
                    _typeFilter |= CardType.Attacker;
                else
                    _typeFilter ^= CardType.Attacker;
                ShowFilteredCards();
            }
        }
        public bool SpellToggle
        {
            set
            {
                if (value)
                    _typeFilter |= CardType.Spell;
                else
                    _typeFilter ^= CardType.Spell;
                ShowFilteredCards();
            }
        }
        public bool MysteryToggle
        {
            set
            {
                if (value)
                    _typeFilter |= CardType.Mystery;
                else
                    _typeFilter ^= CardType.Mystery;
                ShowFilteredCards();
            }
        }

        public bool NeutralToggle
        {
            set
            {
                if (value)
                    _elementFilter |= CardElements.Neutral;
                else
                    _elementFilter ^= CardElements.Neutral;
                ShowFilteredCards();
            }
        }
        public bool EarthToggle
        {
            set
            {
                if (value)
                    _elementFilter |= CardElements.Earth;
                else
                    _elementFilter ^= CardElements.Earth;
                ShowFilteredCards();
            }
        }
        public bool ElectricityToggle
        {
            set
            {
                if (value)
                    _elementFilter |= CardElements.Electric;
                else
                    _elementFilter ^= CardElements.Electric;
                ShowFilteredCards();
            }
        }
        public bool FireToggle
        {
            set
            {
                if (value)
                    _elementFilter |= CardElements.Fire;
                else
                    _elementFilter ^= CardElements.Fire;
                ShowFilteredCards();
            }
        }
        public bool WaterToggle
        {
            set
            {
                if (value)
                    _elementFilter |= CardElements.Water;
                else
                    _elementFilter ^= CardElements.Water;
                ShowFilteredCards();
            }
        }

        public bool CommonToggle
        {
            set
            {
                if (value)
                    _rarityFilter |= Rarity.Common;
                else
                    _rarityFilter ^= Rarity.Common;
                ShowFilteredCards();
            }
        }
        public bool RearToggle
        {
            set
            {
                if (value)
                    _rarityFilter |= Rarity.Rare;
                else
                    _rarityFilter ^= Rarity.Rare;
                ShowFilteredCards();
            }
        }
        public bool EpicToggle
        {
            set
            {
                if (value)
                    _rarityFilter |= Rarity.Epic;
                else
                    _rarityFilter ^= Rarity.Epic;
                ShowFilteredCards();
            }
        }
        public bool LegendaryToggle
        {
            set
            {
                if (value)
                    _rarityFilter |= Rarity.Legendary;
                else
                    _rarityFilter ^= Rarity.Legendary;
                ShowFilteredCards();
            }
        }


        public async void CreateDeck()
        {
            await _prematch.CreateNewDeck();
            IsDeckSelected = true;
            EnableAllCardsBlockRaycast();
            _animator.SetBool("ShowDecks", false);
        }

        public void DisableAllCardsBlockRaycast()
        {
            foreach (var card in _spawnedPlaceholders.Values)
                card.DisableBlockRaycast();
        }


        public void EnableAllCardsBlockRaycast()
        {
            foreach (var card in _spawnedPlaceholders.Values)
                card.EnableBlockRaycast();
        }


        private List<CardUI> _cards = new List<CardUI>();
        public async void AddCard(CardUI card)
        {
            await _prematch.AddCardToDeck(card.CardConfig.id);

//            _cardsPool.Despawn(card);
            UpdateControls();
        }

        public async void AddCard(int id)
        {
            await _prematch.AddCardToDeck(id);

            //            _cardsPool.Despawn(card);
            UpdateControls();
        }

        public async void RemoveCard(CardUI card)
        {
            await _prematch.RemoveCardFromDeck(card.CardConfig.id);

//            _cardsPool.Despawn(card);
            UpdateControls();
        }

        public async void RemoveCard(int id)
        {
            await _prematch.RemoveCardFromDeck(id);
            UpdateControls();
        }

        public async void UpdateSearch(TMP_InputField inputField)
        {
            _searchString = inputField.text;
            await ShowFilteredCards();
        }

        private async Task ShowFilteredCards()
        {
            foreach (var ph in _spawnedPlaceholders.Values)
            {
                ph.gameObject.SetActive(false);
            }
            _spawnedCards.Clear();


            var allCards = await _cardsConfig.GetAllCardsFiltered(_elementFilter, _rarityFilter, _typeFilter, _searchString);
            foreach (var card in allCards)
            {
                var ph = _spawnedPlaceholders[card.id];
                ph.gameObject.SetActive(true);
            }
        }

        private async void Start()
        {
            _cardsPool.CardSpawned += CardSpawned;
            _cardsPool.CardDespawned += CardDespawned;
            _prematch.DeckChanged += DeckChanged;
            UpdateControls();
            DisableAllCardsBlockRaycast();

            var allCards = await _cardsConfig.GetAllCards();
            foreach (var card in allCards)
            {
                var newCard = _cardsPlacwholdersFactory.Create();// _cardsPool.Spawn();
                newCard.Init(card);
                newCard.transform.SetParent(_allCardsContentRoot.transform);
                newCard.transform.localScale = Vector3.one;
                newCard.DisableBlockRaycast();
                _spawnedPlaceholders.Add(card.id, newCard);
            }

            _decksCounter.text = $"{_prematch.DecksList.Count}/5";
        }

        private void CardDespawned(CardUI card)
        {
        }

        private void CardSpawned(CardUI card)
        {
            
        }

        public void ShowDecksList()
        {
            IsDeckSelected = false;
            DisableAllCardsBlockRaycast();
            _animator.SetBool("ShowDecks", true);
            _prematch.DeselectDeck();
        }

        private async void DeckChanged(DeckModel newDeck)
        {
            _cardsCounter.text = $"{newDeck.cards.Length}/{DeckModel.MaxCards}";
            _cards.Clear();
//            _animator.SetBool("ShowDecks", false);
            foreach (var spawnedCard in _spawnedCards.Values)
            {

                spawnedCard.transform.SetParent(_allCardsContentRoot);
                spawnedCard.transform.localScale = Vector3.one;
                
            }
            

            foreach(var card in _deckCards.Values)
            {
                _deckPlaceholdersPool.Despawn(card);
            }
            _deckCards.Clear();


            var allCards = await _cardsConfig.GetAllCards();
            foreach (var cardId in newDeck.cards)
            {
                DeckCardPlaceholder minimizedCard;

                if (_deckCards.ContainsKey(cardId))
                {
                    minimizedCard = _deckCards[cardId];

                }
                else
                {
                    minimizedCard = _deckPlaceholdersPool.Spawn();// _cardsPool.Spawn();// _spawnedCards[cardId];


                    var cardConfig = allCards.FirstOrDefault(c => c.id == cardId);
                    minimizedCard.Init(cardConfig);
                    minimizedCard.transform.SetParent(_deckCardsRoot);

                    minimizedCard.transform.localScale = Vector3.one;
//                    _cards.Add(minimizedCard);
                    _deckCards.Add(cardId, minimizedCard);
                }
            }
        }

        private void UpdateControls()
        {
  //          _textCount.text = $"{_cards.Count}/{_maxElementCount}";
  //          _completeButton.enabled = _cards.Count == _maxElementCount;
        }

        public void Close()
        {
            Hide();
        }

        private void OnDestroy()
        {
            _prematch.DeckChanged -= DeckChanged;
        }
    }
}