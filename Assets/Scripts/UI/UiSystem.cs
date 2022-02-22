using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Goons.UI
{
    public interface IUiSystem
    {
        void ShowCardsWindow(List<CardModel> cards);
        IHidableWindow ShowCardInfoWindow(CardModel cardModel);
        void ShowDeckBuilder();
        void ShowStartWindow();
        void ShowCharacterWindow();
    }

    public interface IHidableWindow
    {
        void Hide();
    }
    public class UiSystem : MonoBehaviour, IUiSystem
    {
        [Inject]
        private IGameModel _gameModel;

        [Inject]
        private IGameMessagesSystem _gameMessagesSystem;

        [Inject]
        private IHidableWindow[] _hidableWindows;

        [Inject]
        private CharacterWindow _characterWindow;

        [Inject]
        private StartWindow _startWindow;

        [SerializeField]
        private SelectCardWindow _selectCardWindow;

        [SerializeField]
        private GameMessageWindow _gameMessageWindow;

        [SerializeField]
        private CardInfoWindow _cardInfoWindow;

        [SerializeField]
        private NextTurnWindow _nextTurnWindow;

        [SerializeField]
        private VersusWindow _versusWindow;

        [SerializeField]
        private ResultWindow _resultWindow;

        [SerializeField]
        private DeckWindow _deckWindow;

        public void ShowStartWindow()
        {
            HideAllWindows();
            _startWindow.Show();
        }

        public void ShowCharacterWindow()
        {
            HideAllWindows();
            _characterWindow.Show();
        }

        private void HideAllWindows()
        {
            foreach (var hidableWindow in _hidableWindows)
                hidableWindow.Hide();
        }

        public void ShowDeckBuilder()
        {
            HideAllWindows();
            _deckWindow.Show();
        }

        public void ShowCardsWindow(List<CardModel> cards)
        {
            _selectCardWindow.Show(cards);
        }

        public IHidableWindow ShowCardInfoWindow(CardModel cardModel)
        {
            _cardInfoWindow.Show(cardModel);
            return _cardInfoWindow;
        }

        public void ShowGameMessage(string message)
        {
            _gameMessageWindow.Show(message);
        }

        // Start is called before the first frame update
        void Start()
        {
            _gameMessagesSystem.GameMessageReceived += GameMessageReceived;
            //     _gameModel.
        }

        private void GameMessageReceived(string message)
        {
            _gameMessageWindow.Show(message);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}