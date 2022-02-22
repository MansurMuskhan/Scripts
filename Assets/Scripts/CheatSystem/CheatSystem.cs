using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using System.Linq;


namespace Goons.Debug
{
    public interface ICheatSystem
    {
        void DrawCardByPlayer();
        void DrawCardByOpponent();
        void PlaceCardByOpponent();
        void MoveRandomPlayerCardToGraveyard();
        void MoveRandomOpponentCardToGraveyard();
        void MoveRandomCardFromGraveyardToPlayerDesk();
        void MoveRandomCardFromGraveyardToOpponentDesk();
        void MoveRandomCardFromGraveyardToPlayerHand();
        void MoveRandomCardFromGraveyardToOpponentHand();

    }
    public class CheatSystem : ICheatSystem
    {
        private IGameModel _gameModel;
        //private IUiSystem _uiSystem;

        public CheatSystem(IGameModel gameModel)
        {
            _gameModel = gameModel;
      //      Start();

        }

        private int _lastCardId = 0;
        public int GetNextCardId()
        {
            ++_lastCardId;
            return _lastCardId;
        }

        private async void Start()
        {
            await Task.Delay(1000);
            for (var i = 0; i < 5; ++i)
            {
                var cardModel = _gameModel.CreateCardModel(GetNextCardId());
                cardModel.IsMine = true;
                await _gameModel.TakeCardByPlayer(cardModel);

            }
            for (var i = 0; i < 5; ++i)
            {
                var cardModel = _gameModel.CreateCardModel(GetNextCardId());
                await _gameModel.TakeCardByOpponent(cardModel);

            }
        }

        public void DrawCardByPlayer()
        {
            var card = _gameModel.CreateCardModel(GetNextCardId());
            card.IsMine = true;
            _gameModel.TakeCardByPlayer(card);
        }
        public void DrawCardByOpponent()
        {
            var card = _gameModel.CreateCardModel(GetNextCardId());
            card.IsMine = false;
            _gameModel.TakeCardByOpponent(card);
        }
        public void PlaceCardByOpponent()
        {
            var opponentCards = _gameModel.Cards.Values.Where(c => !c.IsMine && c.State == CardStates.Hand);
            var card = opponentCards.ElementAt(Random.Range(0, opponentCards.Count()));
            _gameModel.PlaceCardByOpponent(card.CardId);
        }
        public void MoveRandomPlayerCardToGraveyard()
        {
            var opponentCards = _gameModel.Cards.Values.Where(c => c.IsMine);
            var card = opponentCards.ElementAt(Random.Range(0, opponentCards.Count()));
            _gameModel.MoveToGraveyard(card.CardId);
        }
        public void MoveRandomOpponentCardToGraveyard()
        {
            var opponentCards = _gameModel.Cards.Values.Where(c => !c.IsMine);
            var card = opponentCards.ElementAt(Random.Range(0, opponentCards.Count()));
            _gameModel.MoveToGraveyard(card.CardId);
        }
        public void MoveRandomCardFromGraveyardToPlayerDesk()
        {
            var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard && c.IsMine);
            var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
            _gameModel.MoveFromGraveyardToDesk(card.CardId);
        }
        public void MoveRandomCardFromGraveyardToOpponentDesk()
        {
            var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard && !c.IsMine);
            var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
            _gameModel.MoveFromGraveyardToDesk(card.CardId);
        }
        public void MoveRandomCardFromGraveyardToPlayerHand()
        {
            var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard && c.IsMine);
            var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
            _gameModel.MoveFromGraveyardToHand(card.CardId);
        }
        public void MoveRandomCardFromGraveyardToOpponentHand()
        {
            var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard && !c.IsMine);
            var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
            _gameModel.MoveFromGraveyardToHand(card.CardId);
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _gameModel.TakeCardByOpponent(_gameModel.CreateCardModel(GetNextCardId()));
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                var opponentCards = _gameModel.Cards.Values.Where(c => !c.IsMine);
                var card = opponentCards.ElementAt(Random.Range(0, opponentCards.Count()));
                _gameModel.PlaceCardByOpponent(card.CardId);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                var opponentCards = _gameModel.Cards.Values;//.Where(c => !c.IsMine);
                var card = opponentCards.ElementAt(Random.Range(0, opponentCards.Count()));
                _gameModel.MoveToGraveyard(card.CardId);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard);
                var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
                _gameModel.MoveFromGraveyardToDesk(card.CardId);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                var deadCards = _gameModel.Cards.Values.Where(c => c.State == CardStates.Graveyard);
                var card = deadCards.ElementAt(Random.Range(0, deadCards.Count()));
                _gameModel.MoveFromGraveyardToHand(card.CardId);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var newGameState = new GameState();
                newGameState.CardsJList = _gameModel.GameState.CardsJList;

                newGameState.CardsJList[0].State = CardStates.Desk;

        //        var json = JsonUtility.ToJson(_gameModel.GameState);

                _gameModel.GameState.UpdateGameState(newGameState);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                var cardsList = _gameModel.Cards.Values.Where(c => c.IsMine && c.State == CardStates.Graveyard).ToList();
 //               _uiSystem.ShowCardsWindow(cardsList);
            }
        }
    }
}