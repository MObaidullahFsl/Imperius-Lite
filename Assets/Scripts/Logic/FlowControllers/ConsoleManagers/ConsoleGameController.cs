// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using Imperius.Logic;
// using Imperius.Logic;


// namespace Imperius.Logic
// {
//     /// <summary>
//     /// Handles console input/output for the battle game
//     /// Attach this to a GameObject in the scene
//     /// </summary>
//     public class ConsoleGameController : MonoBehaviour
//     {
//         [Header("Game Components")]
//         public BattleManager battleManager;

//         [Header("Console Settings")]
//         [TextArea(5, 10)]
//         public string consoleOutput = "Welcome to Imperius Console Battle!\nType 'help' for commands or 'start' to begin.";

//         [Header("Input")]
//         public string currentInput = "";

//         private void Start()
//         {
//             // Find or create battle manager
//             if (battleManager == null)
//             {
//                 battleManager = FindObjectOfType<BattleManager>();
//                 if (battleManager == null)
//                 {
//                     GameObject bmGO = new GameObject("BattleManager");
//                     battleManager = bmGO.AddComponent<BattleManager>();
//                 }
//             }

//             LogToConsole("=== IMPERIUS CONSOLE BATTLE ===");
//             LogToConsole("Type commands in the 'Current Input' field in the inspector and press Enter in Play mode.");
//             LogToConsole("Type 'help' to see available commands.");
//             LogToConsole("Type 'start' to begin a new battle!");
//         }

//         private void Update()
//         {
//             // Input handling removed to avoid Input System conflicts
//             // Use the Inspector button "Execute Input Command" or call ExecuteInputCommand() from other scripts
//         }

//         /// <summary>
//         /// Process a console command
//         /// </summary>
//         public void ProcessCommand(string command)
//         {
//             if (string.IsNullOrEmpty(command)) return;

//             LogToConsole($"> {command}");

//             string[] parts = command.ToLower().Split(' ');
//             string cmd = parts[0];

//             try
//             {
//                 switch (cmd)
//                 {
//                     case "help":
//                         ShowHelp();
//                         break;

//                     case "start":
//                         StartNewBattle();
//                         break;

//                     case "status":
//                         ShowStatus();
//                         break;

//                     case "hand":
//                         ShowHand();
//                         break;

//                     case "elemana":
//                     case "show":
//                         ShowElemana();
//                         break;

//                     case "play":
//                         PlayCard(parts);
//                         break;

//                     case "switch":
//                         SwitchCharacter(parts);
//                         break;

//                     case "redraw":
//                         RedrawHand();
//                         break;

//                     case "endturn":
//                         EndTurn();
//                         break;

//                     case "quit":
//                         QuitBattle();
//                         break;

//                     case "enemies":
//                         ShowEnemies();
//                         break;

//                     case "characters":
//                         ShowCharacters();
//                         break;

//                     case "deck":
//                         ShowDeckStatus();
//                         break;

//                     case "startpvp":
//                         StartPvPBattle();
//                         break;

//                     case "reroll":
//                         RerollElemana(parts);
//                         break;

//                     case "continue":
//                         ContinueRound();
//                         break;

//                     case "ready":
//                         StartRoundSequence();
//                         break;

//                     default:
//                         LogToConsole($"Unknown command: {cmd}. Type 'help' for available commands.");
//                         break;
//                 }
//             }
//             catch (Exception e)
//             {
//                 LogToConsole($"Error executing command: {e.Message}");
//             }
//         }

//         private void ShowHelp()
//         {
//             LogToConsole(battleManager.GetCommandsHelp());
//         }

//         private void StartNewBattle()
//         {
//             battleManager.InitializeBattle();
//             battleManager.PrepareNewRound(); // Use new method for round preparation

//             // Follow the new battle flow sequence
//             LogToConsole("🎮 === BATTLE INITIALIZED ===\n");

//             // Step 1: Display Enemy Characters
//             ShowEnemyCharacters();

//             // Step 2: Display User Characters
//             ShowUserCharacters();

