using System.Collections.Generic;
using UnityEngine;
using Imperius.Logic;
using Imperius.UI;
using System.Collections;
using TMPro;


namespace Imperius.Logic
{
    /// <summary>
    /// Manages battle flow, turns, and win conditions
    /// </summary>
    /// 

    public class BattleBL : MonoBehaviour
    {
        [Header("Battle Settings")]
        public int maxRounds = 20;

        [Header("Battle State")]

        public BattlePlayer player;
        public BattlePlayer enemy;

        public GameObject PlayerGameObject, EnemyGameObject; // references of the player and enemy Sprites

        // public List<Character> enemies; // For PvE battles
        public int currentRound;

        public bool isPlayerTurn;
        bool turnEnded = false;
        public ElemanaPool RoundEnergy;

        public GameObject PlayerSpawn_Char1, EnemySpawn_Char1;

        public GameObject cardPrefab;
        public Transform cardsPanel;

        //flags
        public bool battleEnded;
        public string battleResult;
        // public bool isPvPBattle = false;
        // public bool isRerollPhase = false;

        //UI

        public TMP_Text RoundLabel;
        public TMP_Text TimerLabel;
        public TMP_Text TurnLabel;
        private void Awake()
        {
            //InitializeBattle();
        }
        // private void Start()
        // {
        //     Debug.Log("Initializing Battle_Start...");
        //     InitializeBattle();
        // }

        /// <summary>
        /// Initialize the battle with player and enemies
        /// </summary>
        // public void InitializeBattle()
        // {
        //     Debug.Log("Initializing Battle...");

        //     // Create player
        //     player = BattleContext.Instance.Player;

        //     PlayerGameObject = BattleUI.CreatePlayerSprites();


        //     enemy = BattleContext.Instance.EnemyPlayer;

        //     EnemyGameObject = BattleUI.CreateEnemySprites();

        //     LobbyBL.InitializePlayer(enemy);

        //     // Create enemies (PvE mode)

        //     // CreateEnemies();
        //     // isPvPBattle = false;

        //     Debug.Log("Battle initialized!");
        //     StartCoroutine(RunBattle());
        // }

        //         IEnumerator RunBattle()
        // {
        //     currentRound = 0;
        //     battleEnded = false;
        //     battleResult = "";
        //     BattlePlayer ActivePlayer;

        //     //Find turn 
        //     int FirstTurn = UnityEngine.Random.Range(0, 2); // 0 or 1

        //     if (FirstTurn == 1)
        //     {
        //         isPlayerTurn = true;
        //         ActivePlayer = player;
        //         EnemyGameObject.GetComponent<Collider2D>().enabled = false;// do this for entire list
        //         PlayerGameObject.GetComponent<Collider2D>().enabled = true;
        //         Debug.Log("Player Turn");
        //     }
        //     else
        //     {
        //         isPlayerTurn = false;
        //         ActivePlayer = enemy;
        //         PlayerGameObject.GetComponent<Collider2D>().enabled = false;
        //         EnemyGameObject.GetComponent<Collider2D>().enabled = true;
        //         Debug.Log("Enemy Turn");
        //     }


        //     // first draw pile 
        //     // all of deck 

        //     foreach (var cd in GameManager.Instance.SelectedDeck.DeckCards)
        //     {
        //         player.DrawPile.Add(cd);
        //     }

        //     RoundLabel.text = "Round " + currentRound.ToString();

        //     while (!battleEnded)
        //     {
        //         TurnLabel.text = ActivePlayer.PlayerName + "'s Turn!";
        //         yield return StartCoroutine(RunTurn(ActivePlayer));
        //         isPlayerTurn = !isPlayerTurn;
        //         ActivePlayer = isPlayerTurn ? player : enemy;
        //     }

        //     // while (!battleEnded)
        //     // {
        //     //     ActivePlayer = isPlayerTurn ? player : enemy;
        //     //     // start some timer

        //     //     if (ActivePlayer == player)
        //     //     {
        //     //         //disp cards 

        //     //         // keyboard or mouse event to select card 

