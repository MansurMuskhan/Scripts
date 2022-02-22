using System;
using System.Collections.Generic;

namespace Goons.Model
{
    [Serializable]
    public class DeckModel
    {
        public int id;
        public string name;
        public int order;
        public int[] cards;

        public const int MaxCards = 30;
    }


}