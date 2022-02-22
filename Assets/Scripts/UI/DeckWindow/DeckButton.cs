using Goons.Model;
using Goons.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Goons.UI {
    public class DeckButton : MonoBehaviour
    {
        [Inject]
        private IPrematch _prematch;
        
        [SerializeField]
        private TMP_Text _text;

        [SerializeField]
        private TMP_Text _cardsCountText;

        [SerializeField]
        private GameObject _empty;

        [SerializeField]
        private GameObject _full;

        private DeckModel _deckModel;

        public event Action<DeckModel> Selected;

        public void Click()
        {
            Selected?.Invoke(_deckModel);
        }


        public void Init(DeckModel deckModel)
        {
            _prematch.DeckChanged += DeckChanged;
            _deckModel = deckModel;
            UpdateView();

        }

        public void UpdateView()
        {
            _text.text = _deckModel.name;
            _empty.SetActive(false);
            _full.SetActive(true);
            _cardsCountText.text = $"{_deckModel.cards.Length}/{DeckModel.MaxCards}";
        }

        private void DeckChanged(DeckModel changedDeck)
        {
            if(changedDeck == _deckModel)
            {
                UpdateView();
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Pool : MemoryPool<DeckButton>
        {

        }
    }
}