//             // Step 3: Allow Active Character Selection
//             LogToConsole("\n🎯 === ACTIVE CHARACTER SELECTION ===");
//             LogToConsole("Current active character: " + battleManager.player.GetActiveCharacter().name);
//             LogToConsole("💡 Type 'switch <number>' to change active character (e.g., 'switch 2')");
//             LogToConsole("✅ Type 'ready' when you're satisfied with your active character to proceed");

//             LogToConsole("\nNew PvE battle started! Select your active character and type 'ready' to begin!");
//         }

//         private void StartPvPBattle()
//         {
//             battleManager.InitializePvPBattle();
//             battleManager.StartNewRound();
//             LogToConsole("New PvP battle started! First player to lose all characters or have lower HP when decks are empty loses!");
//         }

//         private void ShowEnemyCharacters()
//         {
//             LogToConsole("👹 === ENEMY CHARACTERS ===");

//             // Check for PvE enemies
//             if (battleManager.enemies != null && battleManager.enemies.Count > 0)
//             {
//                 for (int i = 0; i < battleManager.enemies.Count; i++)
//                 {
//                     var enemy = battleManager.enemies[i];
//                     string status = enemy.hp <= 0 ? "💀 DEFEATED" : "💪 ALIVE";

//                     LogToConsole($"   {i + 1}. {enemy.name} ({enemy.element}) - Level {enemy.level}");
//                     LogToConsole($"     HP: {enemy.hp}/{enemy.maxHP} | ATK: {enemy.attack} | DEF: {enemy.defense} {status}");
//                     if (!string.IsNullOrEmpty(enemy.passive))
//                     {
//                         LogToConsole($"     Passive: {enemy.passive}");
//                     }
//                     LogToConsole("");
//                 }
//             }
//             // Check for PvP enemy player
//             else if (battleManager.enemyPlayer?.characters != null && battleManager.enemyPlayer.characters.Count > 0)
//             {
//                 for (int i = 0; i < battleManager.enemyPlayer.characters.Count; i++)
//                 {
//                     var enemy = battleManager.enemyPlayer.characters[i];
//                     string status = enemy.hp <= 0 ? "💀 DEFEATED" : "💪 ALIVE";
//                     string activeMarker = (i == battleManager.enemyPlayer.activeCharacterIndex) ? "👑 " : "   ";
//                     string activeSuffix = (i == battleManager.enemyPlayer.activeCharacterIndex) ? " (ACTIVE)" : "";

//                     LogToConsole($"{activeMarker}{i + 1}. {enemy.name} ({enemy.element}) - Level {enemy.level}{activeSuffix}");
//                     LogToConsole($"     HP: {enemy.hp}/{enemy.maxHP} | ATK: {enemy.attack} | DEF: {enemy.defense} {status}");
//                     if (!string.IsNullOrEmpty(enemy.passive))
//                     {
//                         LogToConsole($"     Passive: {enemy.passive}");
//                     }
//                     LogToConsole("");
//                 }
//             }
//             else
//             {
//                 LogToConsole("⚠️ No enemy characters found");
//             }
//         }

//         private void ShowUserCharacters()
//         {
//             LogToConsole("🦸 === YOUR CHARACTERS ===");

//             if (battleManager.player?.characters == null || battleManager.player.characters.Count == 0)
//             {
//                 LogToConsole("⚠️ No player characters found");
//                 return;
//             }

//             for (int i = 0; i < battleManager.player.characters.Count; i++)
//             {
//                 var character = battleManager.player.characters[i];
//                 string status = character.hp <= 0 ? "💀 DEFEATED" : "💪 ALIVE";
//                 string activeMarker = (i == battleManager.player.activeCharacterIndex) ? "⭐ ACTIVE" : "        ";
//                 string activeSuffix = (i == battleManager.player.activeCharacterIndex) ? " (ACTIVE)" : "";

