using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
namespace Goons.View
{
    public class BattleField : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        [Inject]
        private GameView _gameView;
        [Inject]
        private IGameModel _gameModel;
        public void OnDrop(PointerEventData eventData)
        {
            if (_gameView != null)
            {

            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}