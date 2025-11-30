using System;
using UnityEngine;
using System.Collections.Generic;

namespace Imperius.Logic
{
    /// <summary>
    /// Base character class for all game characters (players, enemies, NPCs)
    /// </summary>
    [System.Serializable]
    public class Character
    {
        [Header("Basic Character Properties")]
        public string name;
        public Element element;
        public int level;

        [Header("Combat Stats")]
        //public int defense;
        //public int attack;
        public int hp;

        public Sprite Portrait;



        [Header("Special Abilities")]
        [TextArea(2, 4)]
        public string passive; // Description of passive ability

        /// <summary>
        /// Constructor for creating a new character
        /// </summary>
        public Character(string name, Element element, int level, string passive, int hp)
        {
            this.name = name;
            this.element = element;
            this.level = level;
            this.passive = passive;
            // this.defense = defense;
            // this.attack = attack;
            this.hp = hp;

            LoadPortrait();

            // this.maxHP = hp; // Set max HP to initial HP
        }

        private void LoadPortrait()
        {
            // Assuming your sprites are in Resources/Portraits/
            // And the file name is exactly {character.name}Portrait.png
            string path = "Portraits/" + name + "Portrait";

            Sprite loadedSprite = Resources.Load<Sprite>(path);

            if (loadedSprite != null)
            {
                Portrait = loadedSprite;
            }
            else
            {
                Debug.LogWarning("Portrait not found at path: " + path);
            }
        }
        public Character(Character other)
        {
            this.name = other.name;
            this.element = other.element;
            this.level = other.level;
            this.passive = other.passive;
            this.hp = other.hp;
            // this.maxHP = other.maxHP;

            // Uncomment and copy additional fields if needed
            // this.defense = other.defense;
            // this.attack = other.attack;
        }


        /// <summary>
        /// Default constructor for Unity serialization
        /// </summary>
        public Character()
        {
            // maxHP = hp;
        }

        /// <summary>
        /// Get the character's current health percentage
        /// </summary>
        // public float GetHealthPercentage()
        // {
        //     if (maxHP <= 0) return 0f;
        //     return (float)hp / maxHP;
        // }

        /// <summary>
        /// Check if the character is alive
        /// </summary>
        public bool IsAlive()
        {
            return hp > 0;
        }

        /// <summary>
        /// Check if the character is at full health
        /// </summary>
        // public bool IsAtFullHealth()
        // {
        //     return hp >= maxHP;
        // }

        /// <summary>
        /// Heal the character by a specified amount
        /// </summary>
        // public int Heal(int healAmount)
        // {
        //     if (!IsAlive()) return 0;

        //     int oldHP = hp;
        //     hp = Mathf.Min(hp + healAmount, maxHP);
        //     int actualHealing = hp - oldHP;

        //     Debug.Log($"{name} healed for {actualHealing} HP ({hp}/{maxHP})");
        //     return actualHealing;
        // }

        /// <summary>
        /// Deal damage to the character, accounting for defense
        /// </summary>
        // public int TakeDamage(int damage, Element attackerElement = Element.Fire)
        // {
        //     if (!IsAlive()) return 0;

        //     // Calculate element effectiveness
        //     float effectiveness = ElementUtility.GetElementEffectiveness(attackerElement, this.element);

        //     // Apply defense reduction (defense reduces damage by a percentage)
        //     // float defenseReduction = 1f - (defense * 0.01f); // Each defense point reduces damage by 1%
        //     // defenseReduction = Mathf.Max(defenseReduction, 0.1f); // Minimum 10% damage gets through

        //     // Calculate final damage
        //     int finalDamage = Mathf.RoundToInt(damage * effectiveness * defenseReduction);
        //     finalDamage = Mathf.Max(finalDamage, 1); // Minimum 1 damage

        //     int oldHP = hp;
        //     hp = Mathf.Max(hp - finalDamage, 0);
        //     int actualDamage = oldHP - hp;

        //     string effectivenessText = effectiveness > 1f ? " (Super Effective!)" :
        //                              effectiveness < 1f ? " (Not Very Effective)" : "";

        //     Debug.Log($"{name} took {actualDamage} damage{effectivenessText} ({hp}/{maxHP})");

