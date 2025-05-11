using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.AI;
public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}
public enum AIState
{
    Idle,
    Wandering,
    Attacking
}

public class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    //public ItemData[] dropOnDeath;

    [Header("Movement")]
    public Transform targetDestination;
    private bool reachedFinalDestination = false;

    [Header("AI")]
    private NavMeshAgent agent;
    
    public int detectDistance;//맵한칸 거리 기준
    private AIState aiState;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    public float fieldOfView = 120f;

    [Header("Target")]
    public float playerDistance;
    [SerializeField] private GameObject NearTarget;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        SetState(AIState.Wandering);
        GoToDestination();
    }

    void Update()
    {
        if (NearTarget == null)
        {
            Hero nearest = null;
            float minSqr = Mathf.Infinity;

            var nearbyHeroes = CharacterManager.Instance.GetHeroesNear(transform.position, detectDistance);

            foreach (var hero in nearbyHeroes)
            {
                float sqrDist = (hero.transform.position - transform.position).sqrMagnitude;
                if (sqrDist < detectDistance * detectDistance && sqrDist < minSqr)
                {
                    minSqr = sqrDist;
                    nearest = hero;
                }
            }
            if (nearest != null)
            {
                NearTarget = nearest.gameObject;   
            }
        }
        else
        {
            playerDistance = Vector3.Distance(transform.position, NearTarget.transform.position);
        }
        animator.SetBool("Moving", aiState != AIState.Idle);
        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void GoToDestination()
    {
        if (targetDestination != null)
        {
            agent.SetDestination(targetDestination.position);
        }
    }

    void PassiveUpdate()
    {
        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
            return;
        }

        // 목적지 도착 판정
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            reachedFinalDestination = true;
            SetState(AIState.Idle);
            GameObject.Destroy(gameObject);
        }
    }

    void AttackingUpdate()
    {
        //거리가 안에 있고 시야에 있으면 공격
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;

                var damageTarget = NearTarget.GetComponent<IDamageable>();
                if (damageTarget != null)
                {
                    damageTarget.TakePhysicalDamage(damage);
                }

                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            //에러주석
            //if (playerDistance < detectDistance)
            //{
            //    agent.isStopped = false;
            //    agent.SetDestination(NearTarget.transform.position);
            //}
            //else
            {
                SetState(AIState.Wandering);
                GoToDestination();
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        if (NearTarget == null)
            return false;
        Vector3 directionToPlayer = NearTarget.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        //Drop item
        //for (int i = 0; i < dropOnDeath.Length; i++)
        //{
        //    Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        //}
        //refactoring Destory--> Pooling
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = Color.white;
        }
    }
    public void AttackEnd()
    {

    }
    public void AttackBegin()
    {

    }
}
