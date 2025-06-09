using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using TMPro;

public class BulletScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject moneyPrefab;
    public Transform bulletPoint;
    public Detection dtct;
    public TurretAffairs turretAffairsS;
    public int bulletDamage = 15;
    public float duration = 0f;
    public Money money;
    public int afterEachKill = 30;
    public Spawner spawner;
    public AudioManager audioManager;



    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        spawner = FindAnyObjectByType<Spawner>();
        dtct = FindAnyObjectByType<Detection>();
        if (dtct == null)
        {
            Debug.LogError("Detection component not found!");
            return;
        }

        if (money == null)
        {
            money = FindObjectOfType<Money>();
        }

        // Find the TurretAffairs component on this GameObject's parent
        turretAffairsS = GetComponentInParent<TurretAffairs>();
        if (turretAffairsS == null)
        {
            Debug.LogError("TAS component not found in parent!");
            return;
        }

        if (bulletPrefab == null || bulletPoint == null)
        {
            Debug.LogError("BulletPrefab or BulletPoint is not assigned!");
        }
    }
    public void ShootBullet()
    {
        if (bulletPrefab == null || bulletPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
        bullet.transform.rotation = Quaternion.Euler(90, 0, 0);
        bullet.transform.SetParent(null);
        int temp = turretAffairsS.minIndex;
        if (turretAffairsS.minIndex >= 0 && turretAffairsS.minIndex < dtct.enemies.Count)
        {
            if (dtct.enemies[turretAffairsS.minIndex] != null)
            {
                GameObject target = dtct.enemies[turretAffairsS.minIndex];
                SoldierAffairs enemySA = target.GetComponent<SoldierAffairs>();

                if (enemySA != null)
                {
                    enemySA.TakeDamage(bulletDamage);
                    Vector3 pos = target.transform.position;
                    pos.y += 1.22351f;

                    bullet.transform.DOMove(pos, duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (enemySA.currentHealth <= 0 && !enemySA.isDead)
                        {
                            enemySA.isDead = true;

                            SoldierAffairs zombie = target.GetComponent<SoldierAffairs>();
                            if (target != null)
                            {

                                zombie.animator.SetBool("Dead", true);
                                dtct.RemoveEnemy(target);
                                if (spawner.rounds < 11)
                                {
                                    AudioManager.Instance.PlaySFX(AudioManager.Instance.death, 0.4f);
                                }
                                else if (spawner.rounds == 11)
                                {
                                    AudioManager.Instance.PlaySFX(AudioManager.Instance.BossDies, 0.4f);
                                }

                                TextMeshProUGUI moneyText = target.GetComponentInChildren<TextMeshProUGUI>();
                                moneyText.text = "+" + afterEachKill.ToString() + "$";

                                money.currentMoney += afterEachKill;
                                AudioManager.Instance.PlaySFX(AudioManager.Instance.gainMoney, 0.5f);

                            }
                        }


                        Destroy(bullet);
                    });

                }
            }
        }
        else
        {
            Debug.LogError("Index out of range or no enemies available!");
        }

    }


}
