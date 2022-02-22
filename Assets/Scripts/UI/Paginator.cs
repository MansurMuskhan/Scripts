using Goons.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Goons.UI
{
    public class Paginator : MonoBehaviour
    {
        [Inject]
        private IPrematch _prematch;

        private ToggleGroup _toggleGroup;

        private Toggle[] _toggles;

        [SerializeField]
        private Toggle _deckPaginationItemPrefab;

        private void Awake()
        {
            _toggleGroup = GetComponent<ToggleGroup>();
        }

        void Start()
        {
            _prematch.DecksListReceived += DecksListReceived;
        }

        public void SetPage(int index)
        {
            _toggles[index].isOn = true;
        }

        private void DecksListReceived()
        {
            var decksConut = _prematch.DecksList.Count;
            if (decksConut == 0)
                return;

            _toggles = new Toggle[decksConut];
            var i = 0;
            foreach(var deck in _prematch.DecksList)
            {
                var paginatorToggle = Instantiate<Toggle>(_deckPaginationItemPrefab);
                paginatorToggle.transform.SetParent(transform);
                paginatorToggle.transform.localScale = Vector3.one;
                paginatorToggle.group = _toggleGroup;
                _toggles[i++] = paginatorToggle;
            }
            _toggles[0].isOn = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}