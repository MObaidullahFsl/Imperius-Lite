// using UnityEngine;
// using Imperius.Logic;
// using Imperius.Logic;

// namespace Imperius.Logic
// {
//     /// <summary>
//     /// Simple setup script to initialize the console game
//     /// Attach this to an empty GameObject in your scene
//     /// </summary>
//     public class GameSetup : MonoBehaviour
//     {
//         // [Header("Setup Options")]
//         [Tooltip("Automatically start the game when scene loads")]
//         public bool autoStart = true;

//         [Header("References")]
//         public ConsoleGameController consoleController;

//         private void Start()
//         {
//             InitializeGame();

//             if (autoStart)
//             {
//                 ShowWelcomeMessage();
//             }
//         }

//         private void InitializeGame()
//         {
//             // Find or create console controller
//             if (consoleController == null)
//             {
//                 consoleController = FindFirstObjectByType<ConsoleGameController>();

//                 if (consoleController == null)
//                 {
//                     GameObject controllerGO = new GameObject("ConsoleGameController");
//                     consoleController = controllerGO.AddComponent<ConsoleGameController>();
//                 }
//             }

//             Debug.Log("Imperius Console Battle initialized!");
//         }

//         private void ShowWelcomeMessage()
//         {
//             Debug.Log("=== IMPERIUS CONSOLE BATTLE ===");
//             Debug.Log("Welcome to the console-based card battle game!");
//             Debug.Log("");
//             Debug.Log("HOW TO PLAY:");
//             Debug.Log("1. Look at the ConsoleGameController component in the Inspector");
//             Debug.Log("2. Type commands in the 'Current Input' field");
//             Debug.Log("3. Press Enter in the Scene/Game view or click 'Execute Input Command' button");
//             Debug.Log("4. Watch the 'Console Output' field for game responses");
//             Debug.Log("");
//             Debug.Log("QUICK START:");
//             Debug.Log("- Type 'start' to begin a battle");
//             Debug.Log("- Type 'help' to see all available commands");
//             Debug.Log("- Type 'status' to see current game state");
//             Debug.Log("");
//             Debug.Log("EXAMPLE COMMANDS:");
//             Debug.Log("- start (start new battle)");
//             Debug.Log("- hand (show your cards)");
//             Debug.Log("- play 1 (play first card)");
//             Debug.Log("- switch 2 (switch to character 2)");
//             Debug.Log("- endturn (end your turn)");
//         }

//         /// <summary>
//         /// Method callable from Inspector button
//         /// </summary>
//         [ContextMenu("Show Help")]
//         public void ShowHelp()
//         {
//             if (consoleController != null)
//             {
//                 consoleController.ProcessCommand("help");
//             }
//         }

//         /// <summary>
//         /// Method callable from Inspector button
//         /// </summary>
//         [ContextMenu("Start New Battle")]
//         public void StartNewBattle()
//         {
//             if (consoleController != null)
//             {
//                 consoleController.ProcessCommand("start");
//             }
//         }
//     }
// }