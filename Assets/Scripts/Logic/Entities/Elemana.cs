using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Imperius.Logic;

namespace Imperius.Logic
{
    /// <summary>
    /// Represents elemana types including Universal
    /// </summary>
    public enum ElemanaType
    {
        Fire,
        Water,
        Ice,
        Metal,
        Lightning,
        Nature,
        Dark,
        Universal // Special elemana that can be used for any element
    }

    /// <summary>
    /// Utility to convert between Element and ElemanaType
    /// </summary>
    public static class ElemanaUtility
    {
        public static ElemanaType ElementToElemana(Element element)
        {
            return (ElemanaType)Enum.Parse(typeof(ElemanaType), element.ToString());
        }

        public static Element ElemanaToElement(ElemanaType elemana)
        {
            if (elemana == ElemanaType.Universal)
                return Element.Fire; // Default, though Universal doesn't map directly
            return (Element)Enum.Parse(typeof(Element), elemana.ToString());
        }

        public static Color GetElemanaColor(ElemanaType elemana)
        {
            if (elemana == ElemanaType.Universal)
                return Color.white;
            return ElementUtility.GetElementColor(ElemanaToElement(elemana));
        }
    }

    /// <summary>
    /// Manages elemana resources (like dice/mana)
    /// </summary>
    [System.Serializable]
    public class ElemanaPool
    {
        private Dictionary<ElemanaType, int> elemanaCount;
        private ElemanaType[] individualElemana; // Array to store each of the 9 elemana dice
        private int rerollsUsed;
        private const int maxRerollsPerRound = 2;
        private bool rerollPhaseActive;

        public ElemanaPool()
        {
            elemanaCount = new Dictionary<ElemanaType, int>();
            foreach (ElemanaType type in Enum.GetValues(typeof(ElemanaType)))
            {
                elemanaCount[type] = 0;
            }
            individualElemana = new ElemanaType[9];
            rerollsUsed = 0;
            rerollPhaseActive = false;
        }

        /// <summary>
        /// Roll 9 random elemanas and enter reroll phase
        /// </summary>
        public void RollElemanas()
        {
            // Reset all counts and rerolls for new round
            foreach (ElemanaType type in Enum.GetValues(typeof(ElemanaType)))
            {
                elemanaCount[type] = 0;
            }
            rerollsUsed = 0;
            rerollPhaseActive = true; // Enable reroll phase

            // Roll 9 individual elemanas with weighted distribution
            for (int i = 0; i < 9; i++)
            {
                ElemanaType rolledType = RollSingleElemana();
                individualElemana[i] = rolledType;
                elemanaCount[rolledType]++;
            }

            // Display the initial roll clearly
            Debug.Log("🎲 === ELEMANA ROLLED ===");
            Debug.Log(GetIndividualElemanaDisplay());
            Debug.Log($"📊 Summary: {GetDetailedElemanaString()}");
            Debug.Log("");
            Debug.Log("📋 Review your elemana above.");
            Debug.Log($"💫 You have {GetRerollsRemaining()} rerolls available.");
            Debug.Log("🔄 Type 'reroll <positions>' to reroll specific dice (e.g., 'reroll 1,3,5')");
            Debug.Log("🔄 Type 'reroll <types>' to reroll by type (e.g., 'reroll Fire,Water')");
            Debug.Log("✅ Type 'continue' to proceed with this roll");
            Debug.Log("ℹ️  Type 'elemana' to see your current roll again");
        }

        /// <summary>
        /// Roll a single elemana with weighted probabilities
        /// </summary>
        private ElemanaType RollSingleElemana()
        {
            // Universal elemana has lower chance (10%), others equal (90% / 7 = ~12.86% each)
            float roll = UnityEngine.Random.Range(0f, 100f);

            if (roll < 10f)
                return ElemanaType.Universal;

            // Distribute remaining 90% among the 7 regular elements
            ElemanaType[] regularTypes = {
                ElemanaType.Fire, ElemanaType.Water, ElemanaType.Ice,
                ElemanaType.Metal, ElemanaType.Lightning, ElemanaType.Nature, ElemanaType.Dark
            };

            int index = Mathf.FloorToInt((roll - 10f) / (90f / 7f));
            index = Mathf.Clamp(index, 0, regularTypes.Length - 1);

            return regularTypes[index];
        }

        /// <summary>
        /// Check if we can spend specific elemana amounts
        /// </summary>
        public bool CanSpend(ElemanaType type, int amount)
        {
            return elemanaCount[type] >= amount;
        }

