using System;
using System.Collections.Generic;
using UnityEngine;

namespace Goons.Model
{
    [Serializable]
    public class GameState
    {
        public event Action GameStateChanged;
        //public PlayerState PlayerState = new PlayerState();
        //public PlayerState OpponentState = new PlayerState();
        public void UpdateGameState(GameState newGameState)
        {
                       Cards.Clear();
            foreach (var card in newGameState.CardsJList) 
            {
                Cards.Add(card.CardId, card);
            }
            GameStateChanged?.Invoke();
        }

        public Dictionary<int, CardModel> Cards = new Dictionary<int, CardModel>();

        public List<CardModel> CardsJList = new List<CardModel>();

        public void Serialize()
        {
            var json = JsonUtility.ToJson(this);
        }

    }
}