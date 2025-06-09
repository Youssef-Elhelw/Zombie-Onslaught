using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetReached : MonoBehaviour
{
    public TextMeshProUGUI nonInfected;
    public int nonInfectedCounter = 6000;
    public TextMeshProUGUI infected;
    public int infectedCounter = 0;
    public int zombieMultiplier = 0;
    public Spawner spawner;

    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
        StartCoroutine(updateCityResidents());
    }

    void Update()
    {
        nonInfected.text = nonInfectedCounter.ToString();
        infected.text = infectedCounter.ToString();
    }

    IEnumerator updateCityResidents()
    {
        while (true)
        {
            if (nonInfectedCounter >= zombieMultiplier)
            {
                nonInfectedCounter -= zombieMultiplier;
                infectedCounter += zombieMultiplier;
            }
            else
            {
                infectedCounter = 500 + zombieMultiplier;
                nonInfectedCounter = 0;
                if (!spawner.won)
                {
                    SceneController sceneController = FindAnyObjectByType<SceneController>();
                    sceneController.NextScene("GameOver");
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soldier"))
        {
            if (spawner.rounds == 11)
            {
                zombieMultiplier += 5;
            }
            else
            {
                zombieMultiplier++;
            }

            Destroy(other.gameObject);
        }
    }
}