        //     //         // disp play card button

        //     //         //use mouse or keyboard to select enemy if card is direct 

        //     //         // exec card

        //     //     }

        //     // }

        // }

        // IEnumerator RunTurn(BattlePlayer ActivePlayer)
        // {
        //     turnEnded = false;

        //     GenerateHand(ActivePlayer);

        //     // populate CardsPanel 

        //     foreach (var card in ActivePlayer.Hand)
        //     {
        //         if (card is AttackCard ac)
        //         {

        //             UI_Util.CreateCardUI(ac, cardPrefab, cardsPanel);
        //         }
        //         //else if (card is SkillCard sc)
        //         //UI_Util.CreateCardUI(sc, cardPrefab, cardsPanel);
        //     }

        //     float timer = 20f;

        //     while (timer > 0f && !turnEnded)
        //     {

        //         TimerLabel.text = ((int)timer).ToString();
        //         timer -= Time.deltaTime;
        //         yield return null;

        //         // // turn ends when energy depleted
        //         // if (current.energy <= 0)
        //         //     turnEnded = true;
        //     }

        //     foreach (Transform child in cardsPanel)
        //     {
        //         Destroy(child.gameObject);
        //     }



        // }

        // public void GenerateHand(BattlePlayer ActivePlayer)
        // {
        //     // say hand has 6 cards, if draw pile < 6 then remake draw pile 

        //     int HandSize = 6;

        //     if (ActivePlayer.DrawPile.Count < HandSize)
        //     {
        //         if (ActivePlayer.DiscardPile.Count > 0)
        //         {
        //             Debug.Log("Reshuffling discard pile into draw pile.");
        //             ActivePlayer.DrawPile.AddRange(ActivePlayer.DiscardPile);
        //             ActivePlayer.DiscardPile.Clear();
        //         }
        //     }

        //     int cardsToDraw = Mathf.Min(HandSize, ActivePlayer.DrawPile.Count);
        //     for (int i = 0; i < cardsToDraw; i++)
        //     {
        //         int CardIndex = UnityEngine.Random.Range(0, ActivePlayer.DrawPile.Count);
        //         var drawnCard = ActivePlayer.DrawPile[CardIndex];
        //         ActivePlayer.Hand.Add(drawnCard);
        //         ActivePlayer.DrawPile.RemoveAt(CardIndex);
        //     }



        // }


        /// <summary>
        /// Initialize a PvP battle between two players
        /// </summary>
        // public void InitializePvPBattle()
        // {
        //     Debug.Log("Initializing PvP Battle...");

        //     // Create both players
        //     player = new Player("Player 1");
        //     player.InitializeDefaultSetup();

        //     enemyPlayer = new Player("Player 2");
        //     enemyPlayer.InitializeDefaultSetup();

        //     isPvPBattle = true;
        //     enemies = null; // No AI enemies in PvP

        //     currentRound = 0;
        //     battleEnded = false;
        //     battleResult = "";

        //     Debug.Log("PvP Battle initialized!");
        // }

        // /// <summary>
        // /// Create enemy characters for the battle
        // /// </summary>
        // private void CreateEnemies()
        // {
        //     enemies = new List<Character>();

        //     // Create 2-3 enemies with different elements and stats
        //     enemies.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Fire Demon", Element.Fire, 4, "Burns attackers for 5 damage"));

        //     enemies.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Ice Golem", Element.Ice, 5, "Reflects 20% of received damage"));

        //     enemies.Add(CharacterFactory.CreateLeveledCharacter(
        //         "Shadow Assassin", Element.Dark, 3, "Has 30% chance to dodge attacks"));

        //     Debug.Log($"Created {enemies.Count} enemies");
        // }

        // /// <summary>
        // /// Start a new round
        // /// </summary>
        // public void StartNewRound()
        // {
        //     if (battleEnded) return;

        //     currentRound++;
        //     Debug.Log($"\n=== ROUND {currentRound} ===");

        //     if (currentRound > maxRounds)
        //     {
        //         EndBattle("Draw - Maximum rounds reached!");
        //         return;
        //     }