        //     if (!IsAlive())
        //     {
        //         Debug.Log($"{name} has been defeated!");
        //     }

        //     return actualDamage;
        // }

        /// <summary>
        /// Calculate damage this character would deal with their attack stat
        /// </summary>
        // public int GetAttackDamage()
        // {
        //     // Base damage is attack stat with some randomness
        //     int baseDamage = attack;
        //     int randomVariation = Mathf.RoundToInt(baseDamage * 0.1f); // ±10% variation
        //     int finalDamage = baseDamage + UnityEngine.Random.Range(-randomVariation, randomVariation + 1);

        //     return Mathf.Max(finalDamage, 1); // Minimum 1 damage
        // }

        /// <summary>
        /// Level up the character, increasing stats
        /// </summary>
        // public void LevelUp()
        // {
        //     level++;

        //     // Increase stats based on level (customize these formulas as needed)
        //     int hpIncrease = Mathf.RoundToInt(maxHP * 0.1f) + 5; // 10% + 5 base HP
        //     int attackIncrease = Mathf.RoundToInt(attack * 0.08f) + 2; // 8% + 2 base attack
        //     int defenseIncrease = Mathf.RoundToInt(defense * 0.08f) + 1; // 8% + 1 base defense

        //     maxHP += hpIncrease;
        //     hp += hpIncrease; // Also heal when leveling up
        //     attack += attackIncrease;
        //     defense += defenseIncrease;

        //     Debug.Log($"{name} leveled up to {level}! Stats increased: HP +{hpIncrease}, Attack +{attackIncrease}, Defense +{defenseIncrease}");
        // }

        /// <summary>
        /// Get a detailed string representation of the character
        /// </summary>
        // public override string ToString()
        // {
        //     return $"{name} (Level {level} {element})\n" +
        //            $"HP: {hp}/{maxHP} | Attack: {attack} | Defense: {defense}\n" +
        //            $"Passive: {passive}";
        // }

        /// <summary>
        /// Get a short status string for UI display
        /// </summary>
        // public string GetStatusString()
        // {
        //     string status = IsAlive() ? "Alive" : "Defeated";
        //     return $"{name} - Lv.{level} | {hp}/{maxHP} HP | {status}";
        // }

        /// <summary>
        /// Reset character to full health (useful for battle preparation)
        /// </summary>
        // public void RestoreToFullHealth()
        // {
        //     hp = maxHP;
        //     Debug.Log($"{name} restored to full health ({hp}/{maxHP})");
        // }

        /// <summary>
        /// Apply a temporary stat boost (could be extended for buff system)
        /// </summary>
        // public void ApplyStatBoost(int attackBoost, int defenseBoost, int hpBoost)
        // {
        //     attack += attackBoost;
        //     defense += defenseBoost;
        //     maxHP += hpBoost;
        //     hp += hpBoost;

        //     Debug.Log($"{name} received stat boost: Attack +{attackBoost}, Defense +{defenseBoost}, HP +{hpBoost}");
        // }

    }

    public class BattleCharacter : Character
    {
        public int BattleHp;
        public int block;
        public List<StatusEffect> statusEffects;
        public int hurtTimes;

        public GameObject charUI;
        public GameObject healthBarUI;
        public BattleCharacter(Character charObj) : base(charObj)
        {
            this.BattleHp = charObj.hp;
            this.block = 0;
            this.hurtTimes = 0;
            statusEffects = new();
        }

        public void SetHp(Mode mode, int value)
        {
            if (mode == Mode.Remove)
            {
                int dmg = value;

                // block reduces dmg first
                if (block > 0)
                {
                    int blocked = Mathf.Min(block, dmg);
                    block -= blocked;
                    dmg -= blocked;
                }

                BattleHp -= dmg;
                BattleHp = Mathf.Clamp(BattleHp, 0, hp);

                hurtTimes++;

                Debug.Log($"{name} takes {value} damage -> Hp = {BattleHp}");

                IsDead();
            }
            else if (mode == Mode.Add)
            {
                BattleHp += value;
                BattleHp = Mathf.Clamp(BattleHp, 0, hp);

                Debug.Log($"{name} healed {value} -> Hp = {BattleHp}");
            }
        }

