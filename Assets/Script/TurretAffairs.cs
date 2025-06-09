using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Rendering;

public class TurretAffairs : MonoBehaviour
{
    public Light spotLight;
    private float viewAngle;
    private float viewDistance;
    public Detection dtct;
    public int minIndex = 0;
    public LayerMask viewMask;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public BulletScript bulletScript;
    public ShopItem shopItem;
    public AudioSource audioSource;
    public Transform transform0;
    public Quaternion originalR;

    public bool isDragging = false;


    void Start()
    {

        audioSource.Stop();
        audioSource.volume = 0.2f;
        dtct = FindAnyObjectByType<Detection>();
        if (dtct == null)
        {
            Debug.LogError("Detection component not found!");
            return;
        }

        if (bulletScript == null)
        {
            Debug.Log("BS component not found!");
            return;
        }

        if (spotLight != null)
        {
            viewAngle = spotLight.spotAngle;
            viewDistance = spotLight.range;
        }
    }

    void Update()
    {
        // Debug.Log(isDragging);
        if (isDragging) return; // Skip update logic if dragging

        originalR = transform0.rotation;
        if (dtct.enemies.Count == 0)
        {
            return;
        }

        detectNearestSoldier();

        if (CanSeePlayer())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            rotateToSoldier();
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalR, Time.deltaTime * 5f);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }




    void detectNearestSoldier()
    {
        minIndex = -1;
        float minDistance = float.MaxValue;
        for (int i = 0; i < dtct.enemies.Count; i++)
        {
            if (dtct.enemies[i] == null) continue;

            float distance = Vector3.Distance(dtct.enemies[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }
    }

    bool CanSeePlayer()
    {
        if (minIndex == -1 || dtct.enemies[minIndex] == null) return false;

        Vector3 dirToPlayer = (dtct.enemies[minIndex].transform.position - transform.position).normalized;
        float angleBetween = Vector3.Angle(transform.forward, dirToPlayer);

        if (Vector3.Distance(transform.position, dtct.enemies[minIndex].transform.position) < viewDistance && angleBetween < viewAngle / 2f)
        {
            if (!Physics.Linecast(transform.position, dtct.enemies[minIndex].transform.position, viewMask))
            {
                return true;
            }
        }
        return false;
    }

    void rotateToSoldier()
    {
        if (dtct.enemies[minIndex] == null) return;

        Quaternion targetRotation = Quaternion.LookRotation(dtct.enemies[minIndex].transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            bulletScript.ShootBullet();
        }
    }



    void OnDrawGizmos()
    {
        if (dtct == null || dtct.enemies.Count == 0) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);

        if (minIndex >= 0 && minIndex < dtct.enemies.Count && dtct.enemies[minIndex] != null && CanSeePlayer())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, dtct.enemies[minIndex].transform.position);
        }
    }
}
