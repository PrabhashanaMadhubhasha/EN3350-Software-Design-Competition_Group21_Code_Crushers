using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;
    float attackIndex = 0;

    public float caloriesSpentAttackingToEnemy = 30;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            Debug.Log("Nulllll");
            return;
        }

        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange) // Enemy attack to the player
            {
                animator.SetFloat("attackIndex", attackIndex);
                animator.SetTrigger("attack");
                timePassed = 0;

                attackIndex = UnityEngine.Random.Range(0, 4);
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange) // Set the destination with player
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;
        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            transform.LookAt(player.transform);
        }
        
    }

    void Die() // Enemy die and disappear and show its ragdoll
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyDeathSound);
        Destroy(this.gameObject);
    }

    // Enemy tke damage
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        PlayerState.Instance.currentCalories -= caloriesSpentAttackingToEnemy;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            MissionSystem.Instance.ExtractEnemyIndex(gameObject.name);
            Die();
        }
    }
    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    // Appear Blood effects when hitting
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

}