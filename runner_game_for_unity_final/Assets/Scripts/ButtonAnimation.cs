using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween kutubxonasini ulash

public class ButtonAnimation : MonoBehaviour
{
    private RectTransform rectTransform; // Buttonning RectTransform komponenti

    void Start()
    {
        // RectTransform komponentini olish
        rectTransform = GetComponent<RectTransform>();

        // Button komponentiga bosish funksiyasini ulash
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // Kichraytirish animatsiyasi
        rectTransform.DOScale(0.8f, 0.2f) // Kichraytirish (80%)
            .SetEase(Ease.OutQuad) // Silliq animatsiya
            .OnComplete(() =>
            {
                // Kattalashtirish animatsiyasi
                rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
            });
    }
}