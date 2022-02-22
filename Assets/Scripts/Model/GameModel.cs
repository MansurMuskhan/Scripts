using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace Goons.Model
{

    public interface IGameModel
    {
        GameState GameState { get; }
        event Action ModelChanged;
        event Action ModelInitialized;

        event Action<CardModel> CardTakenByPlayer;
        event Action<CardModel> CardTakenByOpponent;

        event Action<int> CardPlacedByPlayer;
        event Action<int> CardPlacedByOpponent;

        event Action<int> CardMovedToGraveyard;
        event Action<int> CardMovedFromGraveyardToHand;
        event Action<int> CardMovedFromGraveyardToDesk;

        event Action<int, int> Attacked;
        event Action<int> OpponentAttacked;

        Task<bool> PlaceCardByPlayer(int card);
        Task<bool> PlaceCardByOpponent(int card);

        Task TakeCardByPlayer(CardModel card);
        Task TakeCardByOpponent(CardModel card);

        Task MoveToGraveyard(int cardId);

        Task MoveFromGraveyardToHand(int cardId);
        Task MoveFromGraveyardToDesk(int cardId);


        Task Attack(CardModel attacker, CardModel target);
        Task AttasPlayer(CardModel attacker);

        CardModel CreateCardModel(int id);

        IReadOnlyDictionary<int , CardModel> Cards { get; }

    }
    public class GameModel:IGameModel
    {
        public GameState GameState { get; private set; }

        public event Action ModelChanged;
        public event Action ModelInitialized;
        public event Action<int> CardPlacedByPlayer;
        public event Action<int> CardPlacedByOpponent;
        public event Action<int> CardMovedToGraveyard;
        public event Action<CardModel> CardTakenByPlayer;
        public event Action<CardModel> CardTakenByOpponent;
        public event Action<int, int> Attacked;
        public event Action<int> OpponentAttacked;
        public event Action<int> CardMovedFromGraveyardToHand;
        public event Action<int> CardMovedFromGraveyardToDesk;

        public PlayerInfo Player { get; set; }
        public PlayerInfo Opponent { get; set; }

        private IGameMessagesSystem _gameMessagesSystem;

        public IReadOnlyDictionary<int, CardModel> Cards => GameState.Cards;

        public GameModel(IGameMessagesSystem gameMessagesSystem)
        {
            _gameMessagesSystem = gameMessagesSystem;
            GameState = new GameState();

            ModelInitialized?.Invoke();
        }

        public CardModel CreateCardModel(int id)
        {
            var c = new CardModel(id);
            GameState.Cards.Add(id, c);
            GameState.CardsJList.Add(c);
            
            return c;
        }

        public Task MoveFromGraveyardToHand(int cardId)
        {
            var cardModel = Cards[cardId];
            cardModel.State = CardStates.Hand;
            CardMovedFromGraveyardToHand?.Invoke(cardId);
            return Task.CompletedTask;
        }
        public Task MoveFromGraveyardToDesk(int cardId)
        {
            var cardModel = Cards[cardId];
            cardModel.State = CardStates.Desk;
            CardMovedFromGraveyardToDesk?.Invoke(cardId);
            return Task.CompletedTask;
        }

        public Task Attack(CardModel attacker, CardModel target)
        {
            Attacked?.Invoke(attacker.CardId, target.CardId);
            return Task.CompletedTask;
        }

        public Task AttasPlayer(CardModel attacker)
        {
            OpponentAttacked?.Invoke(attacker.CardId);
            return Task.CompletedTask;
        }

        public Task<bool> PlaceCardByPlayer(int cardId)
        {
            //TOTO: add client-server interaction to check if this is correct action
            var result = true;
            
            if (!result)
            {
                _gameMessagesSystem.SendGameMessage("This action is not allowed");
                return Task.FromResult<bool>(false);
            }
            else
            {

                var cardModel = Cards[cardId];
                cardModel.State = CardStates.Desk;
                CardPlacedByPlayer?.Invoke(cardId);

                //TODO: remove this when client-server interaction is done
                cardModel.AddStatus(new CardSleepEffect());

                return Task.FromResult<bool>(true);
            }
        }

        public Task MoveToGraveyard(int cardId)
        {
            var card = Cards[cardId];
            card.State = CardStates.Graveyard;
            CardMovedToGraveyard?.Invoke(cardId);
            return Task.CompletedTask;
        }

        public Task<bool> PlaceCardByOpponent(int cardId)
        {
            var card = Cards[cardId];
            card.State = CardStates.Desk;
            CardPlacedByOpponent?.Invoke(cardId);

            return Task.FromResult<bool>(true);
        }


        public Task TakeCardByPlayer(CardModel card)
        {
            CardTakenByPlayer?.Invoke(card);
            return Task.CompletedTask;
        }

        public Task TakeCardByOpponent(CardModel card)
        {
            CardTakenByOpponent?.Invoke(card);
            return Task.CompletedTask;
        }
    }
}