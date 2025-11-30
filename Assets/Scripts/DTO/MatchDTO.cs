using System;
using System.Collections.Generic;
using Imperius.Logic;

namespace Imperius.Data
{
    [Serializable]
    public class MatchDTO
    {
   public string RoomId;
    public string MatchId;
    public string EnemyName;

    public string PlayerName; // name

    public List<Character> PlayerTeam;
    public List<Character> EnemyTeam;

    public Deck PlayerDeck;
    public Deck EnemyDeck;

    public string ActivePlayer;
    }
}