        //     // Ensure active character is alive, switch to first alive character if needed
        //     EnsureActiveCharacterIsAlive();

        //     // Roll elemanas and enter reroll phase
        //     player.elemanaPool.RollElemanas();
        //     player.DrawHand();
        //     isRerollPhase = true;

        //     // Show the current elemana status after rolling
        //     Debug.Log("🎯 === REROLL PHASE ACTIVE ===");
        //     Debug.Log("💡 Type 'show' to see your current elemana if you missed it above.");
        //     Debug.Log("📝 Type 'reroll <positions>' to reroll (e.g., 'reroll 1,3,5')");
        //     Debug.Log("✅ Type 'continue' when you're satisfied with your elemana");
        // }

        // /// <summary>
        // /// Initialize round counter and ensure active character is ready (used by new flow)
        // /// </summary>
        // public void PrepareNewRound()
        // {
        //     if (battleEnded) return;

        //     currentRound++;
        //     Debug.Log($"\n=== ROUND {currentRound} PREPARATION ===");

        //     if (currentRound > maxRounds)
        //     {
        //         EndBattle("Draw - Maximum rounds reached!");
        //         return;
        //     }

        //     // Ensure active character is alive, switch to first alive character if needed
        //     EnsureActiveCharacterIsAlive();
        // }

        // /// <summary>
        // /// End the player's turn and process enemy actions
        // /// </summary>
        // public void EndPlayerTurn()
        // {
        //     Debug.Log("\n--- Player turn ended ---");

        //     // Process enemy turns (simple AI)
        //     ProcessEnemyTurns();

        //     // Check win conditions
        //     CheckBattleEnd();

        //     if (!battleEnded)
        //     {
        //         // Start next round
        //         StartNewRound();
        //     }
        // }

        // /// <summary>
        // /// Simple enemy AI - attack random player characters
        // /// </summary>
        // private void ProcessEnemyTurns()
        // {
        //     var aliveEnemies = enemies.Where(e => e.IsAlive()).ToList();
        //     var alivePlayerChars = player.characters.Where(c => c.IsAlive()).ToList();

        //     foreach (var enemy in aliveEnemies)
        //     {
        //         if (alivePlayerChars.Count == 0) break;

        //         // Enemy attacks a random alive player character
        //         var target = alivePlayerChars[UnityEngine.Random.Range(0, alivePlayerChars.Count)];
        //         int damage = enemy.GetAttackDamage();

        //         Debug.Log($"{enemy.name} attacks {target.name}!");
        //         target.TakeDamage(damage, enemy.element);

        //         // Update alive characters list
        //         alivePlayerChars = player.characters.Where(c => c.IsAlive()).ToList();
        //     }
        // }

        // /// <summary>
        // /// Check if battle should end
        // /// </summary>
        // private void CheckBattleEnd()
        // {
        //     if (isPvPBattle)
        //     {
        //         CheckPvPBattleEnd();
        //     }
        //     else
        //     {
        //         CheckPvEBattleEnd();
        //     }
        // }

        // /// <summary>
        // /// Check PvP battle end conditions
        // /// </summary>
        // private void CheckPvPBattleEnd()
        // {
        //     var alivePlayer1Chars = player.characters.Where(c => c.IsAlive()).ToList();
        //     var alivePlayer2Chars = enemyPlayer.characters.Where(c => c.IsAlive()).ToList();

        //     // Check if all characters of one side are defeated
        //     if (alivePlayer1Chars.Count == 0)
        //     {
        //         EndBattle("Player 2 Wins - All Player 1 characters defeated!");
        //         return;
        //     }
        //     else if (alivePlayer2Chars.Count == 0)
        //     {
        //         EndBattle("Player 1 Wins - All Player 2 characters defeated!");
        //         return;
        //     }

        //     // Check if both players have empty decks
        //     if (player.IsDeckEmpty() && enemyPlayer.IsDeckEmpty())
        //     {
        //         int player1HP = player.GetCumulativeHP();
        //         int player2HP = enemyPlayer.GetCumulativeHP();

