using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public Detection dtct;
    void Start()
    {
        dtct = FindAnyObjectByType<Detection>();
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Soldier"))
        {
            dtct.RemoveEnemy(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