        // =============================
        //      BLOCK CONTROL
        // =============================
        public void SetBlock(Mode mode, int value)
        {
            if (mode == Mode.Add)
            {
                block += value;
                Debug.Log($"{name} gains {value} block (now {block})");
            }
            else if (mode == Mode.Remove)
            {
                block -= value;
                if (block < 0) block = 0;
                Debug.Log($"{name} loses {value} block (now {block})");
            }
        }

        // =============================
        //      STATUS EFFECT CONTROL
        // =============================
        public void SetStatusEffect(Mode mode, StatusEffect status)
        {
            StatusEffect existing =
                statusEffects.Find(x => x._effect == status._effect);

            if (mode == Mode.Add)
            {
                if (existing != null)
                {
                    existing._quantity += status._quantity;
                }
                else
                {
                    statusEffects.Add(new StatusEffect(status._effect, status._quantity));
                }

                Debug.Log($"{name} gains {status._effect} x{status._quantity}");
            }
            else if (mode == Mode.Remove)
            {
                if (existing != null)
                {
                    existing._quantity -= status._quantity;
                    if (existing._quantity <= 0)
                        statusEffects.Remove(existing);

                    Debug.Log($"{name} loses {status._effect} x{status._quantity}");
                }
            }
        }

        // =============================
        //      CHECKERS
        // =============================
        public void IsDead()
        {
            if (BattleHp <= 0)
            {
                Debug.Log($"{name} is DEAD.");
                // You can add death animation or removal here later
            }
        }

        public bool HasStatusEffect(StatusEffect se)
        {
            return statusEffects.Exists(x => x._effect == se._effect && x._quantity > 0);
        }
        public int GetHp()
        {
            return BattleHp;
        }

        public int GetBlock()
        {
            return block;
        }
    }

    public class StatusEffect
    {
        public enum EffectType
        {
            Weak,
            Vulnerable,
            Strength

        }

        public EffectType _effect;

        public int _quantity;

        public StatusEffect(EffectType effect, int quantity)
        {
            _effect = effect;
            _quantity = quantity;
        }


    }


    public enum Mode
    {
        Add,
        Remove
    }


    /// <summary>
    /// Utility class for creating characters
    /// </summary>
    // public static class CharacterFactory
    // {
    //     /// <summary>
    //     /// Create a basic character with specified stats
    //     /// </summary>
    //     public static Character CreateCharacter(string name, Element element, int level,
    //                                           string passive, int defense, int attack, int hp)
    //     {
    //         return new Character(name, element, level, passive, defense, attack, hp);
    //     }

    //     /// <summary>
    //     /// Create a character with stats appropriate for their level
    //     /// </summary>
    //     public static Character CreateLeveledCharacter(string name, Element element, int level, string passive)
    //     {
    //         // Base stats that scale with level
    //         int baseHP = 50 + (level * 10);
    //         int baseAttack = 10 + (level * 3);
    //         int baseDefense = 5 + (level * 2);

    //         return new Character(name, element, level, passive, baseDefense, baseAttack, baseHP);
    //     }

    //     /// <summary>
    //     /// Create a random character for testing purposes
    //     /// </summary>
    //     public static Character CreateRandomCharacter(int level = 1)
    //     {
    //         Element[] elements = ElementUtility.GetAllElements();
    //         Element randomElement = elements[UnityEngine.Random.Range(0, elements.Length)];

    //         string[] names = { "Aria", "Blaze", "Crystal", "Drake", "Echo", "Frost", "Gale", "Hunter" };
    //         string randomName = names[UnityEngine.Random.Range(0, names.Length)];

    //         string[] passives = {
    //             "Regenerates 5% HP each turn",
    //             "Critical hits deal double damage",
    //             "Immune to status effects",
    //             "Reflects 10% of damage taken",
    //             "Gains attack when HP is low"
    //         };
    //         string randomPassive = passives[UnityEngine.Random.Range(0, passives.Length)];

    //         return CreateLeveledCharacter(randomName, randomElement, level, randomPassive);
    //     }
    // }


}