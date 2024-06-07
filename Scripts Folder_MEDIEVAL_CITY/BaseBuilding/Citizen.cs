using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
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

    float speed;

    public float caloriesSpentAttackingToEnemy = 30;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()    //NOTE: this is not in the game thi code is for citizen following at the player 
    {
        if (0.08f * agent.velocity.magnitude * Vector3.Distance(player.transform.position, transform.position) > 1)
        {
            speed = 1;
        }
        else
        {
            speed = 0.08f * agent.velocity.magnitude * Vector3.Distance(player.transform.position, transform.position);
        }
        animator.SetFloat("speed", speed);
        //Debug.Log("Speeeeeeeeeed:  " + agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            Debug.Log("Nulllll");
            return;
        }

        if (timePassed >= attackCD)
        {
            //Debug.Log("distnce:    "+Vector3.Distance(player.transform.position, transform.position));
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.SetFloat("talkIndex", attackIndex);
                animator.SetTrigger("talk");
                timePassed = 0;
                if (attackIndex >= 1.0f)
                {
                    attackIndex = 0;
                }
                else
                {
                    attackIndex++;
                }
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
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
}