        /// <summary>
        /// Check if we can spend elemana for a card (attack cards need matching or universal)
        /// </summary>
        public bool CanSpendForCard(Element cardElement, int cost, bool isAttackCard)
        {
            if (!isAttackCard)
            {
                // Skill cards can use any elemana
                return GetTotalElemana() >= cost;
            }

            // Attack cards need matching element or universal
            ElemanaType neededType = ElemanaUtility.ElementToElemana(cardElement);
            int available = elemanaCount[neededType] + elemanaCount[ElemanaType.Universal];
            return available >= cost;
        }

        /// <summary>
        /// Spend elemana for a card
        /// </summary>
        public bool SpendForCard(Element cardElement, int cost, bool isAttackCard)
        {
            if (!CanSpendForCard(cardElement, cost, isAttackCard))
                return false;

            if (!isAttackCard)
            {
                // Skill cards: spend any available elemana
                return SpendAnyElemana(cost);
            }

            // Attack cards: spend matching element first, then universal
            ElemanaType neededType = ElemanaUtility.ElementToElemana(cardElement);
            int remaining = cost;

            // First, spend matching element
            int matchingAvailable = elemanaCount[neededType];
            int matchingToSpend = Mathf.Min(remaining, matchingAvailable);
            elemanaCount[neededType] -= matchingToSpend;
            remaining -= matchingToSpend;

            // Then, spend universal if needed
            if (remaining > 0)
            {
                elemanaCount[ElemanaType.Universal] -= remaining;
            }

            return true;
        }

        /// <summary>
        /// Spend any available elemana (for skill cards, redrawing, etc.)
        /// </summary>
        public bool SpendAnyElemana(int amount)
        {
            if (GetTotalElemana() < amount)
                return false;

            int remaining = amount;
            foreach (ElemanaType type in Enum.GetValues(typeof(ElemanaType)))
            {
                if (remaining <= 0) break;

                int available = elemanaCount[type];
                int toSpend = Mathf.Min(remaining, available);
                elemanaCount[type] -= toSpend;
                remaining -= toSpend;
            }

            return remaining == 0;
        }

        /// <summary>
        /// Get total elemana count
        /// </summary>
        public int GetTotalElemana()
        {
            return elemanaCount.Values.Sum();
        }

        /// <summary>
        /// Get count of specific elemana type
        /// </summary>
        public int GetCount(ElemanaType type)
        {
            return elemanaCount[type];
        }

        /// <summary>
        /// Get a formatted string of all elemana counts
        /// </summary>
        public string GetElemanaString()
        {
            var nonZero = elemanaCount.Where(kvp => kvp.Value > 0)
                                    .Select(kvp => $"{kvp.Key}: {kvp.Value}")
                                    .ToArray();
            return string.Join(", ", nonZero);
        }

        /// <summary>
        /// Get detailed elemana breakdown
        /// </summary>
        public string GetDetailedElemanaString()
        {
            var result = new List<string>();
            foreach (var kvp in elemanaCount)
            {
                if (kvp.Value > 0)
                {
                    result.Add($"{kvp.Key}: {kvp.Value}");
                }
            }
            return $"Total: {GetTotalElemana()} [{string.Join(", ", result)}]";
        }

        /// <summary>
        /// Get individual elemana display showing each dice position
        /// </summary>
        public string GetIndividualElemanaDisplay()
        {
            var display = new System.Text.StringBuilder();
            display.AppendLine("Position:  1    2    3    4    5    6    7    8    9");
            display.Append("Elemana:  ");

            for (int i = 0; i < 9; i++)
            {
                string elemanaName = GetShortElemanaName(individualElemana[i]);
                display.Append($"{elemanaName,-4} ");
            }

            return display.ToString();
        }

        /// <summary>
        /// Get short name for elemana type for display
        /// </summary>
        private string GetShortElemanaName(ElemanaType type)
        {
            switch (type)
            {
                case ElemanaType.Fire: return "Fire";
                case ElemanaType.Water: return "Watr";
                case ElemanaType.Ice: return "Ice ";
                case ElemanaType.Metal: return "Metl";
                case ElemanaType.Lightning: return "Ligt";
                case ElemanaType.Nature: return "Natr";
                case ElemanaType.Dark: return "Dark";
                case ElemanaType.Universal: return "Univ";
                default: return "????";
            }
        }

        /// <summary>
        /// Get a clear, formatted display of the current elemana state
        /// </summary>
        public string GetFormattedElemanaDisplay()
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("🎲 === CURRENT ELEMANA ===");
            result.AppendLine(GetIndividualElemanaDisplay());
            result.AppendLine($"📊 Summary: {GetDetailedElemanaString()}");
            if (rerollPhaseActive)
            {
                result.AppendLine($"💫 Rerolls remaining: {GetRerollsRemaining()}/2");
            }
            return result.ToString();
        }

