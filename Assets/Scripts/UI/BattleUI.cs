using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
using System.Data.Common;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using NUnit.Framework;
using System.Linq;

namespace Imperius.UI
{

    public enum BattleState { Starting, PlayerTurn, EnemyTurn, Victory, Defeat }

    public class BattleUI : MonoBehaviour
    {

        // round timer
        // current round
        // rerolls left
        // powerups or passives
        // character hps
        // characters
        // hand cards left, draw pile, discard pile
        // cards of character in hand
        BattleBL battleBL;

        List<GameObject> playerSprites;

        List<GameObject> enemySprites;



        List<GameObject> playerSpawns;
        List<GameObject> enemySpawns;
        List<GameObject> playerHealthBars;
        List<GameObject> enemyHealthBars;

        public GameObject canvas;

        public int selectedCharIndex = 0;

        public BattleCharacter selectedCharacter;

        public GameObject pointer;

        public BattleState bs;

        [SerializeField] private TextMeshProUGUI Turn;
        [SerializeField] private TextMeshProUGUI Timer;
        [SerializeField] private TextMeshProUGUI DrawLabel;
        [SerializeField] private TextMeshProUGUI DiscardLabel;
        [SerializeField] private TextMeshProUGUI EnergyLabel;
        [SerializeField] private Button EndTurnButton;
        private Keyboard keyboard => Keyboard.current;

        bool turnInProgress = false;

        public AttackCard waitingForTargetCard = null;

        public int totalEnergy;

        public int currentEnergy;

        private Coroutine playerTurnCoroutine;
        private Coroutine enemyTurnCoroutine;

        private Button EndTurnBtn;

        void Start()
        {
            battleBL = new BattleBL();
            playerSpawns = new List<GameObject>();
            enemySpawns = new List<GameObject>();
            playerSprites = new List<GameObject>();
            enemySprites = new List<GameObject>();
            playerHealthBars = new List<GameObject>();
            enemyHealthBars = new List<GameObject>();


            playerSpawns.Clear();
            enemySpawns.Clear();

            totalEnergy = 4;
            currentEnergy = 0;
            // Add each spawn point by name
            for (int i = 1; i <= 3; i++)
            {
                string spawnName = $"Player_Spawn_{i}";
                string enemySpawnName = $"Enemy_Spawn_{i}";
                GameObject spawnObj = GameObject.Find($"Canvas/Level/{spawnName}"); ;
                GameObject enemySpawnObj = GameObject.Find($"Canvas/Level/{enemySpawnName}"); ;

                if (spawnObj != null)
                {
                    playerSpawns.Add(spawnObj);
                }
                else
                {
                    Debug.LogError($"Spawn point '{spawnName}' not found in scene!");
                }
                if (enemySpawnObj != null)
                {
                    enemySpawns.Add(enemySpawnObj);
                }
                else
                {
                    Debug.LogError($"Spawn point '{enemySpawnName}' not found in scene!");
                }
            }
            bs = BattleState.Starting;

            canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("No Canvas found in scene!");
                return;
            }

            Turn = canvas.transform.Find("Turn").GetComponent<TextMeshProUGUI>();

            if (!Turn)
            {
                Debug.LogError("Turn TMP component not found!");
            }

            Turn.text = "Battle Starting";

            Timer = canvas.transform.Find("Timer").GetComponent<TextMeshProUGUI>();

            if (!Timer)
            {
                Debug.LogError("Timer TMP component not found!");
            }

            DrawLabel = canvas.transform.Find("DrawLabel").GetComponent<TextMeshProUGUI>();

            if (!DrawLabel)
            {
                Debug.LogError("DrawLabel TMP component not found!");
            }
            DiscardLabel = canvas.transform.Find("DiscardLabel").GetComponent<TextMeshProUGUI>();

            if (!DiscardLabel)
            {
                Debug.LogError("DiscardLabel TMP component not found!");
            }

            EnergyLabel = canvas.transform.Find("EnergyLabel").GetComponent<TextMeshProUGUI>();

            if (!EnergyLabel)
            {
                Debug.LogError("EnergyLabel TMP component not found!");
            }

