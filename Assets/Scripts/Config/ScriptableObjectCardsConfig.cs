using Goons.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Goons.Config
{

    [CreateAssetMenu(fileName = "CardsConfig", menuName = "Config/Cards Config")]
    public class ScriptableObjectCardsConfig : ScriptableObject, ICardsConfig
    {
        [SerializeField]
        private Sprite _cardBackSide;

        public CardConfig[] Cards;

        public Task<CardConfig[]> GetAllCardsFiltered(CardElements elementFilter, Rarity rarityFilter, CardType cardTypeFilter, string searchString)
        {
            return Task.FromResult(Cards);
        }

        public Task<CardConfig[]> GetAllCards()
        {
            return Task.FromResult(Cards);
        }
        public Task<Sprite> GetFrameSprite(int id)
        {
            return Task.FromResult(Cards[id].Frame);
        }
        public Task<Sprite> GetArtSprite(int id)
        {
            return Task.FromResult(Cards[id].Sprite);
        }      
        
        public Task<Sprite> GetCardBackside()
        {
            return Task.FromResult(_cardBackSide);
        }
    }
}