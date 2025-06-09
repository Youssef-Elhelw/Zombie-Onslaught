using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("there are " + enemies.Count);
        enemies.Clear();
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Soldier");

        foreach (GameObject enemy in foundEnemies)
        {
            AddEnemy(enemy);
        }
    }
}
