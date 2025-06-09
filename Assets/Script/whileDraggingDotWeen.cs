using UnityEngine;
using DG.Tweening;

public class whileDraggingDotWeen : MonoBehaviour
{
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void OnEnable()
    {
        // Scale up when activated
        transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
    }
}