//                 LogToConsole($"{activeMarker} {i + 1}. {character.name} ({character.element}) - Level {character.level}{activeSuffix}");
//                 LogToConsole($"          HP: {character.hp}/{character.maxHP} | ATK: {character.attack} | DEF: {character.defense} {status}");
//                 if (!string.IsNullOrEmpty(character.passive))
//                 {
//                     LogToConsole($"          Passive: {character.passive}");
//                 }
//                 LogToConsole("");
//             }
//         }

//         private void StartRoundSequence()
//         {
//             if (battleManager.battleEnded)
//             {
//                 LogToConsole("Battle has already ended. Type 'start' to begin a new battle.");
//                 return;
//             }

//             LogToConsole("🚀 === STARTING ROUND SEQUENCE ===\n");

//             // Step 4: Display 6 cards rolled (draw hand)
//             battleManager.player.DrawHand();
//             LogToConsole("🃏 === YOUR HAND (6 CARDS) ===");
//             ShowHand();

//             // Step 5: Display Elemana Rolled
//             LogToConsole("\n🎲 === ELEMANA ROLL ===");
//             battleManager.player.elemanaPool.RollElemanas();
//             battleManager.isRerollPhase = true;

//             // Step 6: Ask if user wants to reroll or not
//             LogToConsole("\n💫 === REROLL PHASE ===");
//             LogToConsole("🔄 Type 'reroll <positions>' to reroll specific dice (e.g., 'reroll 1,3,5')");
//             LogToConsole("🔄 Type 'reroll <types>' to reroll by element (e.g., 'reroll Fire,Water')");
//             LogToConsole("✅ Type 'continue' to proceed with current elemana");
//             LogToConsole("📋 Type 'show' to see current elemana state");
//         }

//         private void ShowDeckStatus()
//         {
//             if (battleManager.player == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             int deckCount = battleManager.player.deck.Count;
//             int discardCount = battleManager.player.discardPile.Count;
//             int handCount = battleManager.player.hand.Count;
//             int PlayerCardsList = deckCount + discardCount + handCount;

//             LogToConsole($"=== DECK STATUS ===");
//             LogToConsole($"Cards in deck: {deckCount}");
//             LogToConsole($"Cards in hand: {handCount}");
//             LogToConsole($"Cards discarded: {discardCount}");
//             LogToConsole($"Total cards: {PlayerCardsList}");

//             if (deckCount == 0)
//             {
//                 LogToConsole("⚠️ DECK IS EMPTY! Battle will end when all characters fall.");
//                 LogToConsole($"Current HP total: {battleManager.player.GetCumulativeHP()}");
//             }
//         }

//         private void RerollElemana(string[] parts)
//         {
//             if (battleManager.player?.elemanaPool == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             if (!battleManager.player.elemanaPool.CanReroll())
//             {
//                 LogToConsole("No rerolls remaining this round! (Maximum 2 per round)");
//                 return;
//             }

//             if (parts.Length < 2)
//             {
//                 LogToConsole("🔄 === REROLL COMMAND HELP ===");
//                 LogToConsole("Usage: reroll <positions> OR reroll <elemana_types>");
//                 LogToConsole("");
//                 LogToConsole("🎯 Position-based (RECOMMENDED):");
//                 LogToConsole("   reroll 1,3,5     - Reroll dice at positions 1, 3, and 5");
//                 LogToConsole("   reroll 2,7       - Reroll dice at positions 2 and 7");
//                 LogToConsole("");
//                 LogToConsole("🎲 Type-based (legacy):");
//                 LogToConsole("   reroll Fire,Water,Dark - Reroll all Fire, Water, and Dark dice");
//                 LogToConsole("");
//                 LogToConsole("📋 Available types: Fire, Water, Ice, Metal, Lightning, Nature, Dark, Universal");
//                 LogToConsole($"💫 Rerolls remaining: {battleManager.player.elemanaPool.GetRerollsRemaining()}/2");
//                 LogToConsole("");
//                 LogToConsole("💡 Tip: Use position numbers for precise control!");
//                 ShowCurrentElemanaPositions();
//                 return;
//             }

//             string inputString = string.Join(",", parts.Skip(1));

