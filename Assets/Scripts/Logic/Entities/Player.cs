using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Imperius.Logic;
using Imperius.Logic;
using Imperius.Data;

namespace Imperius.Logic
{
    /// <summary>
    /// Manages a player's characters, deck, and hand
    /// </summary>
    [System.Serializable]
    public class Player 
    {
        // [Header("Player Identity")]

        // [Header("Characters")]

        public string playerName;
        public List<Character> PlayerCharactersList;
        //public int activeCharacterIndex;

        // [Header("Cards")]
        public List<Card> PlayerCardsList;

        public List<Deck> DecksList;

        public int PlayerLevel; // this is a derived attribute calculated from player character levels

        // we can also make an xp holder if we want to make a rank system like bronze silver gold etc

        public Player()
        {
            PlayerCharactersList = new List<Character>();  // start empty
            PlayerCardsList = new List<Card>();       // start empty
            DecksList = new List<Deck>();            // start empty
        }

        public Player(string name)
        {
            playerName = name;
            PlayerCharactersList = new List<Character>();  // start empty
            PlayerCardsList = new List<Card>();       // start empty
            DecksList = new List<Deck>();            // start empty
        }
        public Player(Player p)
        {

            PlayerCharactersList = new List<Character>();
            foreach (var pc in p.PlayerCharactersList)
                PlayerCharactersList.Add(new Character(pc)); // assumes Character has a copy constructor

            PlayerCardsList = new List<Card>();
            foreach (var c in p.PlayerCardsList)
                if (c is AttackCard ac)
                    PlayerCardsList.Add(CardFactory.CreateAttackCard(ac));
                else if (c is SkillCard sc)
                    PlayerCardsList.Add(null); //make create skill card

            DecksList = new List<Deck>();
            foreach (var d in p.DecksList)
                DecksList.Add(new Deck(d)); // assumes Deck has a copy constructor
        }

        public Player (PlayerDTO p)
        {
            PlayerCharactersList = new List<Character>();
            foreach (var pc in p.PlayerCharactersList)
                PlayerCharactersList.Add(new Character(pc)); // assumes Character has a copy constructor

            PlayerCardsList = new List<Card>();
            foreach (var c in p.PlayerCardsList)
                if (c is AttackCard ac)
                    PlayerCardsList.Add(CardFactory.CreateAttackCard(ac));
                else if (c is SkillCard sc)
                    PlayerCardsList.Add(null); //make create skill card

            DecksList = new List<Deck>();
            foreach (var d in p.DecksList)
                DecksList.Add(new Deck(d)); // assumes Deck has a copy constructor

            PlayerLevel = p.PlayerLevel;
        }

        public void AddCharacters(List<Character> list)
        {
            PlayerCharactersList.AddRange(list);
        }

        public void AddCards(List<Card> list)
        {
            PlayerCardsList.AddRange(list);
        }

        public void CalculateLevel()
        {
            // we need to decide a good method to calculate level
            int count = 0;

            foreach (var c in PlayerCharactersList)
            {
                count += c.level;
            }
            this.PlayerLevel = count;


        }


        /// <summary>
        /// Get the currently active character
        /// </summary>
        // public Character GetActiveCharacter()
        // {
        //     if (characters.Count == 0 || activeCharacterIndex < 0 || activeCharacterIndex >= characters.Count)
        //         return null;
        //     return characters[activeCharacterIndex];
        // }

        /// <summary>
        /// Switch active character (costs 1 elemana)
        /// </summary>
        // public bool SwitchActiveCharacter(int newIndex)
        // {
        //     if (newIndex < 0 || newIndex >= characters.Count || newIndex == activeCharacterIndex)
        //         return false;

        //     // Cannot switch to a dead character
        //     if (!characters[newIndex].IsAlive())
        //     {
        //         Debug.Log($"Cannot switch to {characters[newIndex].name} - character is defeated!");
        //         return false;
        //     }

        //     if (!elemanaPool.SpendAnyElemana(1))
        //     {
        //         Debug.Log("Not enough elemana to switch characters!");
        //         return false;
        //     }

