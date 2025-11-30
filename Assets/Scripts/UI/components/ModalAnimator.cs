using UnityEngine;
using DG.Tweening;

public class ModalAnimator : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float animTime = 0.45f;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        // Move modal off-screen (bottom)
        float screenHeight = Screen.height;
        rect.anchoredPosition = new Vector2(0, -screenHeight);

        Debug.Log($"[ModalAnimator] Awake/Start: Initial anchoredPosition = {rect.anchoredPosition}");
    }

    public void Show()
    {
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = Vector2.zero;

        Debug.Log($"[ModalAnimator] Show: Moving from {startPos} to {endPos}");

        rect.DOAnchorPos(endPos, animTime)
            .SetEase(Ease.OutCubic);
    }

    public void Hide()
    {
        Vector2 startPos = rect.anchoredPosition;
        float screenHeight = Screen.height;
        Vector2 endPos = new Vector2(0, -screenHeight);

        Debug.Log($"[ModalAnimator] Hide: Moving from {startPos} to {endPos}");

        rect.DOAnchorPos(endPos, animTime)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Debug.Log("[ModalAnimator] Hide complete: Destroying modal");
                Destroy(gameObject);
            });
    }
}