//             // Try to parse as positions first (numbers)
//             var positions = battleManager.ParsePositions(inputString);
//             if (positions.Count > 0)
//             {
//                 bool success = battleManager.RerollElemanaPositions(positions);
//                 if (success)
//                 {
//                     LogToConsole("Elemana positions rerolled successfully!");
//                     ShowElemana();
//                 }
//                 return;
//             }

//             // Fall back to parsing as elemana types
//             var elemanaTypes = battleManager.ParseElemanaTypes(inputString);
//             if (elemanaTypes.Count > 0)
//             {
//                 bool success = battleManager.RerollElemana(elemanaTypes);
//                 if (success)
//                 {
//                     LogToConsole("Elemana types rerolled successfully!");
//                     ShowElemana();
//                 }
//                 return;
//             }

//             LogToConsole("Invalid input! Use either position numbers (1,3,5) or elemana types (Fire,Water,Dark)");
//         }

//         private void ShowCurrentElemanaPositions()
//         {
//             if (battleManager.player?.elemanaPool != null)
//             {
//                 LogToConsole("");
//                 LogToConsole(battleManager.player.elemanaPool.GetIndividualElemanaDisplay());
//             }
//         }

//         private void ShowStatus()
//         {
//             if (battleManager.battleEnded)
//             {
//                 LogToConsole($"Battle ended: {battleManager.battleResult}");
//                 return;
//             }

//             battleManager.PrintRoundStatus();
//         }

//         private void ShowHand()
//         {
//             if (battleManager.player?.hand == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             LogToConsole("=== YOUR HAND ===");
//             for (int i = 0; i < battleManager.player.hand.Count; i++)
//             {
//                 var card = battleManager.player.hand[i];
//                 bool canPlay = battleManager.player.elemanaPool.CanSpendForCard(
//                     card.element, card.cost, card.type == Imperius.Cards.CardType.Attack);
//                 string status = canPlay ? "[CAN PLAY]" : "[NOT ENOUGH ELEMANA]";
//                 LogToConsole($"{i + 1}. {card.title} ({card.element}, Cost: {card.cost}) {status}");
//                 LogToConsole($"   {card.description}");
//             }
//         }

//         private void ShowElemana()
//         {
//             if (battleManager.player?.elemanaPool == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             // Use the formatted display method for consistent output
//             LogToConsole(battleManager.player.elemanaPool.GetFormattedElemanaDisplay());

//             if (battleManager.isRerollPhase)
//             {
//                 LogToConsole("");
//                 if (battleManager.player.elemanaPool.CanReroll())
//                 {
//                     LogToConsole("🎯 To reroll specific positions: reroll 1,3,5");
//                     LogToConsole("🔄 To reroll by type: reroll Fire,Water,Dark");
//                     LogToConsole("📋 Available types: Fire, Water, Ice, Metal, Lightning, Nature, Dark, Universal");
//                 }
//                 LogToConsole("✅ Type 'continue' when satisfied with your elemana");
//             }
//             else
//             {
//                 LogToConsole("🔒 Reroll phase is over - these are your final elemana for this round.");
//             }
//         }

//         private void PlayCard(string[] parts)
//         {
//             if (battleManager.battleEnded)
//             {
//                 LogToConsole("Battle has ended! Type 'start' for a new battle.");
//                 return;
//             }

//             if (battleManager.isRerollPhase)
//             {
//                 LogToConsole("Cannot play cards during reroll phase! Type 'continue' to start the round.");
//                 return;
//             }

//             if (parts.Length < 2)
//             {
//                 LogToConsole("Usage: play <card_number> [target_numbers]");
//                 LogToConsole("Example: play 1 or play 2 1,3");
//                 return;
//             }

//             if (!int.TryParse(parts[1], out int cardIndex) || cardIndex < 1)
//             {
//                 LogToConsole("Invalid card number!");
//                 return;
//             }

//             cardIndex--; // Convert to 0-based index

