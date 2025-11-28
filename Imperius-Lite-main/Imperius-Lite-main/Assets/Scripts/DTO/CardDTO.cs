using System;
using Imperius.Logic;

namespace Imperius.Data
{
    [Serializable]
    public class CardDTO
    {
        public int id;
        public string name;
        public Element Element;
        public string Type;
        public int Energy;
        public string DiscardType;
        public string Description;
        public int Damage;
        public int Tier;
    }
}
