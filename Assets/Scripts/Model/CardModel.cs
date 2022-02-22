using System;
using System.Collections.Generic;

namespace Goons.Model
{
    public class CardEffect
    {

    }
    public class CardSleepEffect : CardEffect
    {

    }

    public enum CardElements
    {
        Earth      =  0x00001,
        Electric   =  0x00010,
        Water      =  0x00100,
        Fire       =  0x01000,
        Neutral    =  0x10000
    }

    [Serializable]
    public class CardModel
    {
        public event Action Changed;

        public int CardId;
        public bool IsMine;

        private CardStates _state;
        public CardStates State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                    return;
                _state = value;
                Changed?.Invoke();
            }
        }
        public List <CardEffect> CardEffects =new List<CardEffect>();
 //       public bool IsActive = true;

        internal CardModel(int cardId)
        {
            CardId = cardId;
        }

        public void AddStatus(CardEffect status)
        {
            CardEffects.Add(status);
            Changed?.Invoke();
        }


    }


    public class AttackingCardModel : CardModel
    {
        
        internal AttackingCardModel(int cardId):base(cardId)   { }
    }
}