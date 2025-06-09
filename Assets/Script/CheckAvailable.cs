using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAvailable : MonoBehaviour
{
    public Money money;
    static int turns = 0;
    void Start()
    {
        if (money == null)
        {
            money = FindObjectOfType<Money>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            turns++;
            Cost turretShopItem = other.GetComponent<Cost>();

            if (turretShopItem != null)
            {
                Destroy(other.gameObject);
                if (turns == 2)
                {
                    turns = 0;
                    return;
                }
                else
                {
                    money.currentMoney += turretShopItem.cost;
                }
            }
        }
    }
}
