using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class AnimationDotWeen : MonoBehaviour
{
    public RectTransform shopPanel;
    public float showPositionY = 188f;
    public float hidePositionY = -268f;
    public float animationDuration = 0.5f;
    public ShopItem shopItem1;
    public ShopItem shopItem2;

    private bool isShopOpen = false;

    void Start()
    {
        shopPanel.anchoredPosition = new Vector2(shopPanel.anchoredPosition.x, hidePositionY);

    }
    void Update()
    {
        // Debug.Log("shop1 dragging" + shopItem1.isDragging);
        // Debug.Log("shop2 dragging" + shopItem2.isDragging);
        // Debug.Log("isshop" + isShopOpen);
        if (isShopOpen && (shopItem1.isDragging || shopItem2.isDragging))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        float targetY = isShopOpen ? showPositionY : hidePositionY;

        shopPanel.DOAnchorPosY(targetY, animationDuration).SetEase(Ease.OutQuad);
    }
}