        //         if (player1HP > player2HP)
        //         {
        //             EndBattle($"Player 1 Wins by HP - {player1HP} vs {player2HP}!");
        //         }
        //         else if (player2HP > player1HP)
        //         {
        //             EndBattle($"Player 2 Wins by HP - {player2HP} vs {player1HP}!");
        //         }
        //         else
        //         {
        //             EndBattle($"Draw - Both players have {player1HP} HP!");
        //         }
        //         return;
        //     }
        // }

        // /// <summary>
        // /// Check PvE battle end conditions
        // /// </summary>
        // private void CheckPvEBattleEnd()
        // {
        //     var aliveEnemies = enemies.Where(e => e.IsAlive()).ToList();
        //     var alivePlayerChars = player.characters.Where(c => c.IsAlive()).ToList();

        //     // Check if all characters of one side are defeated
        //     if (alivePlayerChars.Count == 0)
        //     {
        //         EndBattle("Defeat - All your characters have fallen!");
        //         return;
        //     }
        //     else if (aliveEnemies.Count == 0)
        //     {
        //         EndBattle("Victory - All enemies defeated!");
        //         return;
        //     }

        //     // In PvE, if player deck is empty, battle continues until characters die
        //     if (player.IsDeckEmpty())
        //     {
        //         Debug.Log("Player deck is empty! Battle continues until all characters fall.");
        //     }
        // }

        // /// <summary>
        // /// End the battle with a result
        // /// </summary>
        // private void EndBattle(string result)
        // {
        //     battleEnded = true;
        //     battleResult = result;

        //     Debug.Log($"\n=== BATTLE ENDED ===");
        //     Debug.Log(result);
        //     Debug.Log($"Battle lasted {currentRound} rounds");

        //     PrintFinalStatus();
        // }

        // /// <summary>
        // /// Use a card on targets
        // /// </summary>
        // public bool UseCard(int handIndex, List<int> targetIndices = null, bool targetEnemies = true)
        // {
        //     if (battleEnded)
        //     {
        //         Debug.Log("Battle has ended!");
        //         return false;
        //     }

        //     if (handIndex < 0 || handIndex >= player.hand.Count)
        //     {
        //         Debug.Log("Invalid card index!");
        //         return false;
        //     }

        //     Card card = player.hand[handIndex];
        //     List<Character> targets = new List<Character>();

        //     // Determine targets based on card type
        //     if (card is AttackCard || (card is SkillCard skill && skill.specialization == SkillSpecialization.Debuff))
        //     {
        //         // Attack cards and debuff skills target enemies
        //         var availableTargets = enemies.Where(e => e.IsAlive()).ToList();

        //         if (targetIndices != null)
        //         {
        //             foreach (int index in targetIndices)
        //             {
        //                 if (index >= 0 && index < availableTargets.Count)
        //                 {
        //                     targets.Add(availableTargets[index]);
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             // Default: target first alive enemy
        //             if (availableTargets.Count > 0)
        //                 targets.Add(availableTargets[0]);
        //         }
        //     }
        //     else
        //     {
        //         // Other skill cards target player characters
        //         var availableTargets = player.characters.Where(c => c.IsAlive()).ToList();

        //         if (targetIndices != null)
        //         {
        //             foreach (int index in targetIndices)
        //             {
        //                 if (index >= 0 && index < availableTargets.Count)
        //                 {
        //                     targets.Add(availableTargets[index]);
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             // Default: target active character
        //             var activeChar = player.GetActiveCharacter();
        //             if (activeChar != null && activeChar.IsAlive())
        //                 targets.Add(activeChar);
        //         }
        //     }

        //     return player.PlayCard(handIndex, targets);
        // }

        // /// <summary>
        // /// Switch active character
        // /// </summary>
        // public bool SwitchCharacter(int characterIndex)
        // {
        //     return player.SwitchActiveCharacter(characterIndex);
        // }

        // /// <summary>
        // /// Redraw hand
        // /// </summary>
        // public bool RedrawHand()
        // {
        //     return player.RedrawHand();
        // }