//             List<int> targetIndices = new List<int>();
//             if (parts.Length > 2)
//             {
//                 string[] targets = parts[2].Split(',');
//                 foreach (string target in targets)
//                 {
//                     if (int.TryParse(target.Trim(), out int targetIndex) && targetIndex > 0)
//                     {
//                         targetIndices.Add(targetIndex - 1); // Convert to 0-based
//                     }
//                 }
//             }

//             bool success = battleManager.UseCard(cardIndex, targetIndices.Count > 0 ? targetIndices : null);

//             if (success)
//             {
//                 LogToConsole("Card played successfully!");
//                 // Show updated status after playing card
//                 ShowQuickStatus();
//             }
//         }

//         private void SwitchCharacter(string[] parts)
//         {
//             if (battleManager.isRerollPhase)
//             {
//                 LogToConsole("Cannot switch characters during reroll phase! Type 'continue' to start the round.");
//                 return;
//             }

//             if (parts.Length < 2)
//             {
//                 LogToConsole("Usage: switch <character_number>");
//                 return;
//             }

//             if (!int.TryParse(parts[1], out int charIndex) || charIndex < 1)
//             {
//                 LogToConsole("Invalid character number!");
//                 return;
//             }

//             charIndex--; // Convert to 0-based index

//             bool success = battleManager.SwitchCharacter(charIndex);
//             if (success)
//             {
//                 LogToConsole($"Switched to character {charIndex + 1}!");
//                 ShowQuickStatus();
//             }
//         }

//         private void RedrawHand()
//         {
//             if (battleManager.isRerollPhase)
//             {
//                 LogToConsole("Cannot redraw cards during reroll phase! Type 'continue' to start the round.");
//                 return;
//             }

//             bool success = battleManager.RedrawHand();
//             if (success)
//             {
//                 LogToConsole("Hand redrawn!");
//                 ShowHand();
//                 ShowElemana();
//             }
//         }

//         private void EndTurn()
//         {
//             if (battleManager.battleEnded)
//             {
//                 LogToConsole("Battle has ended!");
//                 return;
//             }

//             if (battleManager.isRerollPhase)
//             {
//                 LogToConsole("Cannot end turn during reroll phase! Type 'continue' to start the round.");
//                 return;
//             }

//             LogToConsole("Ending your turn...");
//             battleManager.EndPlayerTurn();
//         }

//         private void ContinueRound()
//         {
//             if (!battleManager.isRerollPhase)
//             {
//                 LogToConsole("Not in reroll phase! You're already in the main round.");
//                 return;
//             }

//             // Step 7: Final elemana display before starting round
//             LogToConsole("🔒 === LOCKING IN ELEMANA ===");
//             LogToConsole(battleManager.player.elemanaPool.GetFormattedElemanaDisplay());

//             // Step 8: Start Round
//             LogToConsole("\n⚔️ === ROUND STARTED ===");
//             battleManager.ContinueToMainRound();

//             LogToConsole("🎮 Your turn! Available actions:");
//             LogToConsole("🃏 'play <number>' - Play a card from your hand");
//             LogToConsole("🔄 'switch <number>' - Switch active character");
//             LogToConsole("📊 'status' - Check battle status");
//             LogToConsole("🏁 'endturn' - End your turn");
//         }

//         private void QuitBattle()
//         {
//             LogToConsole("Battle ended by player.");
//             battleManager.battleEnded = true;
//             battleManager.battleResult = "Quit by player";
//         }

//         private void ShowEnemies()
//         {
//             if (battleManager.enemies == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             LogToConsole("=== ENEMIES ===");
//             for (int i = 0; i < battleManager.enemies.Count; i++)
//             {
//                 var enemy = battleManager.enemies[i];
//                 string status = enemy.IsAlive() ? "Alive" : "Defeated";
//                 LogToConsole($"{i + 1}. {enemy.name} ({enemy.element})");
//                 LogToConsole($"   HP: {enemy.hp}/{enemy.maxHP} | Attack: {enemy.attack} | Defense: {enemy.defense} | {status}");
//             }
//         }

