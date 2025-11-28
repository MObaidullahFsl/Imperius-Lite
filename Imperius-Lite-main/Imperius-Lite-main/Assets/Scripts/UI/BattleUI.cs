using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Imperius.Logic;
using Imperius.UI;

namespace Imperius.UI
{

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

        void Start()
        {   
            battleBL = new BattleBL();
            initiateBattleUI();
        }

        public void initiateBattleUI()
        {
            Debug.Log("Battle UI initiated!");

            // You can add more initialization logic here as needed
            //battleBL.startBattle();
        }
        public void OnMouseDown()
        {
            Debug.Log($"{gameObject.name} clicked!");
            // You can call other game logic here
        }


        public static GameObject CreatePlayerSprites()
        {
            // a while loop to get all chars sprites

            Sprite PlayerSprite = Resources.Load<Sprite>("Sprites/Knight/Idle");

            RuntimeAnimatorController knightController = Resources.Load<RuntimeAnimatorController>("Anims/Knight_Idle_Controller");
            AnimationClip knightClip = Resources.Load<AnimationClip>("Anims/KnightAnim");

            if (knightController == null)
                Debug.LogError("Could not find Knight_Animation_Controller in Resources/Animations!");
            if (knightClip == null)
                Debug.LogError("Could not find KnightAnim in Resources/Animations!");


            Vector3 PlayerSpawn_Char1 = GameObject.Find("Player_Spawn").transform.position;

            GameObject CharacterGameObject = UI_Util.CreateSpriteObject(PlayerSpawn_Char1, PlayerSprite, knightClip, knightController, false);

            BoxCollider2D collider = CharacterGameObject.AddComponent<BoxCollider2D>();

            // Optionally adjust collider size to match the sprite
            SpriteRenderer sr = CharacterGameObject.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                collider.size = sr.sprite.bounds.size;
            }

            // (Optional) Add your click detection script
            CharacterGameObject.AddComponent<CharacterClick>();

            return CharacterGameObject;

        }
        
        public static GameObject CreateEnemySprites()
        {

            Sprite PlayerSprite = Resources.Load<Sprite>("Sprites/Samurai/Idle");

            RuntimeAnimatorController samuraiController = Resources.Load<RuntimeAnimatorController>("Anims/Samurai_Idle_Controller");
            AnimationClip samuraiClip = Resources.Load<AnimationClip>("Anims/SamuraiAnim");

            if (samuraiController == null)
                Debug.LogError("Could not find Knight_Animation_Controller in Resources/Animations!");
            if (samuraiClip == null)
                Debug.LogError("Could not find KnightAnim in Resources/Animations!");


            Vector3 PlayerSpawn_Char1 = GameObject.Find("Enemy_Spawn").transform.position;

            GameObject CharacterGameObject = UI_Util.CreateSpriteObject(PlayerSpawn_Char1, PlayerSprite, samuraiClip, samuraiController, true);

            CharacterGameObject.AddComponent<CharacterClick>();

            return CharacterGameObject;



        }


        // Update is called once per frame
        void Update()
        {

        }
    }


    public class CharacterClick : MonoBehaviour
    {
        void OnMouseDown()
        {

            // foreach (var card in ActivePlayer.Hand)
            // {
            //     if (card is AttackCard ac)
            //     {

            //         UI_Util.CreateCardUI(ac, cardPrefab, cardsPanel);
            //     }
            //     //else if (card is SkillCard sc)
            //     //UI_Util.CreateCardUI(sc, cardPrefab, cardsPanel);
            // }



            Debug.Log($"{gameObject.name} clicked!");
        }
    }
}