        public void AddElemana(ElemanaType et, int quantity)
        {
            if (!individualElemana.Contains(et))
            {
                individualElemana.Append(et);
            }
            elemanaCount[et]++;
        }

        /// <summary>
        /// Check if we have any elemana left
        /// </summary>
        public bool HasAnyElemana()
        {
            return GetTotalElemana() > 0;
        }

        /// <summary>
        /// Reroll specific elemana positions (only during reroll phase)
        /// </summary>
        public bool RerollElemanaPositions(List<int> positions)
        {
            if (!rerollPhaseActive)
            {
                Debug.Log("Rerolls are only available at the start of the round!");
                return false;
            }

            if (rerollsUsed >= maxRerollsPerRound)
            {
                Debug.Log($"Cannot reroll - already used {maxRerollsPerRound} rerolls this round!");
                return false;
            }

            if (positions == null || positions.Count == 0)
            {
                Debug.Log("No positions selected for reroll!");
                return false;
            }

            // Validate positions
            var validPositions = new List<int>();
            foreach (int pos in positions)
            {
                if (pos >= 1 && pos <= 9)
                {
                    validPositions.Add(pos - 1); // Convert to 0-based index
                }
                else
                {
                    Debug.Log($"Invalid position: {pos}. Positions must be 1-9.");
                }
            }

            if (validPositions.Count == 0)
            {
                Debug.Log("No valid positions to reroll!");
                return false;
            }

            // Store original roll for comparison
            string originalDisplay = GetIndividualElemanaDisplay();

            // Reroll selected positions
            foreach (int index in validPositions)
            {
                // Remove old elemana from count
                elemanaCount[individualElemana[index]]--;

                // Roll new elemana
                ElemanaType newType = RollSingleElemana();
                individualElemana[index] = newType;

                // Add new elemana to count
                elemanaCount[newType]++;
            }

            rerollsUsed++;

            Debug.Log("");
            Debug.Log($"🔄 Rerolled {validPositions.Count} dice at positions: {string.Join(", ", validPositions.Select(i => i + 1))}");
            Debug.Log("");
            Debug.Log("📊 BEFORE:");
            Debug.Log(originalDisplay);
            Debug.Log("");
            Debug.Log("📊 AFTER:");
            Debug.Log(GetIndividualElemanaDisplay());
            Debug.Log("");
            Debug.Log($"� New Summary: {GetDetailedElemanaString()}");
            Debug.Log("");
            Debug.Log($"💫 Rerolls remaining: {GetRerollsRemaining()}/2");

            if (GetRerollsRemaining() > 0)
            {
                Debug.Log("🔄 You can reroll more positions or type 'continue' to proceed");
            }
            else
            {
                Debug.Log("✅ No rerolls left - type 'continue' to start the round");
            }

            return true;
        }

        /// <summary>
        /// Reroll specific elemana types (legacy method, kept for compatibility)
        /// </summary>
        public bool RerollElemana(List<ElemanaType> typesToReroll)
        {
            if (!rerollPhaseActive)
            {
                Debug.Log("Rerolls are only available at the start of the round!");
                return false;
            }

            // Convert types to positions
            var positions = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (typesToReroll.Contains(individualElemana[i]))
                {
                    positions.Add(i + 1); // Convert to 1-based position
                }
            }

            if (positions.Count == 0)
            {
                Debug.Log("Selected elemana types have no dice to reroll!");
                return false;
            }

            return RerollElemanaPositions(positions);
        }

        /// <summary>
        /// Get number of rerolls remaining this round
        /// </summary>
        public int GetRerollsRemaining()
        {
            return maxRerollsPerRound - rerollsUsed;
        }

        /// <summary>
        /// Check if rerolls are available
        /// </summary>
        public bool CanReroll()
        {
            return rerollPhaseActive && rerollsUsed < maxRerollsPerRound;
        }

        /// <summary>
        /// Check if we're in the reroll phase
        /// </summary>
        public bool IsRerollPhaseActive()
        {
            return rerollPhaseActive;
        }

        /// <summary>
        /// End the reroll phase and start the main round
        /// </summary>
        public void EndRerollPhase()
        {
            rerollPhaseActive = false;
            Debug.Log("Reroll phase ended. Round started!");
            Debug.Log($"Final elemana: {GetElemanaString()}");
        }

        /// <summary>
        /// Reset all elemana counts to zero
        /// </summary>
        public void Clear()
        {
            foreach (ElemanaType type in Enum.GetValues(typeof(ElemanaType)))
            {
                elemanaCount[type] = 0;
            }
            rerollsUsed = 0;
            rerollPhaseActive = false;
        }
    }
}