        //     int oldIndex = activeCharacterIndex;
        //     activeCharacterIndex = newIndex;
        //     Debug.Log($"Switched active character from {characters[oldIndex].name} to {characters[newIndex].name}");
        //     return true;
        // }

        /// <summary>
        /// Initialize player with default characters and deck
        /// </summary>
        // public void InitializeDefaultSetup()
        // {
        //     CreateDefaultCharacters();
        //     CreateDefaultDeck();
        //     ShuffleDeck();
        // }

        /// <summary>
        /// Create 3 default characters with different elements
        /// </summary>
        // private void CreateDefaultCharacters()
        // {
        //     characters.Clear();

        //     // Create three diverse characters
        //     characters.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Blaze Warrior", Element.Fire, 5, "Critical hits burn enemies for 2 turns"));

        //     characters.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Frost Guardian", Element.Ice, 5, "Freezes attackers, reducing their speed"));

        //     characters.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Storm Sage", Element.Lightning, 5, "Chain lightning hits additional enemies"));

        //     activeCharacterIndex = 0;
        //     Debug.Log($"Created {characters.Count} characters for {playerName}");
        // }

        /// <summary>
        /// Create a deck of 24 cards (16 attack, 8 skill)
        /// </summary>
        // private void CreateDefaultDeck()
        // {
        //     deck.Clear();

        //     // Create 16 attack cards with variety
        //     Element[] elements = ElementUtility.GetAllElements();

        //     for (int i = 0; i < 16; i++)
        //     {
        //         Element cardElement = elements[i % elements.Length];
        //         string title = $"{cardElement} Strike {i + 1}";
        //         string description = $"A {cardElement.ToString().ToLower()} elemental attack";

        //         int cost = UnityEngine.Random.Range(1, 4); // 1-3 cost
        //         int tier = UnityEngine.Random.Range(1, 4); // 1-3 tier
        //         int enemies = UnityEngine.Random.Range(1, 3); // 1-2 enemies
        //         int damage = cost * 8 + tier * 5; // Scale damage with cost and tier

        //         var attackCard = CardFactory.CreateAttackCard(title, cardElement, description, 
        //                                                     cost, tier, enemies, damage);
        //         deck.Add(attackCard);
        //     }

        //     // Create 8 skill cards
        //     SkillSpecialization[] specializations = { 
        //         SkillSpecialization.Heal, SkillSpecialization.Buff, 
        //         SkillSpecialization.Debuff, SkillSpecialization.Shield 
        //     };

        //     for (int i = 0; i < 8; i++)
        //     {
        //         Element cardElement = elements[i % elements.Length];
        //         SkillSpecialization spec = specializations[i % specializations.Length];

        //         string title = $"{cardElement} {spec} {i + 1}";
        //         string description = $"A {cardElement.ToString().ToLower()} {spec.ToString().ToLower()} ability";

        //         int cost = UnityEngine.Random.Range(1, 4);
        //         int tier = UnityEngine.Random.Range(1, 4);
        //         int characters = spec == SkillSpecialization.Debuff ? 
        //                         UnityEngine.Random.Range(1, 3) : UnityEngine.Random.Range(1, 4);
        //         int turns = UnityEngine.Random.Range(2, 5);
        //         int magnitude = cost * 6 + tier * 4;

        //         var skillCard = CardFactory.CreateSkillCard(title, cardElement, description,
        //                                                   cost, tier, spec, magnitude, characters, turns);
        //         deck.Add(skillCard);
        //     }

        //     Debug.Log($"Created deck with {deck.Count} cards (Attack: {deck.Count(c => c.type == CardType.Attack)}, Skill: {deck.Count(c => c.type == CardType.Skill)})");
        // }

        /// <summary>
        /// Shuffle the deck
        /// </summary>
        // public void ShuffleDeck()
        // {
        //     for (int i = 0; i < deck.Count; i++)
        //     {
        //         Card temp = deck[i];
        //         int randomIndex = UnityEngine.Random.Range(i, deck.Count);
        //         deck[i] = deck[randomIndex];
        //         deck[randomIndex] = temp;
        //     }
        // }