//         private void ShowCharacters()
//         {
//             if (battleManager.player?.characters == null)
//             {
//                 LogToConsole("No active battle. Type 'start' to begin.");
//                 return;
//             }

//             LogToConsole("=== YOUR CHARACTERS ===");
//             for (int i = 0; i < battleManager.player.characters.Count; i++)
//             {
//                 var character = battleManager.player.characters[i];
//                 string activeMarker = i == battleManager.player.activeCharacterIndex ? "*ACTIVE*" : "";
//                 string status = character.IsAlive() ? "Alive" : "Defeated";
//                 LogToConsole($"{i + 1}. {character.name} ({character.element}) {activeMarker}");
//                 LogToConsole($"   HP: {character.hp}/{character.maxHP} | Attack: {character.attack} | Defense: {character.defense} | {status}");
//                 LogToConsole($"   Passive: {character.passive}");
//             }
//         }
//         //Display Battle Status After A card is played
//         private void ShowQuickStatus()
//         {
//             if (battleManager.player == null) return;

//             LogToConsole("\n⚡ === POST-CARD STATUS UPDATE ===");

//             // Show enemy characters first to see the effects of the played card
//             ShowEnemyCharacters();

//             // Show user characters to see current state
//             ShowUserCharacters();

//             // Show quick elemana and hand info
//             LogToConsole("📊 === QUICK INFO ===");
//             LogToConsole($"Elemana: {battleManager.player.elemanaPool.GetDetailedElemanaString()}");
//             LogToConsole($"Hand: {battleManager.player.hand.Count} cards remaining");
//             LogToConsole("");
//         }

//         /// <summary>
//         /// Add a message to the console output
//         /// </summary>
//         private void LogToConsole(string message)
//         {
//             consoleOutput += "\n" + message;
//             Debug.Log(message); // Also log to Unity console

//             // Keep console output manageable (last 50 lines)
//             string[] lines = consoleOutput.Split('\n');
//             if (lines.Length > 50)
//             {
//                 consoleOutput = string.Join("\n", lines.Skip(lines.Length - 50));
//             }
//         }

//         /// <summary>
//         /// Clear the console output
//         /// </summary>
//         [ContextMenu("Clear Console")]
//         public void ClearConsole()
//         {
//             consoleOutput = "Console cleared.";
//         }

//         /// <summary>
//         /// Method to be called from Inspector buttons or other scripts
//         /// </summary>
//         [ContextMenu("Execute Command")]
//         public void ExecuteInputCommand()
//         {
//             ProcessCommand(currentInput);
//             currentInput = "";
//         }

//         /// <summary>
//         /// Quick command buttons for common actions
//         /// </summary>
//         [ContextMenu("Start Battle")]
//         public void QuickStartBattle()
//         {
//             ProcessCommand("start");
//         }

//         [ContextMenu("Show Status")]
//         public void QuickShowStatus()
//         {
//             ProcessCommand("status");
//         }

//         [ContextMenu("Show Hand")]
//         public void QuickShowHand()
//         {
//             ProcessCommand("hand");
//         }

//         [ContextMenu("Show Help")]
//         public void QuickShowHelp()
//         {
//             ProcessCommand("help");
//         }

//         [ContextMenu("Show Reroll Info")]
//         public void QuickShowRerollInfo()
//         {
//             if (battleManager.player?.elemanaPool != null)
//             {
//                 LogToConsole("=== ELEMANA REROLL INFO ===");
//                 LogToConsole($"Current: {battleManager.player.elemanaPool.GetDetailedElemanaString()}");
//                 LogToConsole($"Rerolls remaining: {battleManager.player.elemanaPool.GetRerollsRemaining()}/2");
//                 LogToConsole("Usage: Type 'reroll Fire,Water,Dark' to reroll those types");
//                 LogToConsole("Available: Fire, Water, Ice, Metal, Lightning, Nature, Dark, Universal");
//             }
//             else
//             {
//                 LogToConsole("No active battle. Start a battle first.");
//             }
//         }
//     }
// }