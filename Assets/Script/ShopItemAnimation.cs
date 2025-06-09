using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween

public class ShopItemAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    private Image itemImage;

    void Start()
    {
        originalScale = transform.localScale; // Save original size
        itemImage = GetComponent<Image>(); // Get image component
    }

    public void AnimateOnClick()
    {
        // Scale Up & Down Effect
        transform.DOScale(originalScale * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);

        // Flash Effect (Optional)
        itemImage.DOColor(Color.yellow, 0.1f).SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => itemImage.color = Color.white);

        // Shake Effect (Optional)
        transform.DOShakePosition(0.5f, 5f, 10, 90);
    }
}