        // /// <summary>
        // /// Reroll selected elemana positions
        // /// </summary>
        // public bool RerollElemanaPositions(List<int> positions)
        // {
        //     if (battleEnded)
        //     {
        //         Debug.Log("Battle has ended!");
        //         return false;
        //     }

        //     return player.elemanaPool.RerollElemanaPositions(positions);
        // }

        // /// <summary>
        // /// Reroll selected elemana types (legacy support)
        // /// </summary>
        // public bool RerollElemana(List<ElemanaType> typesToReroll)
        // {
        //     if (battleEnded)
        //     {
        //         Debug.Log("Battle has ended!");
        //         return false;
        //     }

        //     return player.elemanaPool.RerollElemana(typesToReroll);
        // }

        // /// <summary>
        // /// Continue from reroll phase to main round
        // /// </summary>
        // public void ContinueToMainRound()
        // {
        //     if (!isRerollPhase)
        //     {
        //         Debug.Log("Not in reroll phase!");
        //         return;
        //     }

        //     // End reroll phase
        //     player.elemanaPool.EndRerollPhase();
        //     isRerollPhase = false;

        //     Debug.Log("\n=== ROUND STARTED ===");
        //     Debug.Log("You can now play cards, switch characters, or end your turn.");
        //     PrintRoundStatus();
        // }

        // /// <summary>
        // /// Parse position numbers from string (e.g., "1,3,5,7")
        // /// </summary>
        // // public List<int> ParsePositions(string positionsString)
        // // {
        // //     var result = new List<int>();
        // //     if (string.IsNullOrEmpty(positionsString)) return result;

        // //     string[] parts = positionsString.Split(',');
        // //     foreach (string part in parts)
        // //     {
        // //         string trimmed = part.Trim();
        // //         if (int.TryParse(trimmed, out int position))
        // //         {
        // //             result.Add(position);
        // //         }
        // //         else
        // //         {
        // //             Debug.Log($"Invalid position: {trimmed}");
        // //         }
        // //     }
        // //     return result;
        // // }

        // /// <summary>
        // /// Parse elemana types from string (e.g., "Fire,Water,Lightning")
        // /// </summary>
        // public List<ElemanaType> ParseElemanaTypes(string elemanaString)
        // {
        //     var result = new List<ElemanaType>();
        //     if (string.IsNullOrEmpty(elemanaString)) return result;

        //     string[] parts = elemanaString.Split(',');
        //     foreach (string part in parts)
        //     {
        //         string trimmed = part.Trim();
        //         if (System.Enum.TryParse<ElemanaType>(trimmed, true, out ElemanaType elemanaType))
        //         {
        //             result.Add(elemanaType);
        //         }
        //         else
        //         {
        //             Debug.Log($"Invalid elemana type: {trimmed}");
        //         }
        //     }
        //     return result;
        // }

        // /// <summary>
        // /// Print current round status
        // /// </summary>
        // // public void PrintRoundStatus()
        // // {
        // //     Debug.Log("\n" + player.GetStatusString());

        // //     Debug.Log("=== ENEMIES ===");
        // //     for (int i = 0; i < enemies.Count; i++)
        // //     {
        // //         string status = enemies[i].IsAlive() ? "Alive" : "Defeated";
        // //         Debug.Log($"{i + 1}. {enemies[i].name} - {enemies[i].hp}/{enemies[i].maxHP} HP ({status})");
        // //     }

        // //     Debug.Log("\n=== YOUR HAND ===");
        // //     for (int i = 0; i < player.hand.Count; i++)
        // //     {
        // //         Card card = player.hand[i];
        // //         string canPlay = player.elemanaPool.CanSpendForCard(card.element, card.cost, card.type == CardType.Attack)
        // //                         ? "[CAN PLAY]" : "[NOT ENOUGH ELEMANA]";
        // //         Debug.Log($"{i + 1}. {card.ToString()} {canPlay}");
        // //     }
        // // }