        /// <summary>
        /// Draw cards for a new round (4 attack, 2 skill)
        /// </summary>
        // public void DrawHand()
        // {
        //     hand.Clear();

        //     // Check if deck is empty
        //     if (deck.Count == 0)
        //     {
        //         Debug.Log($"{playerName} cannot draw cards - deck is empty!");
        //         return;
        //     }

        //     // Separate attack and skill cards in deck
        //     var attackCards = deck.Where(c => c.type == CardType.Attack).ToList();
        //     var skillCards = deck.Where(c => c.type == CardType.Skill).ToList();

        //     // Draw up to 4 attack cards (or as many as available)
        //     int attackCardsToDraw = Mathf.Min(4, attackCards.Count);
        //     for (int i = 0; i < attackCardsToDraw; i++)
        //     {
        //         int randomIndex = UnityEngine.Random.Range(0, attackCards.Count);
        //         hand.Add(attackCards[randomIndex]);
        //         deck.Remove(attackCards[randomIndex]);
        //         attackCards.RemoveAt(randomIndex);
        //     }

        //     // Draw up to 2 skill cards (or as many as available)
        //     int skillCardsToDraw = Mathf.Min(2, skillCards.Count);
        //     for (int i = 0; i < skillCardsToDraw; i++)
        //     {
        //         int randomIndex = UnityEngine.Random.Range(0, skillCards.Count);
        //         hand.Add(skillCards[randomIndex]);
        //         deck.Remove(skillCards[randomIndex]);
        //         skillCards.RemoveAt(randomIndex);
        //     }

        //     string deckStatus = deck.Count == 0 ? " (DECK EMPTY)" : $" ({deck.Count} cards remaining in deck)";
        //     Debug.Log($"Drew {hand.Count} cards for {playerName}{deckStatus}");
        // }

        /// <summary>
        /// Redraw hand by paying 1 elemana
        /// </summary>
        // public bool RedrawHand()
        // {
        //     if (!elemanaPool.SpendAnyElemana(1))
        //     {
        //         Debug.Log("Not enough elemana to redraw hand!");
        //         return false;
        //     }

        //     // Return current hand to deck
        //     foreach (var card in hand)
        //     {
        //         deck.Add(card);
        //     }

        //     ShuffleDeck();
        //     DrawHand();

        //     Debug.Log($"{playerName} redrawed their hand");
        //     return true;
        // }

        /// <summary>
        /// Play a card from hand
        /// </summary>
        // public bool PlayCard(int handIndex, List<Character> targets = null)
        // {
        //     if (handIndex < 0 || handIndex >= hand.Count)
        //     {
        //         Debug.Log("Invalid card index!");
        //         return false;
        //     }

        //     Card card = hand[handIndex];
        //     bool isAttack = card.type == CardType.Attack;

        //     // Check if we can afford the card
        //     if (!elemanaPool.CanSpendForCard(card.element, card.cost, isAttack))
        //     {
        //         Debug.Log($"Not enough elemana to play {card.title}!");
        //         return false;
        //     }

        //     // Spend the elemana
        //     if (!elemanaPool.SpendForCard(card.element, card.cost, isAttack))
        //     {
        //         Debug.Log($"Failed to spend elemana for {card.title}!");
        //         return false;
        //     }

        //     // Execute the card
        //     ExecuteCard(card, targets);

        //     // Move card to discard pile
        //     hand.RemoveAt(handIndex);
        //     discardPile.Add(card);

        //     Debug.Log($"{playerName} played {card.title}");
        //     return true;
        // }

        /// <summary>
        /// Execute a card's effects
        /// </summary>
        // private void ExecuteCard(Card card, List<Character> targets)
        // {
        //     Character activeChar = GetActiveCharacter();

        //     if (card is AttackCard attackCard)
        //     {
        //         // Attack cards deal damage through the active character
        //         int baseDamage = activeChar != null ? activeChar.GetAttackDamage() : 10;
        //         int cardDamage = attackCard.magnitude;
        //         int totalDamage = baseDamage + cardDamage;

