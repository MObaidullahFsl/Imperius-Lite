using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Imperius.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Imperius.Logic
{

    public class LobbyBL
    {
        static List<CardDTO> allCards;
        LobbyDL lobbyDL;

        Player Player;

        public LobbyBL()
        {
            // Read the JSON file into a string
            // string jsonString = Resources.Load<TextAsset>("Data/cards").text;

            // Deserialize into a list of CardData
            allCards = JsonUtility.FromJson<JsonWrapper>(Resources.Load<TextAsset>("Data/cards").text).cards;

            lobbyDL = new();
        }
        public async Task<Player> fetchPlayer()
        {

            var p = await lobbyDL.getPlayerFromDB(GlobalContext.Instance.GlobalUser);

            // naive approach to make default player if dl returns empty data
            if (p.PlayerCharactersList == null || p.PlayerCharactersList.Count == 0)
            {
                Debug.Log("dev solution make test player");
                Player newPlayer = new Player("default_player"); // -1 indicates guest or test player
                InitializePlayer(newPlayer);
                Player = newPlayer;
                return newPlayer;
            }
            else
            {
                Player existingPlayer = new Player(p);
                Player = existingPlayer;
                return existingPlayer;
            }

        }

        public async Task<bool> StartMatch(MatchDTO myMatch)
        {
            MatchDTO serverMatch = await lobbyDL.RequestMatchFromServer(myMatch);

            if (serverMatch == null)
            {
                Debug.Log("Matchmaking failed");
                return false;
            }

            Debug.Log("Enemy ID = " + serverMatch.EnemyName);

            MatchContext.Instance.EnemyDeck = serverMatch.EnemyDeck;
            MatchContext.Instance.EnemyTeam = serverMatch.EnemyTeam;
            MatchContext.Instance.EnemyName = serverMatch.EnemyName;
            MatchContext.Instance.ActivePlayer = (serverMatch.ActivePlayer == MatchContext.Instance.PlayerName) ? MatchContext.Instance.PlayerName : MatchContext.Instance.EnemyName;

            return true;
        }

        public async Task CancelMatch()
        {
            await lobbyDL.StopMatch();
        }

        public static void InitializePlayer(Player PlayerToInitialize)
        {
            //here u first get player characters and cards from either persistent storage or server

            GetDefaultCharacterList(PlayerToInitialize);

            // else get default 

            GetDefaultCardsList(PlayerToInitialize);

            // deck initialization here

            GetDefaultDeck(PlayerToInitialize);

            GetLevel(PlayerToInitialize);

        }

        static void GetDefaultCharacterList(Player PlayerAttrib)
        {
            Character c1 = new("default1", Element.Fire, 1, "Does literally nothing", 100);
            Character c2 = new("default2", Element.Fire, 1, "Does literally everything", 100);
            List<Character> clist = new()
        {
            c1,
            c2
        };

            PlayerAttrib.AddCharacters(clist);

        }

        static void GetDefaultCardsList(Player PlayerAttrib)
        {
            List<Card> clist = new();

            // i need to add a better card adding sys here 

            //Element[] elements = ElementUtility.GetAllElements(); this is way to get all element enums

            // we can add basic attack and basic defense cards by default for guests and new starters

            // lets add Basic Attack, Basic Defense and 5 random good cards
            // keeping cards no equal to deck size here 30

            string cardToFind = "Basic Attack";
            CardDTO cardData = allCards.FirstOrDefault(c => c.name == cardToFind);

            if (cardData != null)
            {
                Console.WriteLine($"Found card: {cardData.name}, Damage: {cardData.Damage}");

                for (int i = 0; i < 25; i++)
                {   
                    bool isTargetting = false;
                    if(cardData.Targeting == "Single")
                    {
                        isTargetting = true;
                    }

                    AttackCard attackCard = CardFactory.CreateAttackCard(cardData.name, cardData.Element, cardData.Description, cardData.Energy, cardData.Tier, cardData.Damage, isTargetting);
                    if (attackCard.effect == null)
                        Debug.LogError("Effect NOT assigned for: " + attackCard.title);
                    else
                        Debug.Log("Effect assigned successfully for: " + attackCard.title);
                        
                    clist.Add(attackCard);
                }

            }
            else
            {
                Console.WriteLine("Card not found!");
            }

            // for (int i = 0; i < 5; i++)
            // {
            //     int RandCardId = UnityEngine.Random.Range(1, allCards.Count); // not basic attack

            //     CardDTO RandCardData = allCards.FirstOrDefault(c => c.id == RandCardId);

            //     if (RandCardData != null)
            //     {
            //         Console.WriteLine($"Found card: {RandCardData.name}, Damage: {RandCardData.Damage}");
            //         bool isTargetting = false;
            //         if(RandCardData.Targeting == "Single")
            //         {
            //             isTargetting = true;
            //         }

            //         AttackCard RandAttackCard = CardFactory.CreateAttackCard(RandCardData.name, RandCardData.Element, RandCardData.Description, RandCardData.Energy, RandCardData.Tier, RandCardData.Damage, isTargetting);
                    
        

            //         if (RandAttackCard.effect == null)
            //             Debug.LogError("Effect NOT assigned for: " + RandAttackCard.title);
            //         else
            //             Debug.Log("Effect assigned successfully for: " + RandAttackCard.title);
            //         clist.Add(RandAttackCard);
            //     }
            //     else
            //     {
            //         Console.WriteLine("Card not found!");
            //     }

            // }


            PlayerAttrib.AddCards(clist);
        }

        static void GetDefaultDeck(Player PlayerAttrib)
        {
            // by default we can add n / TotalCardsinDeck cards per character (n is total chars, Total Cards in Deck is defined in Deck.cs)

            // right now im maknig an easier version just to make it work

            Deck DefaultDeck = new("Preset_1");

            // foreach (var ch in PlayerAttrib.PlayerCharactersList)
            // {
            for (int i = 0; i < DefaultDeck.CardsNumber; i++)
            {
                DefaultDeck.AddCard(PlayerAttrib.PlayerCardsList[i]);
            }
            //}
            PlayerAttrib.DecksList.Add(DefaultDeck);

        }
        static void GetLevel(Player PlayerAttrib)
        {
            PlayerAttrib.CalculateLevel();
        }

        // Update is called once per frame
        // void Update()
        // {

        // }



        [Serializable]
        public class JsonWrapper
        {
            public List<CardDTO> cards;
        }
    }
}