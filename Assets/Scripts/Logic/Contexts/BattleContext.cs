using System;
using UnityEngine;
using Imperius.Data;
using System.Collections.Generic;

namespace Imperius.Logic
{
    [Serializable]
    public class BattleContext : MonoBehaviour
    {
        public static BattleContext Instance { get; private set; }
        public BattlePlayer Player;
        public BattlePlayer EnemyPlayer;

        public int roomId;

        public int currentRound;

        public BattleCharacter PlayerActiveChar;

        public BattleCharacter SelectedEnemyChar;

        public List<Card> PlayerHandPile;
        public List<Card> PlayerDrawPile;
        public List<Card> PlayerDiscardPile;


        public List<Card> EnemyHandPile;
        public List<Card> EnemyDrawPile;
        public List<Card> EnemyDiscardPile;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        // public enum PileType { Hand, Draw, Discard }
        // public enum PlayerType { Player, Enemy }


        // private List<Card> GetPile(PlayerType player, PileType pile)
        // {
        //     return player switch
        //     {
        //         PlayerType.Player => pile switch
        //         {
        //             PileType.Hand => PlayerHandPile,
        //             PileType.Draw => PlayerDrawPile,
        //             PileType.Discard => PlayerDiscardPile,
        //             _ => null
        //         },
        //         PlayerType.Enemy => pile switch
        //         {
        //             PileType.Hand => EnemyHandPile,
        //             PileType.Draw => EnemyDrawPile,
        //             PileType.Discard => EnemyDiscardPile,
        //             _ => null
        //         },
        //         _ => null
        //     };
        // }

        // public void AddCard(PlayerType player, PileType pile, Card card)
        // {
        //     var targetPile = GetPile(player, pile);
        //     if (targetPile == null || card == null) return;

        //     targetPile.Add(card);
        //     Debug.Log($"{card.Name} added to {player}'s {pile} pile.");
        // }


        // public void RemoveCard(PlayerType player, PileType pile, Card card)
        // {
        //     var targetPile = GetPile(player, pile);
        //     if (targetPile == null || card == null) return;

        //     if (targetPile.Remove(card))
        //         Debug.Log($"{card.Name} removed from {player}'s {pile} pile.");
        //     else
        //         Debug.LogWarning($"{card.Name} not found in {player}'s {pile} pile!");
        // }

        // public void DrawCard(PlayerType player)
        // {
        //     var drawPile = GetPile(player, PileType.Draw);
        //     var handPile = GetPile(player, PileType.Hand);

        //     if (drawPile == null || drawPile.Count == 0)
        //     {
        //         Debug.Log($"{player}'s draw pile is empty!");
        //         return;
        //     }

        //     // Pick a random index
        //     int randomIndex = UnityEngine.Random.Range(0, drawPile.Count);
        //     var card = drawPile[randomIndex];

        //     // Remove from draw pile and add to hand
        //     drawPile.RemoveAt(randomIndex);
        //     handPile.Add(card);

        //     Debug.Log($"{player} drew {card.Title} to hand.");
        // }


        // public void DiscardCard(PlayerType player, Card card)
        // {
        //     RemoveCard(player, PileType.Hand, card);
        //     AddCard(player, PileType.Discard, card);
        // }

        // public void FlushPile(PlayerType player, PileType pile)
        // {
        //     var sourcePile = GetPile(player, pile);
        //     if (sourcePile == null || sourcePile.Count == 0) return;

        //     List<Card> targetPile = null;

        //     switch (pile)
        //     {
        //         case PileType.Discard:
        //             targetPile = GetPile(player, PileType.Draw);
        //             break;
        //         case PileType.Hand:
        //             targetPile = GetPile(player, PileType.Discard);
        //             break;
        //         case PileType.Draw:
        //             targetPile = GetPile(player, PileType.Hand);
        //             break;
        //     }

        //     if (targetPile != null)
        //     {
        //         targetPile.AddRange(sourcePile);
        //         sourcePile.Clear();
        //         Debug.Log($"{player}'s {pile} pile flushed into {targetPile}");
        //     }
        // }



        // public void CreateHand(PlayerType player, int handSize)
        // {
        //     for (int i = 0; i < handSize; i++)
        //     {
        //         DrawCard(player);
        //     }
        // }

        // public void PrintPile(PlayerType player, PileType pile)
        // {
        //     var targetPile = GetPile(player, pile);
        //     string pileContent = targetPile.Count > 0 ? string.Join(", ", targetPile) : "Empty";
        //     Debug.Log($"{player}'s {pile} pile: {pileContent}");
        // }

    }
}