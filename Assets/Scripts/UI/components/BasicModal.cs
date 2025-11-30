using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Imperius.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class BasicModal : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI Heading;
    public Transform Content;
    public Transform Footer;
    private CanvasGroup canvasGroup;

    private int maxSelection = 3;
    private int currentSelection = 0;
    private Action<List<Character>> onSelectionComplete;
    private List<Character> selectedCharacters = new();
    private Deck selectedDeck = new();

    private TaskCompletionSource<List<Character>> tcs;
    private TaskCompletionSource<Deck> tcsDeck;

    public int imageItems;
    void Awake()
    {

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            Debug.LogError("BasicModal is missing a CanvasGroup component!");


        if (Heading == null)
        {
            Heading = GetComponentInChildren<TextMeshProUGUI>(true);

            if (Heading == null)
            {
                Debug.LogError("Heading TextMeshProUGUI NOT FOUND in BasicModal prefab!");
            }
        }

        // ---- SAFE CONTENT LOOKUP ----
        if (Content == null)
        {
            Content = transform.Find("Cover/Border/Body/Content");
            if (Content == null)
                Debug.LogError("Content transform NOT FOUND at path: Cover/Border/Body/Content");
        }

        // ---- SAFE FOOTER LOOKUP ----
        if (Footer == null)
        {
            Footer = transform.Find("Cover/Border/Body/Footer");
            if (Footer == null)
                Debug.LogError("Footer transform NOT FOUND at path: Cover/Border/Body/Footer");
        }

        imageItems = 0;

        Hide(false);
    }


    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    public Task<Deck> DeckSelectMode(List<Deck> Decks)
    {
        tcsDeck = new TaskCompletionSource<Deck>();

        selectedDeck = null;

        foreach (var c in Decks)
        {
            AddContentText(c);
        }

        AddFooterButton("Cancel",()=>{
            tcsDeck.TrySetResult(null);
            Hide(true);} );

        return tcsDeck.Task; // Return the task to your button code
    
    
    }
    public void AddContentText(Deck d)
    {

        GameObject item = Instantiate(GlobalContext.Instance.ContentTextPrefab, Content);  

        if (item != null)
        {
            Button btn = item.GetComponent<Button>();

             TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
                text.text = d.Name;
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {

                    selectedDeck= new Deck(d);

                        tcsDeck.TrySetResult(selectedDeck);
                        Hide(true);

                });
            }
        }

    }

     public void AddContentText(string s)
    {

        GameObject item = Instantiate(GlobalContext.Instance.ContentTextPrefab, Content);  

        if (item != null)
        {
        
             TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
                text.text = s;
        }

    }
    public void Hide(bool destroy = true)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (destroy)
            Destroy(gameObject);
    }

    public void SetHeading(string text)
    {
        Heading.text = text;
    }

    // public GameObject AddContent(GameObject prefab)
    // {
    //     return Instantiate(prefab, Content);
    // }

    public void AddFooterButton(string buttonText, Action callback)
    {
        // Instantiate the button under Footer
        var btnObj = Instantiate(GlobalContext.Instance.BasicButtonPrefab, Footer);

        // Set the button's onClick event
        var btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => callback?.Invoke());

        // Set the button's text
        var tmp = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = buttonText;

    }

    public void AddImageItem(GameObject prefab, string label, Sprite icon, Action onClick)
    {
        GameObject item = Instantiate(prefab, Content, false);

        // Assign image + label safely
        Image img = item.GetComponentInChildren<Image>(includeInactive: true);
        TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);

        this.imageItems++;

        if (img != null)
            img.sprite = icon;

        if (text != null)
            text.text = label;

        // Add click event
        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => onClick.Invoke());
        }


    }

    public Task<List<Character>> CharacterSelectModeAsync(string header, int maxCount, List<Character> availableCharacters)
    {
        tcs = new TaskCompletionSource<List<Character>>();

        maxSelection = maxCount;
        currentSelection = 0;
        selectedCharacters.Clear();

        UpdateHeader(header);
        GameObject ImageItemPrefab = Resources.Load<GameObject>("Prefabs/ImageItem");
        foreach (var c in availableCharacters)
        {
            AddImageItem(ImageItemPrefab, c.element.ToString(), c.Portrait,
                () => OnCharacterClicked(c, header));
        }

        return tcs.Task; // Return the task to your button code
    }
    
    private void UpdateHeader(string baseHeader)
    {
        Heading.text = $"{baseHeader} ({currentSelection}/{maxSelection} chosen)";
    }

    private void OnCharacterClicked(Character c, string baseHeader)
    {
        if (selectedCharacters.Contains(c))
            return; // Already selected, should not happen since prefab will be destroyed

        selectedCharacters.Add(c);
        currentSelection++;

        UpdateHeader(baseHeader);

        this.imageItems--;

        // Remove entire ImageItem prefab
        GameObject clickedItem = EventSystem.current.currentSelectedGameObject;
        Destroy(clickedItem);

        // Check if selection complete
        if (currentSelection >= maxSelection || this.imageItems == 0)
        {
            tcs.TrySetResult(selectedCharacters);
            Hide(true);
        }
    }
}



