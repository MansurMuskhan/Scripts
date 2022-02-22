using Goons.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Linq;
using System.Collections;
using Goons.Model;

namespace Goons.Config
{
    public class NetworkCardsConfig : ICardsConfig
    {
        public Dictionary<int, CardConfig> CashedCards;

        [Serializable]
        private class ConfigsList
        {
            public List<CardConfig> Configs;
        }
        private INetworkInteractions _newworkInteractions;
        public NetworkCardsConfig(INetworkInteractions ni)
        {
            _newworkInteractions = ni;
        }

        public async Task<CardConfig[]> GetAllCardsFiltered(CardElements elementFilter, Rarity rarityFilter, CardType cardTypeFilter, string searchString)
        {
            var cards = await GetAllCards();
            Func<CardConfig, bool> elementPredicate = c => {
                if (elementFilter == 0)
                    return true;
                var result = c.CardElement & elementFilter;
                return result>0;
            };
            Func<CardConfig, bool> rarityPredicate = c => {
                if (rarityFilter == 0)
                    return true;
                var result = c.Rarity & rarityFilter;
                return result > 0;
            };
            Func<CardConfig, bool> typePredicate = c => {
                if (cardTypeFilter == 0)
                    return true;
                var result = c.CardType & cardTypeFilter;
                return result > 0;
            };

            var filtered = cards.Where(elementPredicate).Where(rarityPredicate).Where(typePredicate);
            if (!string.IsNullOrEmpty(searchString))
            {
                filtered = filtered.Where(c => c.name.ToLower().Contains(searchString.ToLower() ));
            }
            return filtered.ToArray();
        }

        

        public async Task<CardConfig[]> GetAllCards()
        {
            if (CashedCards == null)
            {
                CashedCards = new Dictionary<int, CardConfig>();
                var url = "https://dev-api.getagoon.com/api/v1/list_cards";

                var responseText = await _newworkInteractions.SentGetRequest(url);

                var cardsList = JsonConvert.DeserializeObject<CardConfig[]>(responseText);
                foreach(var card in cardsList)
                {
                    CashedCards.Add(card.id, card);

                }
            }
            return CashedCards.Values.ToArray();
        }

        public Task<Sprite> GetArtSprite(int id)
        {
            var fileName = Path.GetFileNameWithoutExtension(CashedCards[id].card_image);
            var sprite =  Resources.Load<Sprite>($"Sprites/cards/{fileName}");
            return Task.FromResult(sprite);
        }

        public Task<Sprite> GetCardBackside()
        {
            return Task.FromResult<Sprite>(null);
        }

        public Task<Sprite> GetFrameSprite(int id)
        {
            var cardConfig = CashedCards[id];
            var spriteName = "";
            switch (cardConfig.element)
            {
                case "water" :
                    if (cardConfig.type == "minion")
                        spriteName = "Water (minion)";
                    else 
                        spriteName = "Waterl (spell)";

                    break;
                case "electric":
                    if (cardConfig.type == "minion")
                        spriteName = "Electric (minion)";
                    else
                        spriteName = "Electric (spell)";
                    break;

                case "neutral":
                    if (cardConfig.type == "minion")
                        spriteName = "Neutral (minion)";
                    else
                        spriteName = "Neutral (spell)";
                    break;                
                
                case "fire":
                    if (cardConfig.type == "minion")
                        spriteName = "Fire (minion)";
                    else
                        spriteName = "Fire (spell)";
                    break;                
                case "earth":
                    if (cardConfig.type == "minion")
                        spriteName = "Earth (minion)";
                    else
                        spriteName = "Earth (spell)";
                    break;
            }
            var sprite = Resources.Load<Sprite>($"Sprites/frames/{spriteName}");
            return Task.FromResult(sprite);
        }
    }
}