using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
namespace Goons.View
{
    public class PlayerView : MonoBehaviour, IDropHandler
    {
        public bool IsMine;

        [Inject]
        private IGameModel _gameModel;

        [Inject]
        private GameView _gameView;

        public void OnDrop(PointerEventData eventData)
        {
            _gameView.StopDrawingAttackTarget();
            if (eventData.pointerDrag.gameObject != null)
            {
                var playerCardView = eventData.pointerDrag.gameObject.GetComponent<CardView>();
                if (playerCardView != null && playerCardView.IsMine && playerCardView.CardModel.State == CardStates.Desk)
                {
                    _gameModel.AttasPlayer(playerCardView.CardModel);
                    return;
                }
            }
        }
    }
}