        // /// <summary>
        // /// Print final battle status
        // /// </summary>
        // // private void PrintFinalStatus()
        // // {
        // //     Debug.Log("\n=== FINAL STATUS ===");
        // //     Debug.Log("Your Characters:");
        // //     foreach (var character in player.characters)
        // //     {
        // //         string status = character.IsAlive() ? $"{character.hp}/{character.maxHP} HP" : "Defeated";
        // //         Debug.Log($"- {character.name}: {status}");
        // //     }

        // //     Debug.Log("\nEnemies:");
        // //     foreach (var enemy in enemies)
        // //     {
        // //         string status = enemy.IsAlive() ? $"{enemy.hp}/{enemy.maxHP} HP" : "Defeated";
        // //         Debug.Log($"- {enemy.name}: {status}");
        // //     }
        // // }

        // /// <summary>
        // /// Ensure the active character is alive, switch to first alive character if not
        // /// </summary>
        // private void EnsureActiveCharacterIsAlive()
        // {
        //     var activeChar = player.GetActiveCharacter();
        //     if (activeChar == null || !activeChar.IsAlive())
        //     {
        //         // Find first alive character
        //         for (int i = 0; i < player.characters.Count; i++)
        //         {
        //             if (player.characters[i].IsAlive())
        //             {
        //                 player.activeCharacterIndex = i;
        //                 Debug.Log($"Active character switched to {player.characters[i].name} (previous was defeated)");
        //                 return;
        //             }
        //         }
        //         // If no alive characters, the battle should end
        //         Debug.Log("No alive characters remaining!");
        //     }
        // }

        /// <summary>
        /// Get available console commands help
        /// </summary>
        //         public string GetCommandsHelp()
        //         {
        //             return @"

        // === CONSOLE COMMANDS ===
        // start - Start a new PvE battle
        // startpvp - Start a new PvP battle
        // ready - Proceed to round sequence (after selecting active character)
        // status - Show current battle status
        // hand - Show your current hand
        // elemana/show - Show current elemana and reroll status
        // deck - Show deck status and card counts
        // characters - Show your characters
        // enemies - Show enemy status
        // continue - Continue from reroll phase to main round
        // play <card_number> [target_numbers] - Play a card (e.g., 'play 1', 'play 2 1,3')
        // switch <character_number> - Switch active character (costs 1 elemana, only alive characters)
        // redraw - Redraw your hand (costs 1 elemana)
        // reroll <positions/types> - Reroll elemana (e.g., 'reroll 1,3,5' or 'reroll Fire,Water')
        // endturn - End your turn
        // help - Show this help
        // quit - End the battle

        // NEW BATTLE FLOW:
        // 1. Type 'start' → View enemy characters → View your characters
        // 2. Select active character with 'switch <number>' → Type 'ready'
        // 3. View 6 cards drawn → View elemana rolled
        // 4. REROLL PHASE: Reroll elemana up to 2 times → Type 'continue'
        // 5. MAIN ROUND: Play cards, switch characters, end turn
        // 5. Rerolls are NOT available during the main round

        // ELEMANA REROLL RULES:
        // - Rerolls ONLY available at the start of the round
        // - Up to 2 rerolls per round during reroll phase
        // - POSITION-BASED: reroll 1,3,5 (reroll dice at positions 1, 3, and 5)
        // - TYPE-BASED: reroll Fire,Water,Lightning (reroll all of those types)
        // - Available types: Fire, Water, Ice, Metal, Lightning, Nature, Dark, Universal

        // BATTLE RULES:
        // - First player to lose all 3 characters LOSES
        // - If both decks are empty, player with higher cumulative HP WINS
        // - Cards used in battle are discarded (don't return to hand)
        // - Dead characters cannot be switched to or use passives
        // - Attack cards need matching/universal elemana, skills use any elemana

        // Examples:
        // - reroll 1,3,5 - Reroll dice at positions 1, 3, and 5 during reroll phase
        // - reroll Fire,Water - Reroll all Fire and Water dice during reroll phase
        // - continue - Start the main round
        // - play 1 - Play first card on default target
        // - switch 2 - Switch to character 2 (if alive)
        // ";
        //         }

    }
}