        //         Debug.Log($"{activeChar?.name ?? "Unknown"} attacks with {card.title} for {totalDamage} total damage ({baseDamage} character + {cardDamage} card)");

        //         // Apply damage to targets (this would be handled by the battle system)
        //         if (targets != null)
        //         {
        //             int enemiesHit = Mathf.Min(attackCard.numberOfEnemies, targets.Count);
        //             for (int i = 0; i < enemiesHit; i++)
        //             {
        //                 targets[i].TakeDamage(totalDamage, card.element);
        //             }
        //         }
        //     }
        //     else if (card is SkillCard skillCard)
        //     {
        //         Debug.Log($"Using skill {card.title}: {skillCard.specialization}");

        //         // Apply skill effects to targets
        //         if (targets != null)
        //         {
        //             int charactersAffected = Mathf.Min(skillCard.characters, targets.Count);
        //             for (int i = 0; i < charactersAffected; i++)
        //             {
        //                 ApplySkillEffect(skillCard, targets[i]);
        //             }
        //         }
        //     }
        // }

        /// <summary>
        /// Apply skill card effects to a character
        /// </summary>
        // private void ApplySkillEffect(SkillCard skill, Character target)
        // {
        //     switch (skill.specialization)
        //     {
        //         case SkillSpecialization.Heal:
        //             target.Heal(skill.magnitude);
        //             break;
        //         case SkillSpecialization.Buff:
        //             target.ApplyStatBoost(skill.magnitude, 0, 0);
        //             break;
        //         case SkillSpecialization.Debuff:
        //             // This would typically reduce enemy stats (implement as needed)
        //             Debug.Log($"{target.name} is debuffed by {skill.magnitude} for {skill.turns} turns");
        //             break;
        //         case SkillSpecialization.Shield:
        //             // This would typically add shield points (implement as needed)
        //             Debug.Log($"{target.name} gains {skill.magnitude} shield for {skill.turns} turns");
        //             break;
        //     }
        // }

        /// <summary>
        /// Start a new round (elemana rolling now handled by BattleManager)
        /// </summary>
        // public void StartNewRound()
        // {
        //     // Elemana rolling and hand drawing now handled by BattleManager
        //     // to support the reroll phase workflow
        // }

        /// <summary>
        /// Check if all characters are defeated
        /// </summary>
        // public bool IsDefeated()
        // {
        //     return characters.All(c => !c.IsAlive());
        // }

        /// <summary>
        /// Get cumulative HP of all alive characters (for deck empty win condition)
        /// </summary>
        // public int GetCumulativeHP()
        // {
        //     return characters.Where(c => c.IsAlive()).Sum(c => c.hp);
        // }

        /// <summary>
        /// Check if deck is empty (no more cards to draw)
        /// </summary>
        // public bool IsDeckEmpty()
        // {
        //     return deck.Count == 0;
        // }

        /// <summary>
        /// Get player status string
        /// </summary>
        // public string GetStatusString()
        // {
        //     var status = $"=== {playerName} ===\n";
        //     status += $"Active: {GetActiveCharacter()?.name ?? "None"}\n";
        //     status += $"Characters:\n";

        //     for (int i = 0; i < characters.Count; i++)
        //     {
        //         string activeMarker = i == activeCharacterIndex ? "*" : " ";
        //         status += $"{activeMarker} {i + 1}. {characters[i].GetStatusString()}\n";
        //     }

        //     status += $"Hand: {hand.Count} cards\n";
        //     status += $"Elemana: {elemanaPool.GetDetailedElemanaString()}\n";

        //     return status;
        // }

    }

        public class BattlePlayer : Player
    {


        public BattlePlayer(Player p) : base(p)
        {
            foreach (var ch in p.PlayerCharactersList)
            {
                PlayerBattleChars.Add(new BattleCharacter(ch));
            }
        }

        public List<BattleCharacter> PlayerBattleChars = new();

        public CardPile Pile = new();

    }



}