using Goons.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Goons.Config
{
    public interface ICardsConfig 
    {
        Task<Sprite> GetFrameSprite(int id);
        Task<Sprite> GetArtSprite(int id);
        Task<Sprite> GetCardBackside();
        Task<CardConfig[]> GetAllCards();
        Task<CardConfig[]> GetAllCardsFiltered(CardElements elementFilter, Rarity rarityFilter, CardType cardTypeFilter, string searchString);
    }
}