using Goons.Model;
using Goons.Network;
using Goons.ScenesManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
namespace Goons.UI
{
    public class StartWindow : BaseWindow
    {
        [Inject]
        private IUiSystem _uiSystem;
        [Inject]
        private IScenesManager _scenesManager;
        [Inject]
        private IPrematch _prematch;

        [SerializeField]
        private TMP_Text _deckToBattle;


        [SerializeField]
        private TMP_Text _nickName;

        private Paginator _paginator;

        public async void BattleButtonClicked()
        {
            await _scenesManager.LoadBattle();
            Hide();
        }

        protected override void Awake()
        {
            base.Awake();
            _paginator = GetComponentInChildren<Paginator>();
        }

        private void Start()
        {
            _prematch.DecksListReceived += DecksListReceived;

        }

        private void DecksListReceived()
        {
            _prematch.DecksListReceived -= DecksListReceived;

            if (_prematch.DecksList.Count > 0)
            {
                _prematch.SelectDeckToBattle(_prematch.DecksList[0]);

                _deckToBattle.text = _prematch.DeckToBattle.name;
            }
        }

        public void PrevDeckButtonClicked()
        {
            var currentDeckIndex = _prematch.DecksList.IndexOf(_prematch.DeckToBattle);
            if (currentDeckIndex == 0)
                currentDeckIndex = _prematch.DecksList.Count - 1;
            else --currentDeckIndex;
            var prevDeck = _prematch.DecksList[currentDeckIndex];
            _prematch.SelectDeckToBattle(prevDeck);
            _deckToBattle.text = _prematch.DeckToBattle.name;
            _paginator.SetPage(currentDeckIndex);
        }

        public void NextDeckButtonClicked()
        {
            var currentDeckIndex = _prematch.DecksList.IndexOf(_prematch.DeckToBattle);
            if (currentDeckIndex >= _prematch.DecksList.Count - 1)
                currentDeckIndex = 0;
            else
                ++currentDeckIndex;
            var nextDeck = _prematch.DecksList[currentDeckIndex];
            _prematch.SelectDeckToBattle(nextDeck);
            _deckToBattle.text = _prematch.DeckToBattle.name;
            _paginator.SetPage(currentDeckIndex);
        }

    }
}