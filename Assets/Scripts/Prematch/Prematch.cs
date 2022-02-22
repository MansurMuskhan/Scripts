using Goons.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Goons.Network
{
    public class BodyTraits
    {
        public int Id;
        public Sprite BodyPreview;
        public Sprite Pants;
        public Sprite Shoes;
        public Sprite Belt;
        public Sprite Weapon;
        public Sprite Acessory;
    }
    public class Prematch : IPrematch
    {

        private class AvatarsResponce
        {
            public int[] user_avatars;
        }
        private class NewDeckModel
        {
            public string name;
            public int order;
            public int[] cards;
        }
        private class UpdateDeckModel
        {
            public int custom_deck_id;
            public string name;
            public int order;
            public int[] cards;
        }

        private INetworkInteractions _networkInteractions;
        private ICustomizationImagesConfig _customizationImagesConfig;

        private List<DeckModel> _decksList;
        public List<DeckModel> DecksList => _decksList;

        public event Action<DeckModel> DeckSelected;
        public event Action DeckDeselected;
        public event Action<DeckModel> DeckChanged;
        public event Action<List<DeckModel>> DecksListChanged;
        public event Action DecksListReceived;

        public DeckModel CurrentDeck { get; private set; }
        public void SetCurrentDeck(DeckModel currendDeck)
        {
            CurrentDeck = currendDeck;
            DeckSelected?.Invoke(CurrentDeck);
            DeckChanged?.Invoke(CurrentDeck);
        }

        public void DeselectDeck()
        {
            DeckDeselected?.Invoke();
        }

        public Prematch(INetworkInteractions networkInteractions, ICustomizationImagesConfig customizationImagesConfig)
        {
            _networkInteractions = networkInteractions;
            _customizationImagesConfig = customizationImagesConfig;
        }

        private int GetFreeDeckOrder()
        {
            if (_decksList.Count == 0)
                return 1;
            return _decksList.Max(d => d.order) + 1;
        }

        public async Task RenameDeck(DeckModel deckModel)
        {
            await ChangeDeckAsync(new UpdateDeckModel() { custom_deck_id = deckModel.id, name = deckModel.name, order = deckModel.order, cards = deckModel.cards });
        }

        public void SelectDeckToBattle(DeckModel deck)
        {
            DeckToBattle = deck;
        }
        public DeckModel DeckToBattle { get; private set; }

        public async Task CreateNewDeck()
        {


            var newOrder = GetFreeDeckOrder();
            var newDeckModel = new NewDeckModel() { name = "New Deck", cards = new int[0], order = newOrder };
            var json = JsonUtility.ToJson(newDeckModel);
            var result = await _networkInteractions.SentPostRequest("https://dev-api.getagoon.com/api/v1/create_custom_deck", json);// httpWebRequest.GetResponseAsync();

            var updatedDeck = JsonUtility.FromJson<DeckModel>(result);
            _decksList.Add(updatedDeck);
            DecksListChanged.Invoke(_decksList);

            SetCurrentDeck(updatedDeck);
        }

        private async Task ChangeDeckAsync(UpdateDeckModel newDeckModel)
        {
            var json = JsonUtility.ToJson(newDeckModel);

            var result = await _networkInteractions.SentPostRequest("https://dev-api.getagoon.com/api/v1/update_custom_deck", json);
            try
            {
                var updatedDeck = JsonUtility.FromJson<UpdateDeckModel>(result);
                CurrentDeck.cards = updatedDeck.cards;
                DeckChanged?.Invoke(CurrentDeck);
            }
            catch(Exception e)
            {
                Debug.Log($"Change deck failed with message {result}");
            }
        }

        public async Task RemoveCardFromDeck(int cardId)
        {
            var cardsInDeck = CurrentDeck.cards.ToList();
            if (!cardsInDeck.Contains(cardId))
                return;

            cardsInDeck.Remove(cardId);

            await ChangeDeckAsync(new UpdateDeckModel() { custom_deck_id = CurrentDeck.id, name = CurrentDeck.name, order = CurrentDeck.order, cards = cardsInDeck.ToArray() });

        }

        public async Task AddCardToDeck(int cardId)
        {
            var cardsInDeck = CurrentDeck.cards.ToList();
            cardsInDeck.Add(cardId);

            await ChangeDeckAsync(new UpdateDeckModel() { custom_deck_id = CurrentDeck.id, name = CurrentDeck.name, order = CurrentDeck.order, cards = cardsInDeck.ToArray() });
        }

        public async Task<DeckModel[]> GetAllDecks()
        {
            var url = "https://dev-api.getagoon.com/api/v1/list_custom_decks";

            var responseText = await _networkInteractions.SentGetRequest(url);
            var decksList = JsonConvert.DeserializeObject<DeckModel[]>(responseText);
            _decksList = decksList.ToList();
            DecksListReceived?.Invoke();
            return decksList;
        }

        private Dictionary<int, Sprite> _cashedBodies = new Dictionary<int, Sprite>();

        public async Task<Sprite> GetFullBody(int id)
        {
            if (!_cashedBodies.ContainsKey(id))
            {
                var fileName = _customizationImagesConfig.GetHeadFileNameById(id);
                var imgUrl = $"https://goonbodies.s3.amazonaws.com/Bodies/{fileName}";

                var downloadedSprite = await GetTexture(imgUrl);
                _cashedBodies.Add(id, downloadedSprite);
            }
            return _cashedBodies[id];

        }

        public async Task<BodyTraits> GetBodyTraits(int id)
        {
            var bodies = await GetBodies();
            return bodies[id];
        }

        private Dictionary<int, BodyTraits> _bodies;
        public async Task<IReadOnlyDictionary<int, BodyTraits>> GetBodies()
        {
            Debug.Log("Start downloading bodies");
            if (_bodies == null)
            {

                _bodies = new Dictionary<int, BodyTraits>();

                //   var id = 1;
                foreach (var id in new int[]{ 1,2,3,4,5})
                {
                    var url = $"https://goonbodies.s3.amazonaws.com/metadata/{id}";
                    var responseText = await _networkInteractions.SentGetRequest(url, false);
                    var json = JObject.Parse(responseText);
                    var imgUrl = json["image"].ToString();
                    var bodyPreview = await GetTexture(imgUrl);
                    var wearTraits = json["traitIds"].ToArray();

                    var bodyTraits = new BodyTraits() { BodyPreview = bodyPreview };
                    
                   
                    foreach (var traitId in wearTraits)
                    {
                        var intTraitId = traitId.Value<int>();// int.Parse(traitId.tos);
                        var bodyTraitsConfig = _customizationImagesConfig.GetBodyTraitsById(intTraitId);
                        if (bodyTraitsConfig == null)
                            continue;

                        var traitUrl = bodyTraitsConfig.Url;
                        switch (bodyTraitsConfig.Category)
                        {
                            case "Bottom":
                                bodyTraits.Pants = await GetTexture(traitUrl);
                                break;                            
                            case "Footwear":
                                bodyTraits.Shoes = await GetTexture(traitUrl);
                                break;
                            case "Belt":
                                bodyTraits.Belt = await GetTexture(traitUrl);
                                break;
                            case "Weapon":
                                bodyTraits.Weapon = await GetTexture(traitUrl);
                                break;
                            case "Accessory":
                                bodyTraits.Acessory = await GetTexture(traitUrl);
                                break;
                        }
                    }

                    


                    _bodies.Add(id, bodyTraits);
                }
            }
            Debug.Log("Bodies downloading completed");
            return _bodies;
        }

        private Dictionary<int, Sprite> _heads;
        public async Task<IReadOnlyDictionary<int, Sprite>> GetHeads()
        {
            Debug.Log("Start downloading heads");
            if (_heads == null)
            {
                var avatars = await GetUserAvatarIds();
                _heads = new Dictionary<int, Sprite>();
                foreach (var id in avatars)
                {
                    var fileName = _customizationImagesConfig.GetHeadFileNameById(id);
                    var url = $"https://goons-metadata.herokuapp.com/api/token/{fileName}";
                    var responseText = await _networkInteractions.SentGetRequest(url);
                    var json = JObject.Parse(responseText);
                    var imgUrl = json["image"].ToString();
                    var downloadedTexture = await GetTexture(imgUrl);
                    _heads.Add(id, downloadedTexture);
                }
            }
            Debug.Log("Heads download completed");
            return _heads;
        }

        private int[] _userAvatars;

        public async Task<int[]> GetUserAvatarIds()
        {
            if (_userAvatars == null)
            {
                await _networkInteractions.GetToken();
                var walletId = _networkInteractions.MetamaskToken;
                var url = $"https://store-api.goonsofbalatroon.com/api/v1/avatar_ids?wallet={walletId}";

                var responseText = await _networkInteractions.SentGetRequest(url, false);
                var result = JsonConvert.DeserializeObject<AvatarsResponce>(responseText);
                _userAvatars = result.user_avatars;
            }
            return  _userAvatars;
        }

        private async Task<Sprite> GetTexture(string url)
        {
            Debug.Log($"    Start download texture from {url}");
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                
                await uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    return null;
                 //   Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    Debug.Log($"    texture downloaded from {url}");
                    return sprite;
                }
            }
        }

    }
}