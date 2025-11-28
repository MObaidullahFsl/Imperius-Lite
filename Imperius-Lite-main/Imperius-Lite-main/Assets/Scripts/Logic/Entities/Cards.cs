using System;
using System.Collections.Generic;
using UnityEngine;
using Imperius.Logic;
using System.Linq;
using System.IO;
using Imperius.Data;

namespace Imperius.Logic
{

    /// <summary>
    /// Enumeration for card types
    /// </summary>
    public enum CardType
    {
        Attack,
        Skill
    }

    /// <summary>
    /// Enumeration for skill specializations
    /// </summary>
    public enum SkillSpecialization
    {
        Heal,
        Buff,
        Debuff,
        Shield
    }

    /// <summary>
    /// Base abstract class for all cards
    /// </summary>
    [System.Serializable]
    public abstract class Card
    {
        [Header("Basic Card Properties")]
        public string title;
        public Element element;
        [TextArea(3, 5)]
        public string description;
        public CardType type;
        public int cost;
        public int tier;

        public CardEffect effect;

        /// <summary>
        /// Constructor for base card properties
        /// </summary>
        public Card(string title, Element element, string description, CardType type, int cost, int tier)
        {
            this.title = title;
            this.element = element;
            this.description = description;
            this.type = type;
            this.cost = cost;
            this.tier = tier;
            this.AssignEffect();
        }

        public Card(Card c)
        {
            this.title = c.title;
            this.element = c.element;
            this.description = c.description;
            this.type = c.type;
            this.cost = c.cost;
            this.tier = c.tier;
            this.effect = c.effect;
        }
        /// <summary>
        /// Default constructor for Unity serialization
        /// </summary>
        public Card() { }

        /// <summary>
        /// Abstract method to be implemented by derived classes for card effects
        /// </summary>
        public abstract void PlayCard();

        /// <summary>
        /// Get a formatted string representation of the card
        /// </summary>
        public override string ToString()
        {
            return $"{title} ({element}) - Tier {tier} - Cost: {cost}";
        }

        public void AssignEffect()
        {
            if (CardRegistry.Effects.TryGetValue(title, out var effect))
                this.effect = effect;
            else
                Debug.LogWarning($"No registered effect found for card: {title}");
        }

        public void SetCost(Mode mode, int quantity)
        {
            if (mode == Mode.Add)
            {
                this.cost += quantity;
            }
            else
            {
                this.cost -= quantity;
            }
        }
    }

    /// <summary>
    /// Attack type card class
    /// </summary>
    [System.Serializable]
    public class AttackCard : Card
    {
        // [Header("Attack Properties")]
        // public int numberOfEnemies;
        public int damage; // Damage

        /// <summary>
        /// Constructor for Attack Card
        /// </summary>
        public AttackCard(string title, Element element, string description, int cost, int tier,
                           int damage)
            : base(title, element, description, CardType.Attack, cost, tier)
        {
            // this.numberOfEnemies = numberOfEnemies;
            this.damage = damage;
        }

        /// <summary>
        /// Default constructor for Unity serialization
        /// </summary>
        public AttackCard() : base()
        {
            type = CardType.Attack;
        }

        // action is fetched from the Card Dictionary

        public CardEffect Action { get; private set; }

        public void SetAction(CardEffect action)
        {
            this.Action = action;
        }


        /// <summary>
        /// Execute the attack card effect
        /// </summary>
        public override void PlayCard()
        {
            Debug.Log($"Playing Attack Card: {title}");
            Debug.Log($"Dealing {damage} damage to enemies");
            // Implement actual attack logic here

        }

        /// <summary>
        /// Get detailed string representation of attack card
        /// </summary>
        // public override string ToString()
        // {
        //     return base.ToString() + $" - Attack: {magnitude} damage to {numberOfEnemies} enemies";
        // }
    }

    /// <summary>
    /// Skill type card class
    /// </summary>
    [System.Serializable]
    public class SkillCard : Card
    {
        [Header("Skill Properties")]
        public SkillSpecialization specialization;
        public int magnitude;
        public int characters; // Number of characters affected
        public int turns; // Duration in turns

        /// <summary>
        /// Constructor for Skill Card
        /// </summary>
        public SkillCard(string title, Element element, string description, int cost, int tier,
                        SkillSpecialization specialization, int magnitude, int characters, int turns)
            : base(title, element, description, CardType.Skill, cost, tier)
        {
            this.specialization = specialization;
            this.magnitude = magnitude;
            this.characters = characters;
            this.turns = turns;
        }

        /// <summary>
        /// Default constructor for Unity serialization
        /// </summary>
        public SkillCard() : base()
        {
            type = CardType.Skill;
        }

        /// <summary>
        /// Execute the skill card effect
        /// </summary>
        public override void PlayCard()
        {
            Debug.Log($"Executing Skill Card: {title}");
            Debug.Log($"Applying {specialization} with magnitude {magnitude} to {characters} characters for {turns} turns");

            // Implement specific skill logic based on specialization
            switch (specialization)
            {
                case SkillSpecialization.Heal:
                    Debug.Log($"Healing {characters} characters for {magnitude} HP over {turns} turns");
                    break;
                case SkillSpecialization.Buff:
                    Debug.Log($"Buffing {characters} characters by {magnitude} for {turns} turns");
                    break;
                case SkillSpecialization.Debuff:
                    Debug.Log($"Debuffing {characters} characters by {magnitude} for {turns} turns");
                    break;
                case SkillSpecialization.Shield:
                    Debug.Log($"Shielding {characters} characters with {magnitude} shield points for {turns} turns");
                    break;
            }
        }

