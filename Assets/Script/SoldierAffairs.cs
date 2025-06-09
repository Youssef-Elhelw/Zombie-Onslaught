using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class SoldierAffairs : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarScript healthbarS;
    public Spawner spawner;
    public Animator animator;
    public bool isDead = false;
    public bool belowHalf = false;
    private bool justOnce = true;
    public SplineAnimate splineAnimate;
    public bool nowMoveTowardsTheTarget = false;
    public float runSpeed = 0.05f;
    private Rigidbody rigidbody;
    public bool rageDone = false;
    private int randomBossSound;
    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();

        rigidbody = GetComponent<Rigidbody>();
        splineAnimate = GetComponent<SplineAnimate>();
        currentHealth = maxHealth;
        healthbarS = GetComponentInChildren<HealthBarScript>();
        healthbarS.UpdateHealthBar(maxHealth, currentHealth);
    }

    void Update()
    {
        if (spawner.rounds < spawner.bossRound)
        {
            animator.SetInteger("Speed", (int)spawner.pathSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (spawner.rounds == spawner.bossRound && (other.CompareTag("obstcale") || other.CompareTag("Turret")))
        {
            Destroy(other.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (nowMoveTowardsTheTarget && !isDead)
        {
            Vector3 targetPosition = new Vector3(45.6199951f, 0.779999971f, -22.0599995f);
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            Vector3 direction = (targetPosition - transform.position).normalized;
            rigidbody.AddForce(direction * runSpeed, ForceMode.VelocityChange);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthbarS.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= maxHealth / 2 && justOnce && spawner.rounds == spawner.bossRound)
        {
            splineAnimate.Pause();
            animator.SetBool("BelowHalf", true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.zombieSound, 0.8f);
            StartCoroutine(RageThenRun());
            justOnce = false;
        }
    }

    IEnumerator RageThenRun()
    {
        while (!rageDone)
        {
            yield return null;
        }
        Debug.Log("run now");
        animator.SetBool("Run", true);
        splineAnimate.enabled = false;
        nowMoveTowardsTheTarget = true;
        yield return null;
    }

    public void WaitRage()
    {
        rageDone = true;
    }
    public void DestroyEnemy()
    {
        if (spawner.rounds == spawner.bossRound)
        {
            spawner.won = true;
        }
        Destroy(gameObject);
    }
}