            EndTurnBtn = GameObject.Find("BasicButton").GetComponent<Button>();
            if (EndTurnBtn == null)
            {
                Debug.LogError("EndTurnButton not found in scene!");

            }
            else
            {

                EndTurnBtn.onClick.RemoveAllListeners();
                EndTurnBtn.onClick.AddListener(EndTurnButton_OnClick);

            }




            InitiateBattleUI();

            StartBattle();

        }
        void Update()
        {

            if (bs != BattleState.PlayerTurn) return;
            if (keyboard == null) return;

            if (playerSprites == null || playerSprites.Count == 0) return;
            if (pointer == null) return;

            if (keyboard.rightArrowKey.wasPressedThisFrame)
            {
                selectedCharIndex = (selectedCharIndex + 1) % playerSprites.Count;
            }

            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                selectedCharIndex = (selectedCharIndex - 1 + playerSprites.Count) % playerSprites.Count;
            }

            if (BattleContext.Instance.PlayerTeam == null || BattleContext.Instance.PlayerTeam.Count == 0)
            {
                selectedCharacter = null;
                return;
            }

            // Clamp index to PlayerTeam size
            selectedCharIndex = Mathf.Clamp(selectedCharIndex, 0, BattleContext.Instance.PlayerTeam.Count - 1);


            // Update BattleContext selection
            selectedCharacter = BattleContext.Instance.PlayerTeam[selectedCharIndex];
            BattleContext.Instance.SelectedPlayerChar = selectedCharacter;
            if (pointer != null)
            {
                updatePointer();
            }


            // void Update()
            // {

            // if (keyboard.rightArrowKey.wasPressedThisFrame)
            //     selectedCharIndex = (selectedCharIndex + 1) % playerSprites.Count;

            // if (keyboard.leftArrowKey.wasPressedThisFrame)
            //     selectedCharIndex = (selectedCharIndex - 1 + playerSprites.Count) % playerSprites.Count;

            // selectedCharacter = BattleContext.Instance.PlayerTeam[selectedCharIndex];
            // BattleContext.Instance.SelectedPlayerChar = selectedCharacter;