        /// <summary>
        /// Get detailed string representation of skill card
        /// </summary>
        public override string ToString()
        {
            return base.ToString() + $" - {specialization}: {magnitude} to {characters} characters for {turns} turns";
        }
    }

    /// <summary>
    /// Utility class for creating cards
    /// </summary>
    public static class CardFactory
    {
        /// <summary>
        /// Create an attack card
        /// </summary>
        public static AttackCard CreateAttackCard(
               string title, Element element, string description, int cost, int tier, int damage,
               CardEffect action = null)
        {
            var card = new AttackCard(title, element, description, cost, tier, damage);
            if (action != null)
                card.SetAction(action);
            return card;
        }

        public static AttackCard CreateAttackCard(AttackCard original)
        {
            if (original == null)
                return null;

            // Create a new card with the same primitive and value-type fields
            var copy = new AttackCard(
                original.title,
                original.element,
                original.description,
                original.cost,
                original.tier,
                original.damage
            );

            // Deep-copy or reassign the effect (depending on what CardEffect is)
            if (original.Action != null)
                copy.SetAction(original.Action); // if Clone() exists
                                                 // or: copy.SetAction(original.GetAction());   // if reuse is okay

            return copy;
        }

        public static AttackCard CreateAttackCard(string cardName)
        {
            if (string.IsNullOrEmpty(cardName))
                return null;

            string jsonString = File.ReadAllText("cards.json");
            List<CardDTO> allCards = JsonUtility.FromJson<List<CardDTO>>(jsonString);

            // Search for the card data in the JSON list
            CardDTO cardData = allCards.FirstOrDefault(c => c.name == cardName);

            if (cardData == null)
            {
                Console.WriteLine($"Card '{cardName}' not found in JSON data!");
                return null;
            }

            // Create and return the AttackCard
            var attackCard = CreateAttackCard(cardData.name,
                cardData.Element,
                cardData.Description,
                cardData.Energy,
                cardData.Tier,
                cardData.Damage);

            return attackCard;
        }


        /// <summary>
        /// Create a skill card
        /// </summary>
        public static SkillCard CreateSkillCard(string title, Element element, string description,
                                               int cost, int tier, SkillSpecialization specialization,
                                               int magnitude, int characters, int turns)
        {
            return new SkillCard(title, element, description, cost, tier, specialization, magnitude, characters, turns);
        }
    }


    [Serializable]
    public class CardPile
    {
        public enum PileType { Hand, Draw, Discard }

        public List<Card> Hand = new();
        public List<Card> Draw = new();
        public List<Card> Discard = new();

        // Get pile by type
        private List<Card> GetPile(PileType pile)
        {
            return pile switch
            {
                PileType.Hand => Hand,
                PileType.Draw => Draw,
                PileType.Discard => Discard,
                _ => null
            };
        }

        // Add a card to a pile
        public void AddCard(PileType pile, Card card)
        {
            var targetPile = GetPile(pile);
            if (targetPile == null || card == null) return;

            targetPile.Add(card);
            Debug.Log($"{card.title} added to {pile} pile.");
        }

        // Remove a card from a pile
        public void RemoveCard(PileType pile, Card card)
        {
            var targetPile = GetPile(pile);
            if (targetPile == null || card == null) return;

            if (targetPile.Remove(card))
                Debug.Log($"{card.title} removed from {pile} pile.");
            else
                Debug.LogWarning($"{card.title} not found in {pile} pile!");
        }

        // Draw a random card from Draw pile to Hand
        public void DrawCard()
        {
            if (Draw.Count == 0)
            {
                Debug.Log("Draw pile is empty!");
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, Draw.Count);
            var card = Draw[randomIndex];
            Draw.RemoveAt(randomIndex);
            Hand.Add(card);

            Debug.Log($"{card.title} drawn to hand.");
        }

        // Discard a card from Hand to Discard pile
        public void DiscardCard(Card card)
        {
            RemoveCard(PileType.Hand, card);
            AddCard(PileType.Discard, card);
        }

        // Flush a pile into another pile
        public void FlushPile(PileType pile)
        {
            var sourcePile = GetPile(pile);
            if (sourcePile == null || sourcePile.Count == 0) return;

            List<Card> targetPile = pile switch
            {
                PileType.Discard => Draw,
                PileType.Hand => Discard,
                PileType.Draw => Hand,
                _ => null
            };

            if (targetPile != null)
            {
                targetPile.AddRange(sourcePile);
                sourcePile.Clear();
                Debug.Log($"{pile} pile flushed into {targetPile}");
            }
        }

        // Create hand by drawing multiple cards
        public void CreateHand(int handSize)
        {
            for (int i = 0; i < handSize; i++)
                DrawCard();
        }

        // Print pile content for debugging
        public void PrintPile(PileType pile)
        {
            var targetPile = GetPile(pile);
            string pileContent = targetPile.Count > 0 ? string.Join(", ", targetPile) : "Empty";
            Debug.Log($"{pile} pile: {pileContent}");
        }
    }
}
