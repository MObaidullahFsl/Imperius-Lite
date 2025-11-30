using UnityEngine;
using TMPro;
using DG.Tweening;

public class Alert : MonoBehaviour
{
    public TMP_Text messageText;        // Assign in prefab (or find in Awake)
    private RectTransform rect;
    public float animTime = 0.15f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogError("[Alert] No RectTransform found!");
            return;
        }

        // Move off-screen (bottom)
        rect.anchoredPosition = new Vector2(0, -Screen.height);

        // Optional: Find TMP_Text if not assigned
        if (messageText == null)
            messageText = GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Set custom text for this alert
    /// </summary>
    public void SetText(string text)
    {
        if (messageText != null)
            messageText.text = text;
        else
            Debug.LogWarning("[Alert] TMP_Text not assigned!");
    }

    /// <summary>
    /// Show the alert with slide-up animation
    /// </summary>
    public void Show()
    {
        if (rect == null) return;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new(0, -220);

        Debug.Log($"[Alert] Show: Moving from {startPos} to {endPos}");
        rect.DOAnchorPos(endPos, animTime).SetEase(Ease.OutCubic);

        // Auto-hide after 2 seconds
        DOVirtual.DelayedCall(2f, () => Hide());
    }

    /// <summary>
    /// Hide the alert with slide-down animation
    /// </summary>
    public void Hide()
    {
        if (rect == null) return;

        float screenHeight = Screen.height;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(0, -screenHeight);

        Debug.Log($"[Alert] Hide: Moving from {startPos} to {endPos}");
        rect.DOAnchorPos(endPos, animTime).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Debug.Log("[Alert] Hide complete: Destroying alert");
                Destroy(gameObject);
            });
    }
}