            // updatePointer();
            // }



        }

        private void EndTurnButton_OnClick()
        {

            Debug.Log("End Turn button clicked by player.");
            var modal = UI_Util.CreateModal("Turn Ended");
            modal.AddContentText("You have ended your turn.");
        }

        public bool battleEnded()
        {
            bool enemyWin = true;
            bool playerWin = true;
            var enemyTeam = BattleContext.Instance.EnemyTeam;

            if (enemyTeam != null && enemyTeam.Count != 0)
            {
                // If ALL player characters have BattleHp <= 0, enemy wins

                foreach (var c in BattleContext.Instance.PlayerTeam)
                {
                    Debug.Log("Player character " + c.name + " HP: " + c.BattleHp);
                    if (c.BattleHp > 0)
                    {
                        enemyWin = false;
                    }

                }
                Debug.Log("Enemy win check: " + enemyWin);
                if (enemyWin)
                {
                    bs = BattleState.Defeat;

                }
            }
            var playerTeam = BattleContext.Instance.PlayerTeam;

            if (playerTeam != null && playerTeam.Count != 0)
            {
                // If ALL enemy characters have BattleHp <= 0, player wins
                foreach (var c in BattleContext.Instance.EnemyTeam)
                {
                    Debug.Log("Enemy character " + c.name + " HP: " + c.BattleHp);
                    if (c.BattleHp > 0)
                    {
                        playerWin = false;
                    }

                }
                Debug.Log("Player win check: " + playerWin);
                if (playerWin)
                {

                    bs = BattleState.Victory;
                }
            }

            return enemyWin || playerWin;

        }


        public void enablePointer()
        {

            if (canvas == null)
            {
                Debug.LogError("No Canvas found in scene!");
                return;
            }
            if (pointer != null)
            {
                Debug.Log("Pointer already enabled, skipping.");
                Destroy(pointer); // Prevent duplicates
            }

            if (BattleContext.Instance.PlayerTeam == null || BattleContext.Instance.PlayerTeam.Count == 0)
            {
                Debug.LogError("Player team empty — cannot enable pointer.");
                return;
            }
            pointer = Instantiate(Resources.Load<GameObject>("Prefabs/arrow"), canvas.transform);
            Debug.Log("Loaded arrow = " + pointer);
            selectedCharIndex = 0;
            selectedCharacter = BattleContext.Instance.PlayerTeam[selectedCharIndex];
            BattleContext.Instance.SelectedPlayerChar = selectedCharacter;
            updatePointer();


        }

        public void disablePointer()
        {
            if (pointer != null)
            {
                Destroy(pointer);
                pointer = null; // Important: prevents lingering reference
                Debug.Log("Pointer disabled and destroyed.");
            }
        }


        public void updatePointer()
        {

            if (pointer == null)
                return; // pointer destroyed, nothing to update

            if (playerSprites == null || playerSprites.Count == 0)
                return; // no sprites to reference

            // if (selectedCharIndex < 0 || selectedCharIndex >= playerSprites.Count) selectedCharIndex = 0;
            // // clamp to valid range

            if (selectedCharIndex == 0)
            {
                //Debug.Log("Pointer at 1st character"); 
                pointer.transform.position = playerSprites[0].transform.position + new Vector3(0, -100f, 0);
            }
            else if (selectedCharIndex == 1)
            { //Debug.Log("Pointer at 2nd character"); 
                pointer.transform.position = playerSprites[1].transform.position + new Vector3(0, -100f, 0);
            }
            else if (selectedCharIndex == 2)
            { //Debug.Log("Pointer at 3rd character"); 
                pointer.transform.position = playerSprites[2].transform.position + new Vector3(0, -100f, 0);
            }
            else { Debug.Log("selectedCharIndex out of bounds"); }


            GameObject selectedSprite = playerSprites[selectedCharIndex];
            if (selectedSprite == null)
                return; // sprite destroyed

            pointer.transform.position = selectedSprite.transform.position + new Vector3(0, -70f, 0);

        }


        public void InitiateBattleUI()
        {

            battleBL.initializeBattleContext();


            CreatePlayerSprites();

            CreateEnemySprites();

            // You can add more initialization logic here as needed
        }

        public void CreatePlayerSprites()
        {

            if (playerSprites.Count > 0)
            {
                Debug.Log("Player sprites already created, skipping.");
                //RemoveSprites(true);

            }
            playerSprites.Clear(); playerHealthBars.Clear();
            for (int i = 0; i < BattleContext.Instance.PlayerTeam.Count; i++)
            {
                BattleCharacter c = BattleContext.Instance.PlayerTeam[i];

                // Create the character prefab
                GameObject go = CreateCharacterSprites(c);

                if (go != null)
                {
                    // Place it at the corresponding spawn point
                    if (i < playerSpawns.Count && playerSpawns[i] != null)
                    {
                        go.transform.SetParent(playerSpawns[i].transform.parent, false);
                        go.transform.localPosition = playerSpawns[i].transform.localPosition;
                        go.transform.localScale = new Vector3(0.9f, 0.9f, 1f);

                    }
                    else
                    {
                        Debug.LogWarning("Not enough spawn points for player index " + i);
                    }

                    playerSprites.Add(go);

                    if (c.healthBarUI != null)
                    {
                        Debug.Log("Health bar already exists for character " + c.name);
                        UpdateHealthBar(c);
                    }
                    else
                    {
                        
                    AddHealthBar(go, c, true);
                    }

                }
            }

        }

        public void CreateEnemySprites()
        {

            if (enemySprites.Count > 0)
            {
                Debug.Log("Player sprites already created, skipping.");
                //RemoveSprites(false);

            }
            enemySprites.Clear(); enemyHealthBars.Clear();
            for (int i = 0; i < BattleContext.Instance.EnemyTeam.Count; i++)
            {
                BattleCharacter c = BattleContext.Instance.EnemyTeam[i];

                // Create the character prefab
                GameObject go = CreateCharacterSprites(c);
                var trigger = go.AddComponent<UnityEngine.UI.Button>();
                trigger.onClick.AddListener(() => OnPointerClick(null, c));
                if (go != null)
                {
                    // Place it at the corresponding spawn point
                    if (i < enemySpawns.Count && enemySpawns[i] != null)
                    {
                        go.transform.SetParent(enemySpawns[i].transform.parent, false);
                        go.transform.localPosition = enemySpawns[i].transform.localPosition;
                        go.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
                        Vector3 scale = go.transform.localScale;
                        scale.x *= -1; // invert X
                        go.transform.localScale = scale;

                        c.charUI = go;

                    }
                    else
                    {
                        Debug.LogWarning("Not enough spawn points for enemy index " + i);
                    }

                    enemySprites.Add(go);
//                    AddHealthBar(go, c, false);

                      if (c.healthBarUI != null)
                    {
                        Debug.Log("Health bar already exists for character " + c.name);
                        UpdateHealthBar(c);
                    }
                    else
                    {
                        
                    AddHealthBar(go, c, false);
                    }

                }

            }
        }

        public void OnPointerClick(PointerEventData eventData, BattleCharacter character)
        {


            if (waitingForTargetCard == null)
            {
                Debug.Log("No card is waiting for targeting.");
                return; // Not waiting for targeting
            }
            if (bs != BattleState.PlayerTurn)
            {
                Debug.Log("Not player's turn, cannot target.");
                return;
            }

            Debug.Log($"Enemy clicked! Using card: {waitingForTargetCard.title} on {character.name}");

            // Set target in context
            BattleContext.Instance.SelectedEnemyChar = character;

            // Apply effect
            waitingForTargetCard.effect(BattleContext.Instance, waitingForTargetCard);

            Debug.Log($"Enemy new HP = {character.BattleHp}");

            BattleContext.Instance.PlayerCardPile.Hand.Remove(waitingForTargetCard);
            BattleContext.Instance.PlayerCardPile.Discard.Add(waitingForTargetCard);
            currentEnergy -= waitingForTargetCard.cost;
            EnergyLabel.text = "Energy: " + currentEnergy.ToString() + "/" + totalEnergy.ToString();
            DiscardLabel.text = "Discard Pile: " + BattleContext.Instance.PlayerCardPile.Discard.Count.ToString();
            UpdateHealthBar(character);


            // Destroy UI
            if (waitingForTargetCard.cardUI != null)
                GameObject.Destroy(waitingForTargetCard.cardUI);

            // Clear targeting mode
            waitingForTargetCard = null;

            if (battleEnded())
            {
                Debug.Log("Battle ended after playing card.");
                endGame();
            }

        }

        public void StartBattle()
        {

            // get active player from server

            if (isActivePlayer())
            {
                Debug.Log("Player turn about to start");

                PlayerTurn();
            }
            else
            {
                Debug.Log("Enemy turn about to start");
                // Enemy logic here

                EnemyTurn();

                bs = BattleState.EnemyTurn;
            }
        }

        void ClearHandUI()
        {
            Transform panel = GameObject.Find("Canvas/CardSection").transform;

            foreach (Transform child in panel)
                GameObject.Destroy(child.gameObject);
        }
        public void AddHealthBar(GameObject charObj, BattleCharacter character, bool isPlayer)
        {


            if (charObj == null || character == null) return;

            if (character.healthBarUI != null)
            {
                Debug.Log("Health bar already exists for character " + character.name);
                UpdateHealthBar(character);
                return; // already has a health bar
            }

            // Load prefab from Resources
            GameObject hbPrefab = Resources.Load<GameObject>("Prefabs/healthbar");
            if (hbPrefab == null)
            {
                Debug.LogError("HealthBar prefab not found!");
                return;
            }

            // Instantiate under canvas
            GameObject canvas = GameObject.Find("Canvas");
            GameObject hbObj = Instantiate(hbPrefab, canvas.transform);

            character.healthBarUI = hbObj;
            // Position relative to character like pointer
            Vector3 offset = new Vector3(-5f, 80f, 0); // adjust Y offset above sprite
            hbObj.transform.position = charObj.transform.position + offset;

            if (isPlayer)
                playerHealthBars.Add(hbObj);
            else
                enemyHealthBars.Add(hbObj);
            UpdateHealthBar(character);
        }

        public void UpdateHealthBar(BattleCharacter character)
        {
            // Find the bar (child with Image)
            GameObject hbObj = character.healthBarUI;
            if (hbObj == null) return;

            Transform barTransform = hbObj.transform.Find("bar");
            if (barTransform == null)
            {
                Debug.LogError("HealthBar prefab missing child 'bar'");
                return;
            }

            RectTransform barRect = barTransform.GetComponent<RectTransform>();
            if (barRect == null)
            {
                Debug.LogError("Bar missing RectTransform!");
                return;
            }

            float hpPercent = Mathf.Clamp01((float)character.BattleHp / character.hp);
            float lostPercent = 1f - hpPercent;

            // === New logic: shrink using right margin ===
            // Stretch anchors must be full width (minX = 0, maxX = 1)
            // left stays 0, right moves inward based on lost hp
            float parentWidth = ((RectTransform)hbObj.transform).rect.width;
            float rightOffset = parentWidth * lostPercent;

            // offsetMax.x controls right margin (negative = shrink)
            barRect.offsetMax = new Vector2(-rightOffset, barRect.offsetMax.y);

            // color
            Image barImage = barTransform.GetComponent<Image>();
            barImage.color = (hpPercent > 0.1f) ? Color.green : Color.red;
        }


        public void RemoveSprites(bool isPlayer)
        {
            if (isPlayer)
            {
                foreach (var hb in playerHealthBars)
                {
                    Destroy(hb);
                }
                foreach (var c in playerSprites)
                {
                    Destroy(c);
                }
                playerHealthBars.Clear();
            }
            else
            {
                foreach (var hb in enemyHealthBars)
                {
                    Destroy(hb);
                }
                foreach (var c in enemySprites)
                {
                    Destroy(c);
                }
                enemyHealthBars.Clear();
            }

        }
        public void PlayerTurn()
        {
            // hand has 7 at most cards and 1 atleast 

            if (turnInProgress)
            {
                Debug.Log("Turn already in progress, cannot start another.");
                return;
            }
            turnInProgress = true;

            if (enemyTurnCoroutine != null)
            {
                Debug.Log("Stopping enemy turn coroutine.");
                // StopCoroutine(enemyTurnCoroutine);
                enemyTurnCoroutine = null;
            }
            if (bs == BattleState.Victory || bs == BattleState.Defeat)
            {
                Debug.Log("Battle already ended, cannot start player turn.");
                return;
            }
            bs = BattleState.PlayerTurn;

            ClearHandUI();

            CreatePlayerSprites();


            if (BattleContext.Instance.PlayerCardPile.Hand.Count > 0)
            {
                Debug.Log("Flushing hand at start of turn");
                BattleContext.Instance.PlayerCardPile.FlushPile(CardPile.PileType.Hand);
            }

            if (BattleContext.Instance.PlayerCardPile.Draw.Count == 0)
            {
                Debug.Log("Draw is empty, flushing!");


                if (BattleContext.Instance.PlayerCardPile.Discard.Count == 0)
                {
                    Debug.Log("No cards to draw after flushing, ending turn.");
                    return;
                }

                BattleContext.Instance.PlayerCardPile.FlushPile(CardPile.PileType.Discard);
            }

            for (int i = 0; i < 6; i++)
            {
                if (BattleContext.Instance.PlayerCardPile.DrawCard())
                {
                    Debug.Log("Drawn a card for player");

                }
                else
                {
                    return;
                }
            }

            //print hand 

            foreach (var c in BattleContext.Instance.PlayerCardPile.Hand)
            {
                Debug.Log("Player hand contains: " + c.title);
            }


            DiscardLabel.text = "Discard Pile: " + BattleContext.Instance.PlayerCardPile.Discard.Count.ToString();
            DrawLabel.text = "Draw Pile: " + BattleContext.Instance.PlayerCardPile.Draw.Count.ToString();
            currentEnergy = totalEnergy;
            EnergyLabel.text = "Energy: " + currentEnergy.ToString() + "/" + totalEnergy.ToString();


            foreach (var card in BattleContext.Instance.PlayerCardPile.Hand)
            {
                if (card is AttackCard ac)
                {
                    Debug.Log("Creating card in hand: " + ac.title);
                    GameObject cardObj = UI_Util.CreateCardUI(ac, Resources.Load<GameObject>("Prefabs/CardUI"), GameObject.Find("Canvas/CardSection").transform);
                    card.cardUI = cardObj;
                    var trigger = cardObj.AddComponent<UnityEngine.UI.Button>();
                    trigger.onClick.AddListener(() => OnCardClicked((AttackCard)card));
                }
            }

            Turn.text = "Player Turn";

            // activate click event for cards 

            //activate pointer
            enablePointer();

            // send server start signal 

            playerTurnCoroutine = StartCoroutine(TurnTimer(20, () =>
            {
                Debug.Log("Player turn ended");
                //Server.SendTurnEnded(BattleContext.Instance.currentRound);

                turnInProgress = false;
                //EnableCharacterSelection(false);
                disablePointer();

                // check win condition here

                EnemyTurn();
            }));

        }


        public void OnCardClicked(AttackCard card)
        {
            // == Needs targeting ==
            if (currentEnergy < card.cost)
            {
                Debug.Log("Not enough energy to play this card.");
                return;
            }

            if (card.needsTargeting)
            {
                waitingForTargetCard = card;
                Debug.Log($"Card clicked: {card.title}. Click on an enemy to use this card!");
                return;
            }


            // == No targeting needed ==
            Debug.Log($"Using card instantly: {card.title}");
            card.effect(BattleContext.Instance, card);

            Debug.Log("Card effect applied instantly.");

            BattleContext.Instance.PlayerCardPile.Hand.Remove(card);

            BattleContext.Instance.PlayerCardPile.Discard.Add(card);

            currentEnergy -= card.cost;
            EnergyLabel.text = "Energy: " + currentEnergy.ToString() + "/" + totalEnergy.ToString();
            DiscardLabel.text = "Discard Pile: " + BattleContext.Instance.PlayerCardPile.Discard.Count.ToString();

            UpdateHealthBar(BattleContext.Instance.SelectedEnemyChar);

            if (battleEnded())
            {
                Debug.Log("Battle ended after playing card.");
                endGame();
            }

            // Destroy UI
            if (card.cardUI != null)
                GameObject.Destroy(card.cardUI);



        }

        public void endGame()
        {
            StopAllCoroutines();
            BattleContext.ResetInstance();

            if (bs == BattleState.PlayerTurn)
            {
                if (playerTurnCoroutine != null)
                {
                    StopCoroutine(playerTurnCoroutine);
                    Debug.Log("Stopped player turn coroutine on battle end.");
                    playerTurnCoroutine = null;

                }
            }
            else if (bs == BattleState.EnemyTurn)
            {
                if (enemyTurnCoroutine != null)
                {
                    StopCoroutine(enemyTurnCoroutine);
                    Debug.Log("Stopped enemy turn coroutine on battle end.");
                    enemyTurnCoroutine = null;
                }
            }
            else if (bs == BattleState.Victory)
            {
                Debug.Log("Player won the battle!");
                BasicModal modal = UI_Util.CreateModal("Battle Ended");
                modal.AddContentText("Congratulations on your victory!");

                modal.AddFooterButton("Return", () =>
                {
                    SceneManager.LoadScene("LobbyScene");
                });


            }
            else if (bs == BattleState.Defeat)
            {
                Debug.Log("Player lost the battle!");
                var modal = UI_Util.CreateModal("Battle Ended");
                modal.AddContentText("You have been defeated.");
                modal.AddFooterButton("Return", () =>
                {
                    SceneManager.LoadScene("MainMenu");
                });
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }


        public void EnemyTurn()
        {

            if (turnInProgress)
            {
                Debug.Log("Turn already in progress, cannot start another.");
                return;
            }
            turnInProgress = true;

            if (playerTurnCoroutine != null)
            {
                Debug.Log("Stopping player turn coroutine.");
                //StopCoroutine(playerTurnCoroutine);
                playerTurnCoroutine = null;
            }
            if (bs == BattleState.Victory)
            {
                Debug.Log("Battle already ended, cannot start enemy turn.");
                return;
            }
            else if (bs == BattleState.Defeat)
            {
                Debug.Log("Battle already ended, cannot start enemy turn.");
                return;
            }

            bs = BattleState.EnemyTurn;
            ClearHandUI();

            CreateEnemySprites();


            Debug.Log("Enemy is taking its turn...");

            Turn.text = "Enemy Turn";

            // wait for server for hand and active player

            disablePointer();

            DiscardLabel.text = "Discard Pile: " + BattleContext.Instance.EnemyCardPile.Discard.Count.ToString();
            DrawLabel.text = "Draw Pile: " + BattleContext.Instance.EnemyCardPile.Draw.Count.ToString();
            currentEnergy = totalEnergy;
            EnergyLabel.text = "Energy: " + currentEnergy.ToString() + "/" + totalEnergy.ToString();

            foreach (var card in BattleContext.Instance.EnemyCardPile.Hand)
            {
                if (card is AttackCard ac)
                {
                    Debug.Log("Creating card in hand: " + ac.title);
                    UI_Util.CreateCardUI(ac, Resources.Load<GameObject>("Prefabs/CardUI"), GameObject.Find("Canvas/CardSection").transform);

                }


            }

            // wait for server for start
            // then start timer 


            // Simulate enemy turn duration
            enemyTurnCoroutine = StartCoroutine(TurnTimer(20, () =>
            {
                Debug.Log("Enemy turn ended");

                // Check win condition here


                turnInProgress = false;
                PlayerTurn();
            }));

            // wait for server end signal
        }


        private IEnumerator TurnTimer(int seconds, Action callback)
        {
            float time = seconds;

            while (time > 0)
            {
                yield return new WaitForSeconds(1);
                time -= 1;
                // Update UI timer here...
                Timer.text = time.ToString();
                Debug.Log("Timer: " + time);


            }

            callback?.Invoke();
        }

        public bool isActivePlayer()
        {
            return BattleContext.Instance.ActivePlayer == BattleContext.Instance.PlayerName;
        }
        public void OnMouseDown()
        {
            Debug.Log($"{gameObject.name} clicked!");
            // You can call other game logic here
        }


        public static GameObject CreateCharacterSprites(Character c)
        {
            GameObject prefab = null;

            // Find the Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("No Canvas found in scene!");
                return null;
            }

            // Choose prefab based on character element
            if (c.element.ToString() == "Fire")
            {
                prefab = Resources.Load<GameObject>("Prefabs/KnightChar"); // path inside Resources
            }
            // Add other elements here

            if (prefab == null)
            {
                Debug.LogError("Prefab not found for element: " + c.element);
                return null;
            }

            // Instantiate prefab inside Canvas
            GameObject charObj = Instantiate(prefab, canvas.transform);

            return charObj;
        }



        // {
        //     GameObject prefab = null;

        //     // Find the Canvas
        //     GameObject canvas = GameObject.Find("Canvas");
        //     if (canvas == null)
        //     {
        //         Debug.LogError("No Canvas found in scene!");
        //         return null;
        //     }

        //     // Choose prefab based on character element
        //     if (c.element.ToString() == "Fire")
        //     {
        //         prefab = Resources.Load<GameObject>("Prefabs/KnightChar"); // path inside Resources
        //     }
        //     // Add other elements here

        //     if (prefab == null)
        //     {
        //         Debug.LogError("Prefab not found for element: " + c.element);
        //         return null;
        //     }

        //     // Instantiate prefab inside Canvas
        //     GameObject charObj = Instantiate(prefab, canvas.transform);

        //     return charObj;

        //     // RuntimeAnimatorController knightController = Resources.Load<RuntimeAnimatorController>("Anims/Knight_Idle_Controller");
        //     // AnimationClip knightClip = Resources.Load<AnimationClip>("Anims/KnightAnim");

        //     // if (knightController == null)
        //     //     Debug.LogError("Could not find Knight_Animation_Controller in Resources/Animations!");
        //     // if (knightClip == null)
        //     //     Debug.LogError("Could not find KnightAnim in Resources/Animations!");


        //     // Vector3 PlayerSpawn_Char1 = GameObject.Find("Player_Spawn").transform.position;

        //     // GameObject CharacterGameObject = UI_Util.CreateSpriteObject(PlayerSpawn_Char1, PlayerSprite, knightClip, knightController, false);

        //     // BoxCollider2D collider = CharacterGameObject.AddComponent<BoxCollider2D>();

        //     // // Optionally adjust collider size to match the sprite
        //     // SpriteRenderer sr = CharacterGameObject.GetComponent<SpriteRenderer>();
        //     // if (sr != null && sr.sprite != null)
        //     // {
        //     //     collider.size = sr.sprite.bounds.size;
        //     // }

        //     // // (Optional) Add your click detection script
        //     // CharacterGameObject.AddComponent<CharacterClick>();


        // }

    }


}
