using Goons.Model;
using System;
using UnityEngine;

namespace Goons.Config
{
    public enum CardType
    {
        None = 0,
        Attacker    = 0x001,
        Spell       = 0x010,
        Mystery     = 0x100
    }

    public enum Rarity
    {
        None = 0,
        Common =    0x0001,
        Rare    =   0x0010,
        Epic    =   0x0100,
        Legendary=  0x1000
    }

    [Serializable]
    public class CardConfig
    {
        public int Id => id;
        public Sprite Sprite { get; private set; }
        public Sprite Frame { get; private set; }

        private CardType _cardType = CardType.None;
        private Rarity _rarity = Rarity.None;

        public Rarity Rarity
        {
            get
            {
                if(_rarity == Rarity.None)
                {
                    switch (rarity)
                    {
                        case "common":
                            _rarity = Rarity.Common;
                                break;
                        case "rare":
                            _rarity = Rarity.Rare;
                            break;
                        case "epic":
                            _rarity = Rarity.Epic;
                            break;
                        case "legendary":
                            _rarity = Rarity.Legendary;
                            break;
                    }
                }
                return _rarity;
            }
        }

        public CardType CardType
        {
            get
            {
                if(_cardType == CardType.None)
                {
                    switch (type)
                    {
                        case "minion":
                            _cardType = CardType.Attacker;
                            break;
                        case "spell_card":
                            _cardType = CardType.Spell;
                            break;
                        default:
                            _cardType = CardType.Mystery;
                            break;
                    }
                }
                return _cardType;
            }
        }

        private bool _cardElementInitialized;
        private CardElements _cardElement ;
        public CardElements CardElement
        {
            get
            {
                if (!_cardElementInitialized)
                {
                    switch (element)
                    {
                        case "water":
                            _cardElement = CardElements.Water;
                            break;
                        case "electric":
                            _cardElement = CardElements.Electric;
                            break;

                        case "neutral":
                            _cardElement = CardElements.Neutral;
                            break;

                        case "fire":
                            _cardElement = CardElements.Fire;
                            break;
                        case "earth":
                            _cardElement = CardElements.Earth;
                            break;
                    }
                    _cardElementInitialized = true;
                }
                return _cardElement;
            }
        }

        public int id;
        public string name;
        public string rarity;
        public string quality;
        public string description;
        public int hp;
        public int quantity;
        public string type;
        public int attack;
        public string element;
        public string card_image;
        public string card_image_frame;


    }
}