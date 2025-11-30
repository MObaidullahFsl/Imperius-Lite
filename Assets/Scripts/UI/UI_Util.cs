using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Imperius.Logic;
using Imperius.UI;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Imperius.UI
{
    public class UI_Util : MonoBehaviour
    {

        public static GameObject CreateSpriteObject(Vector3 position, Sprite sprite, AnimationClip anim, RuntimeAnimatorController controller, bool flipped)
        {
            GameObject CharacterGameObj = new("CharacterObj");
            CharacterGameObj.transform.SetParent(GameObject.Find("Level").transform, false);
            CharacterGameObj.transform.position = position;
            CharacterGameObj.transform.localScale = new Vector3(130f, 130f, 1f);


            //  Add SpriteRenderer
            SpriteRenderer sr = CharacterGameObj.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 5;
            if (flipped)
            {
                sr.flipX = true;
            }

            //  Add Animator and assign controller
            Animator animator = CharacterGameObj.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.speed = 0.4f;

            BoxCollider2D collider = CharacterGameObj.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;

            // Add click handler
            CharacterGameObj.AddComponent<BattleUI>(); //check if further specification is possible 

            //  (Optional) immediately play the animation
            animator.Play(anim.name);

            return CharacterGameObj;
        }

        public static GameObject CreateCardUI(AttackCard card, GameObject cardPrefab, Transform cardsPanel)
        {
            // --- SAFETY CHECKS ---
            if (cardPrefab == null)
            {
                Debug.LogError("CreateCardUI FAILED: cardPrefab is NULL! Did Resources.Load(\"Prefabs/CardUI\") fail?");
                return null;
            }

            if (cardsPanel == null)
            {
                Debug.LogError("CreateCardUI FAILED: cardsPanel is NULL! Is GameObject.Find(\"Canvas/CardPanel\") correct?");
                return null;
            }

            GameObject cardObj = Instantiate(cardPrefab, cardsPanel);

            if (cardObj == null)
            {
                Debug.LogError("CreateCardUI FAILED: Instantiating cardPrefab returned NULL.");
                return null;
            }

            // Attach click handler
            CardClickHandler clickHandler = cardObj.AddComponent<CardClickHandler>();
            clickHandler.card = card;

            // Load sprite
            Sprite cardSprite = Resources.Load<Sprite>("Img/CardImgs/" + card.title);
            if (cardSprite == null)
            {
                Debug.LogWarning("Card sprite not found for " + card.title + ". Using default sprite.");
                cardSprite = Resources.Load<Sprite>("Img/CardImgs/DefaultImg");
            }

            // --- SAFE UI ASSIGNMENTS ---
            SafeSetTMP(cardObj, "CardBg/CardPanel/Header/Name", card.title);
            SafeSetTMP(cardObj, "CardBg/CardPanel/Header/Energy", card.cost.ToString());
            SafeSetTMP(cardObj, "CardBg/CardPanel/Header/Type", card.element.ToString());
            SafeSetTMP(cardObj, "CardBg/CardPanel/Body/Desc", card.description);
            SafeSetImage(cardObj, "CardBg/CardPanel/ImageHolder/CardImg", cardSprite);

            return cardObj;

        }

        private static void SafeSetTMP(GameObject root, string path, string value)
        {
            Transform t = root.transform.Find(path);
            if (t == null)
            {
                Debug.LogError($"CreateCardUI FAILED: Could not find TMP path: {path}");
                return;
            }

            TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
            if (tmp == null)
            {
                Debug.LogError($"CreateCardUI FAILED: TMP component missing at: {path}");
                return;
            }

            tmp.text = value;
        }

        private static void SafeSetImage(GameObject root, string path, Sprite sprite)
        {
            Transform t = root.transform.Find(path);
            if (t == null)
            {
                Debug.LogError($"CreateCardUI FAILED: Could not find Image path: {path}");
                return;
            }

            Image img = t.GetComponent<Image>();
            if (img == null)
            {
                Debug.LogError($"CreateCardUI FAILED: Image component missing at: {path}");
                return;
            }

            img.sprite = sprite;
        }


        public static void CreateCardUI(SkillCard card)
        {
            // same code just overloaded
        }

        public class CardClickHandler : MonoBehaviour, IPointerClickHandler
        {
            public Card card;
            public void OnPointerClick(PointerEventData eventData)
            {
                Debug.Log("Clicked card: " + card.title);




            }
        }

        public static BasicModal CreateModal(string heading)
        {
            if (GlobalContext.Instance.BasicModalPrefab == null)
            {
                Debug.LogError("❌ BasicModalPrefab is NULL in GlobalContext!");
            }
            var canvas = GameObject.Find("Canvas");
            var modalObj = Instantiate(GlobalContext.Instance.BasicModalPrefab, canvas.transform, false);
            RectTransform rt = modalObj.GetComponent<RectTransform>();
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            var modal = modalObj.GetComponent<BasicModal>();

            modal.SetHeading(heading);
            modal.Show();

            return modal;
        }


        // // Start is called once before the first execution of Update after the MonoBehaviour is created
        // void Start()
        // {

        // }

        // // Update is called once per frame
        // void Update()
        // {

        // }
    }
}