using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;   // <-- ADD THIS

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rect;
    private Vector3 normalScale;
    public float hoverScale = 1.08f;
    public float animSpeed = 0.15f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        normalScale = rect.localScale;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        // Scale UP on hover
        rect.DOScale(normalScale * hoverScale, animSpeed)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData e)
    {
        // Scale BACK when exit
        rect.DOScale(normalScale, animSpeed)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData e)
    {
        // Quick press animation
        rect.DOScale(normalScale * 0.95f, animSpeed * 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rect.DOScale(normalScale, animSpeed * 0.5f)
                    .SetEase(Ease.OutBounce);
            });
    }
}
