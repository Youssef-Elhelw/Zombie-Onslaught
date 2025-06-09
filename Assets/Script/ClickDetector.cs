using System.Collections;
using TMPro;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    public Money money;
    public GameObject UpgradeAndSell;
    int numberOfUpgrades = 1;
    public Animator animator;
    public Animator animatorMoneyError;
    public Animator animatorMaximumUpdate;
    public int bulletDamageUpgrade = 4;
    public bool opened = false;
    public TextMeshProUGUI CanonLevel;

    void Start()
    {
        CanonLevel.text = numberOfUpgrades.ToString();
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#B4A40F", out newColor))
        {
            CanonLevel.color = newColor;
        }
        money = FindAnyObjectByType<Money>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    animator.SetTrigger("CanavasIn");
                    opened = true;
                }
                // else
                // {
                //     Debug.Log(hit.transform.gameObject + " i hitted this shit");
                // }
            }
            if (opened)
            {
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;
                if (Physics.Raycast(ray1, out hit1))
                {
                    if (hit1.transform.gameObject != gameObject)
                    {
                        animator.SetTrigger("CanavasOut");
                        opened = false;
                    }
                }
            }
        }

    }

    public CheckIfBuyable checkIfBuyable;
    public void UpgradeBullet()
    {
        checkIfBuyable = gameObject.GetComponentInChildren<CheckIfBuyable>();
        BulletScript bulletScript = gameObject.GetComponentInChildren<BulletScript>();
        if (money.currentMoney >= checkIfBuyable.itemCost)
        {
            if (numberOfUpgrades <= 3)
            {

                numberOfUpgrades++;
                CanonLevel.text = numberOfUpgrades.ToString();
                Color newColor;
                if (numberOfUpgrades == 2)
                {
                    if (ColorUtility.TryParseHtmlString("#CD5706", out newColor))
                    {
                        CanonLevel.color = newColor;
                    }
                }
                else if (numberOfUpgrades == 3)
                {
                    if (ColorUtility.TryParseHtmlString("#B4130F", out newColor))
                    {
                        CanonLevel.color = newColor;
                    }
                }
                else if (numberOfUpgrades == 4)
                {
                    if (ColorUtility.TryParseHtmlString("#B40F92", out newColor))
                    {
                        CanonLevel.color = newColor;
                    }
                }
                money.currentMoney -= checkIfBuyable.itemCost;
                bulletScript.bulletDamage += bulletDamageUpgrade;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.UpgradeSound);
            }
            else
            {
                animatorMaximumUpdate.SetTrigger("Max");
            }
        }
        else
        {
            animatorMoneyError.SetTrigger("NoMoney");
        }
        animator.SetTrigger("CanavasOut");
        StartCoroutine(waitAfewSeconds(0.23f));
        // UpgradeAndSell.SetActive(false);

    }

    IEnumerator waitAfewSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void SellTurret()
    {
        checkIfBuyable = gameObject.GetComponentInChildren<CheckIfBuyable>();
        money.currentMoney += checkIfBuyable.itemCost;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gainMoney);
        animator.SetTrigger("CanavasOut");
        StartCoroutine(waitAfewSeconds(0.23f));
        Destroy(gameObject);
    }

}
