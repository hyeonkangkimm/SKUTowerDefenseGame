using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
public class CharacterControllerH : Controller
{
    private Renderer Renderer;
    protected Color originalColor;
    [NonSerialized] public Character character;
    
    [NonSerialized] public HealthSystem healthSystem;
    //public List<Skill> SkillList;
    public event Action OnDeath;
    public event Action<int,bool> OnDamage;
    public event Action OnAttack;
    public event Action<int> OnHeal;
    //public event Action OnAttackSpeedChange;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    public bool isHit;
    public bool isHeal;
    public bool isDead;
    public bool isChanneling;

[NonSerialized] public bool isEnemy;

    protected override void Awake() 
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
        character = GetComponent<Character>();
        isDead = false;
        Renderer = GetComponentInChildren<Renderer>();
        originalColor = Renderer.material.color;
    }
    protected override void Update() 
    {
        base.Update();
        character.StateMachine.Update(); 
    }
    #region Hurt and Death Coroutine
    public override IEnumerator PlayHurtAnimationAndIdleCoroutine()
    {
        #region 경직on
        //if (currentHurtCoroutine != null) StopCoroutine(currentHurtCoroutine);
        //isHit = true;
        //character.Animator.SetTrigger(character.DataAnim.HurtParameterHash);
        //yield return hurtAnimLength;
        //isHit = false;
        #endregion
        #region 경직off
        Renderer.material.color = Color.red;
        yield return hurtAnimLength;
        Renderer.material.color = originalColor;
        #endregion
    }
    public override IEnumerator PlayDeathAnimationAndIdleCoroutine()
    {
        character.StateMachine.ChangeState(character.StateMachine.Death);
        yield return deadAnimLength;
        //if (character.EntityType == EEntityType.MONSTER) 
        gameObject.SetActive(false);
        animator.enabled = false;
    }
    public void OnEnable()
    {
        animator.enabled = true;
        isDead = false;
        //isChanneling = false;
        //spriteRenderer.color = originalColor;
    }
    public void OnDisable()
    {
        currentHurtCoroutine = null;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                MonsterAI ai = enemy.GetComponent<MonsterAI>();
                if (ai != null)
                {
                    ai.OnDeath -= OnEnemyDeath;  
                }
            }
        }
        enemiesInRange.Clear(); // 리스트 초기화

    }
    #endregion
    #region Action CallBack
    public void CallDeath()
    {
        OnDeath?.Invoke();
        isDead = true;
        StartCoroutine(PlayDeathAnimationAndIdleCoroutine());
    }
    public void CallOnDamage(int Damage,bool critic)
    {
        if (!isDead)
        {
        DamagedAnim();
        OnDamage?.Invoke(ApplyDef(Damage),critic);
        }
    }
    public void CallAttack()
    {

        if (!isAttacking & !isDead)
        {
            //ChooseAttackType();
            OnAttack?.Invoke();

        }
    }
    public void CallHeal(int amount)
    {
        OnHeal?.Invoke(amount);
    }
    //public void CallAttackSpeedChange(float speed)
    //{
    //    OnAttackSpeedChange?.Invoke();
    //}
    #endregion
    
    public void ChooseAttackType()
    {
        

    }
    public void AttackEvent()
    {

    }

    #region 전투상태관리
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterAI ai = other.GetComponent<MonsterAI>();
            ai.OnDeath -= OnEnemyDeath;
            ai.OnDeath += OnEnemyDeath;
            enemiesInRange.Add(other.gameObject);
            this.character.StateMachine.ChangeState(character.StateMachine.NormalAttack);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
           MonsterAI ai = other.GetComponent<MonsterAI>();
           ai.OnDeath -= OnEnemyDeath;
           enemiesInRange.Remove(other.gameObject);
           if (enemiesInRange.Count == 0)
           {
               this.character.StateMachine.ChangeState(character.StateMachine.Idle);
           }
        }
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
            if (enemiesInRange.Count == 0)
            {
            this.character.StateMachine.ChangeState(character.StateMachine.Idle);
            }
        }
       
    }
    #endregion
    #region 공격전 데미지계산
    /// <summary>
    /// 플레이어의 현재 공격력을 받아서, AtkMultiplier를 계산
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public (int Damage, bool IsCritical) CalculateDamage(int baseDamage)
    {
        // 공격력 증가 계수를 적용한 데미지 계산
        int enhancedDamage = baseDamage;

        // 크리티컬 데미지 계산
        float CriticalChance = character.StatHandler.curStat.GetCurCriticalInfo().CriRate;
        float CriticalMutiplier = character.StatHandler.curStat.GetCurCriticalInfo().CriDamage;
        bool isCritical = UnityEngine.Random.value < CriticalChance;
        float CriticDamage = isCritical ? (enhancedDamage * CriticalMutiplier) : enhancedDamage;
        int finalDamage = (int)(CriticDamage * character.StatHandler.curStat.GetDamageMuliplier());
        return (finalDamage, isCritical);
    }
    #endregion
    #region 공격후 피격때 계산
    protected int ApplyDef(int value)  
    {
        
        int EffectiveDamage = (int)(value * (100 / (100 + character.StatHandler.curStat.GetCurDefense())));

        return EffectiveDamage;
    }
    #endregion
}

