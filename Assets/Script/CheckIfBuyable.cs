using UnityEngine;
using TMPro;

public class CheckIfBuyable : MonoBehaviour
{
    public Money money;
    public int itemCost;
    public TextMeshProUGUI itemCostText;
    private Color lastColor;

    void Start()
    {
        if (money == null)
        {
            money = FindObjectOfType<Money>();
        }

        UpdateTextColor();
    }

    void Update()
    {
        if (money == null) return;

        Color newColor = (money.currentMoney >= itemCost) ? Color.green : Color.red;
        
        if (newColor != lastColor)
        {
            itemCostText.color = newColor;
            lastColor = newColor;
        }
    }

    private void UpdateTextColor()
    {
        if (money == null) return;
        lastColor = (money.currentMoney >= itemCost) ? Color.green : Color.red;
        itemCostText.color = lastColor;
    }
}
