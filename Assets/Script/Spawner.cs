using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Spawner : MonoBehaviour
{
    public SplineContainer path;
    public float pathSpeed = 7;
    public float bossSpeed = 3;
    public int nOfEnemies = 2;
    public GameObject Soldier;
    public GameObject zombieBoss;
    public float secondsToWait = 3;
    public float secondsBetweenEnemies = 0.5f;
    public TextMeshProUGUI numberOfRounds;
    public TextMeshProUGUI UntillNextRound;
    public TextMeshProUGUI NofZombiesComming;
    public GameObject clickSpaceToStart;
    public int rounds = 1;
    int battalion = 0;
    int nOfbattalions = 2;
    public int secondsBetweenRounds = 15;
    public GameObject Wait;
    public GameObject gameObjectZombieText;
    public TextMeshProUGUI ZombieAreComningText;
    public GameObject turretA;
    public GameObject turretB;
    GameObject spawnedBoss;
    bool toNextRound = false;
    public Detection dtct;
    public Money money;
    public bool won = false;
    public int bossRound = 11;

    void Start()
    {
        if (rounds == 1)
        {
            SoldierAffairs soldierAffairs = Soldier.GetComponent<SoldierAffairs>();
            soldierAffairs.maxHealth = 70;
        }
        money = FindAnyObjectByType<Money>();
        dtct = FindAnyObjectByType<Detection>();
        clickSpaceToStart.SetActive(true);
    }
    bool first = false;
    bool onetime = true;

    bool checkSkip = false;
    bool skip = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !first)
        {
            int randomNextRoundSound = UnityEngine.Random.Range(0, 6);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.nextRoundSounds[randomNextRoundSound], 0.277f);
            first = true;
            clickSpaceToStart.SetActive(false);
            StartCoroutine(SpawningWaitingTime());
        }
        if (rounds == bossRound && onetime)
        {
            SpawnFinalRoundBoss();
            onetime = false;
        }
        if (!onetime && spawnedBoss != null)
        {
            spawnedBoss.GetComponent<SplineAnimate>().MaxSpeed = bossSpeed;
        }

        if (checkSkip)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                skip = true;
            }
        }


    }

    void Spawn()
    {
        GameObject spawnedSoldier = Instantiate(Soldier);
        SplineAnimate splineAnimate = spawnedSoldier.GetComponent<SplineAnimate>();
        if (dtct != null)
        {
            dtct.AddEnemy(spawnedSoldier);
        }

        // Debug.Log("soldier spwaned now there are " + dtct.enemies.Count);

        if (splineAnimate != null)
        {
            splineAnimate.Container = path;
            splineAnimate.MaxSpeed = pathSpeed;
            splineAnimate.Play();
        }
    }
    void SpawnFinalRoundBoss()
    {

        spawnedBoss = Instantiate(zombieBoss);
        SplineAnimate splineAnimate = spawnedBoss.GetComponent<SplineAnimate>();
        if (splineAnimate != null)
        {
            splineAnimate.Container = path;
            splineAnimate.MaxSpeed = bossSpeed;
            splineAnimate.Play();
        }
    }

    IEnumerator SpawningWaitingTime()
    {
        while (rounds < bossRound)
        {
            for (int i = 0; i < nOfEnemies; i++)
            {
                Spawn();
                yield return new WaitForSeconds(secondsBetweenEnemies);
            }

            battalion++;
            if (battalion == nOfbattalions)
            {
                battalion = 0;
                Wait.SetActive(true);
                toNextRound = true;
                checkSkip = true;
                gameObjectZombieText.SetActive(true);
                if (rounds < bossRound - 1)
                {
                    NofZombiesComming.text = ((nOfEnemies + 1) * (nOfbattalions + 1)).ToString();
                }
                else if (rounds == bossRound - 1)
                {
                    NofZombiesComming.text = 1.ToString();
                    ZombieAreComningText.fontSize += 4;
                    ZombieAreComningText.text = "Zombie is coming";
                }

                for (int t = (int)secondsBetweenRounds; t >= 0 && !skip; t--)
                {
                    if (t <= 10 && t > 5)
                    {
                        UntillNextRound.color = Color.yellow;
                    }
                    else if (t <= 5)
                    {
                        UntillNextRound.color = Color.red;
                    }
                    else
                    {
                        UntillNextRound.color = Color.green;
                    }
                    UntillNextRound.text = t.ToString();
                    yield return new WaitForSeconds(1);
                }

                checkSkip = false;
                skip = false;
                rounds++;
                money.currentMoney += 40;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.gainMoney, 0.277f);
                int randomNextRoundSound = UnityEngine.Random.Range(0, 6);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.nextRoundSounds[randomNextRoundSound], 0.277f);
                SoldierAffairs soldierAffairs = Soldier.GetComponent<SoldierAffairs>();
                soldierAffairs.maxHealth += 30;// i edited on the orignal prefab i should fix this


                // secondsBetweenRounds -= 1;
                // if (secondsBetweenRounds == 0)
                // {
                //     secondsBetweenRounds = 15;
                // }

                pathSpeed *= 1.2f;
                nOfEnemies += 1;
                nOfbattalions += 1;//edited from adding two to one
                numberOfRounds.text = rounds.ToString();
            }
            if (toNextRound)
            {
                gameObjectZombieText.SetActive(false);
                toNextRound = false;
                Wait.SetActive(false);
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(secondsToWait);
            }
        }
    }

}
