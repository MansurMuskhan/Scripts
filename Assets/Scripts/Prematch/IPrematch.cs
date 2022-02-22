using Goons.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Goons.Network
{
    public interface IPrematch
    {
        Task<DeckModel[]> GetAllDecks();

        DeckModel CurrentDeck { get; }
        void SetCurrentDeck(DeckModel currendDeck);
        Task RenameDeck(DeckModel deckModel);
        Task AddCardToDeck(int cardId);
        Task RemoveCardFromDeck(int cardId);
        Task CreateNewDeck();
        List<DeckModel> DecksList { get; }
        void SelectDeckToBattle(DeckModel deck);
        DeckModel DeckToBattle { get; }
        Task<IReadOnlyDictionary<int,Sprite>> GetHeads();
        Task<IReadOnlyDictionary<int, BodyTraits>> GetBodies();
        Task<Sprite> GetFullBody(int id);
        Task<BodyTraits> GetBodyTraits(int id);
        Task<int[]> GetUserAvatarIds();
        void DeselectDeck();

        event Action<DeckModel> DeckSelected;
        event Action DeckDeselected;
        event Action<DeckModel> DeckChanged;
        event Action<List<DeckModel>> DecksListChanged;
        event Action DecksListReceived;

    }
}