using System; 
using System.Collections.Generic;
using Imperius.Logic;

namespace Imperius.Data
{
    public class PlayerDTO
    {
        public List<Character> PlayerCharactersList;
    
        public List<Card> PlayerCardsList;

        public List<Deck> DecksList;

        public int PlayerLevel;
    }

}