using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking
}
[System.Serializable]
public struct DropItem
{
    public int amount;
    public resourseType type;
    public DropItem(resourseType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}

public class MonsterAI : MonoBehaviour, IDamageable
{

    [Header("Stats")]
    
    [Tooltip("StageManager에서 관리할 변수들 구조체, 소환하면서 곱셈할거임")]

    public int Health;
    public int BaseHp;
    public int Damage;
    public int BaseDamage;
    public Sprite Icon;


    public DropItem[] DropItems;
    public bool Die;
    public event Action<GameObject> OnDeath;
    public float walkSpeed;

    [Header("Movement")]
    public Transform targetDestination;
    public bool reachedFinalDestination = false;

    [Header("AI")]
    private NavMeshAgent agent;
    
    public int detectionRange;//맵한칸 거리 기준
    [SerializeField]private AIState aiState;

    [Header("Combat")]
    
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    public float fieldOfView = 120f;

    [Header("Target")]
    public float TargetDistance;
    [SerializeField] private GameObject NearTarget;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;
    LayerMask enemyLayer;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
    }

    void Start()
    {
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        SetState(AIState.Wandering);
        GoToDestination();
        Die = false;
        enemyLayer = 1 << 6;
        
    }
    private void OnEnable()
    {
        Die = false;
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        //NearTarget이 죽었을 때 타겟 해제, if문 안에서 앞 조건식 먼저 계산한후 false면 if문을 나가기 때문에 뒤에 NullReferenceException오류가 안난다
        if (NearTarget != null &&!NearTarget.activeInHierarchy)
            NearTarget = null;
        if(null!=NearTarget)
            TargetDistance = (NearTarget.transform.position-this.transform.position).magnitude;
        
       Vector3 origin = transform.position + Vector3.up ;
       Vector3 direction = transform.forward;
       Debug.DrawRay(origin, direction * detectionRange, Color.red);
       RaycastHit[] hits = Physics.RaycastAll(origin, direction, detectionRange,enemyLayer);
       foreach (RaycastHit hit in hits)
       {
           NearTarget = hit.collider.gameObject;
           break;
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
        if (TargetDistance < detectionRange&&NearTarget!=null)
        {
            SetState(AIState.Attacking);
            return;
        }

        // 목적지 도착 판정
        if (!agent.pathPending&&agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Reached");
            reachedFinalDestination = true;
            SetState(AIState.Idle);
            gameObject.SetActive(false);
            //벽 체력깍기
        }
    }

    void AttackingUpdate()
    {  if (Die)
            return;
        //거리가 안에 있고 시야에 있으면 공격
        if (TargetDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;

            if (Time.time - lastAttackTime > attackRate)//attackRate*clipSpeed
            {
                lastAttackTime = Time.time;
                //애니메이션에서 Event함수로 조절함
                //var damageTarget = NearTarget.GetComponent<IDamageable>();
                //if (damageTarget != null)
                //{
                //    damageTarget.TakeDamage(damage);
                //}


                //animator.speed = 1/attackRate;
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
    public void TakeDamage(int value, bool critic = false)
    {
        TakePhysicalDamage(value);
    }
    public void TakePhysicalDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDie();
        }
        if(!Die)
             StartCoroutine(DamageFlash());
    }

    void OnDie()
    {
        Die = true;
        for (int i = 0; i <DropItems.Length; i++)
        {
            ResourceManager.Instance.GainResource(DropItems[i].type, DropItems[i].amount);
        }
        OnDeath?.Invoke(this.gameObject);
        gameObject.SetActive(false);
    }

    IEnumerator DamageFlash()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = Color.white;
        }
    }
    /// <summary>
    /// 공격 Anim끝나는 지점
    /// </summary>
    public void AttackEnd()
    {

    }
    /// <summary>
    /// 공격Anim시작지점
    /// </summary>
    public void AttackBegin()
    {
        if(NearTarget != null)
        {
        IDamageable character = NearTarget.GetComponent<IDamageable>();
        character.TakeDamage(Damage);
        } 
       
    }

   
}
