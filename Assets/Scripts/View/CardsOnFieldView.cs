using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Zenject;
using Goons.Model;
using System;

namespace Goons.View
{
    public class CardsOnFieldView : MonoBehaviour
    {
        [Inject]
        private IGameModel _gameModel;

        [Inject]
        private GameView _gameView;

        [Inject]
        private CardView.Pool _cardViewPrefabsPool;

        [SerializeField]
        private CardSlot[] _slots;





        public CardSlot GetFreeSlot()
        {
            return _slots.FirstOrDefault(s => !s.IsBusy);
        }

        //public void PlaceCard(int cardId)
        //{
        //    var cardView = _gameView.SpawnedCards[cardId];
        //    var t = GetFreeSlot();
        //    cardView.transform.SetParent(t);
        //    cardView.transform.localScale = Vector3.one;
        //    cardView.transform.DOLocalMove(Vector3.zero, 1f);
        //}

        //public void DrawCard(CardModel cardModel)
        //{
        //    var t = GetFirstFreePosition();
        //    if (t != null)
        //    {
        //        var card = _cardViewPrefabsPool.Spawn();
        //        card.CardId = cardModel.CardId;
        //        _gameView.SpawnedCards.Add(cardModel.CardId, card);
        //        card.transform.position = _gameView.PlayerCordPosition.position;

        //        card.transform.SetParent(t);
        //        card.transform.localScale = Vector3.one;
        //        card.transform.DOLocalMove( Vector3.zero, 1f);
        //    }
        //}

    }
}