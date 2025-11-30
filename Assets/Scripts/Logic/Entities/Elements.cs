using System;
using UnityEngine;

namespace Imperius.Logic
{
    /// <summary>
    /// Enumeration for all elements in the game
    /// Used by cards, characters, and other game systems
    /// </summary>
    public enum Element
    {
        Fire,
        Water,
        Ice,
        Metal,
        Lightning,
        Nature,
        Dark
    }

    /// <summary>
    /// Utility class for element-related operations
    /// </summary>
    public static class ElementUtility
    {
        /// <summary>
        /// Get the color associated with an element for UI purposes
        /// </summary>
        public static Color GetElementColor(Element element)
        {
            switch (element)
            {
                case Element.Fire:
                    return new Color(1f, 0.3f, 0.1f); // Red-Orange
                case Element.Water:
                    return new Color(0.1f, 0.5f, 1f); // Blue
                case Element.Ice:
                    return new Color(0.7f, 0.9f, 1f); // Light Blue
                case Element.Metal:
                    return new Color(0.6f, 0.6f, 0.7f); // Gray
                case Element.Lightning:
                    return new Color(1f, 1f, 0.2f); // Yellow
                case Element.Nature:
                    return new Color(0.2f, 0.8f, 0.2f); // Green
                case Element.Dark:
                    return new Color(0.2f, 0.1f, 0.3f); // Dark Purple
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// Get element effectiveness multiplier against another element
        /// Returns damage multiplier (1.0 = normal, 1.5 = effective, 0.5 = weak)
        /// </summary>
        public static float GetElementEffectiveness(Element attackerElement, Element defenderElement)
        {
            // Define element effectiveness relationships
            switch (attackerElement)
            {
                case Element.Fire:
                    if (defenderElement == Element.Ice || defenderElement == Element.Nature)
                        return 1.5f; // Fire is strong against Ice and Nature
                    if (defenderElement == Element.Water)
                        return 0.5f; // Fire is weak against Water
                    break;

                case Element.Water:
                    if (defenderElement == Element.Fire || defenderElement == Element.Metal)
                        return 1.5f; // Water is strong against Fire and Metal
                    if (defenderElement == Element.Lightning || defenderElement == Element.Nature)
                        return 0.5f; // Water is weak against Lightning and Nature
                    break;

                case Element.Ice:
                    if (defenderElement == Element.Water || defenderElement == Element.Nature)
                        return 1.5f; // Ice is strong against Water and Nature
                    if (defenderElement == Element.Fire || defenderElement == Element.Metal)
                        return 0.5f; // Ice is weak against Fire and Metal
                    break;

                case Element.Metal:
                    if (defenderElement == Element.Lightning || defenderElement == Element.Ice)
                        return 1.5f; // Metal is strong against Lightning and Ice
                    if (defenderElement == Element.Fire || defenderElement == Element.Water)
                        return 0.5f; // Metal is weak against Fire and Water
                    break;

                case Element.Lightning:
                    if (defenderElement == Element.Water || defenderElement == Element.Metal)
                        return 1.5f; // Lightning is strong against Water and Metal
                    if (defenderElement == Element.Nature || defenderElement == Element.Dark)
                        return 0.5f; // Lightning is weak against Nature and Dark
                    break;

                case Element.Nature:
                    if (defenderElement == Element.Water || defenderElement == Element.Lightning)
                        return 1.5f; // Nature is strong against Water and Lightning
                    if (defenderElement == Element.Fire || defenderElement == Element.Ice)
                        return 0.5f; // Nature is weak against Fire and Ice
                    break;

                case Element.Dark:
                    if (defenderElement == Element.Lightning)
                        return 1.5f; // Dark is strong against Lightning
                    if (defenderElement == Element.Dark)
                        return 0.5f; // Dark is weak against itself
                    break;
            }

            return 1.0f; // Normal effectiveness
        }

        /// <summary>
        /// Get a description of the element
        /// </summary>
        public static string GetElementDescription(Element element)
        {
            switch (element)
            {
                case Element.Fire:
                    return "The element of passion and destruction. Burns bright and consumes all.";
                case Element.Water:
                    return "The element of flow and adaptation. Gentle yet persistent.";
                case Element.Ice:
                    return "The element of preservation and stillness. Cold and unyielding.";
                case Element.Metal:
                    return "The element of strength and endurance. Hard and unbreakable.";
                case Element.Lightning:
                    return "The element of speed and power. Swift and electrifying.";
                case Element.Nature:
                    return "The element of growth and harmony. Living and ever-changing.";
                case Element.Dark:
                    return "The element of mystery and void. Hidden and unknowable.";
                default:
                    return "An unknown element.";
            }
        }

        /// <summary>
        /// Get all available elements as an array
        /// </summary>
        public static Element[] GetAllElements()
        {
            return (Element[])Enum.GetValues(typeof(Element));
        }

        /// <summary>
        /// Check if an element is considered "offensive" (typically used for attack abilities)
        /// </summary>
        public static bool IsOffensiveElement(Element element)
        {
            return element == Element.Fire || element == Element.Lightning || element == Element.Dark;
        }

        /// <summary>
        /// Check if an element is considered "defensive" (typically used for protective abilities)
        /// </summary>
        public static bool IsDefensiveElement(Element element)
        {
            return element == Element.Water || element == Element.Ice || element == Element.Metal;
        }

        /// <summary>
        /// Check if an element is considered "supportive" (typically used for healing/buff abilities)
        /// </summary>
        public static bool IsSupportiveElement(Element element)
        {
            return element == Element.Nature;
        }
    }
}