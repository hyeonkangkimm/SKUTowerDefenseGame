using System;
using System.Collections;
using UnityEngine;
public class CharacterControllerH : Controller
{
    public Renderer Renderer;
    protected Color originalColor;
    [NonSerialized] public Character character;
    public Collider DetectCollider;
    public Renderer DetectRenderer;
    [NonSerialized] public HealthSystem healthSystem;
    //public List<Skill> SkillList;
    public event Action OnDeath;
    public event Action<int,bool> OnDamage;
    public event Action OnAttack;
    public event Action<int> OnHeal;
    //public event Action OnAttackSpeedChange;

    public bool isHit;
    public bool isHeal;
    public bool isDead;
    public bool isChanneling;

[NonSerialized] public bool isEnemy;

    protected override void Awake() 
    {
        base.Awake();
        healthSystem = GetComponent<HealthSystem>();
        DetectRenderer.enabled = false;
        character = GetComponent<Character>();
        isDead = false;
        Renderer = GetComponentInChildren<Renderer>();
        originalColor = Renderer.material.color;
    }
    protected override void Update() 
    {
        base.Update();
        //character.StateMachine.Update(); 
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
        //animator.enabled = false;
    }
    public void OnEnable()
    {
        //animator.enabled = true;
        //isDead = false;
        //isChanneling = false;
        //spriteRenderer.color = originalColor;
    }
    public void OnDisable()
    {
        currentHurtCoroutine = null;
    }
    #endregion
    #region Action CallBack
    public void CallDeath()
    {
        OnDeath?.Invoke();
        CharacterManager.Instance.Unregister(character);
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
    [ContextMenu("Walk")]
    public void CMFight()
    {
         character.StateMachine.ChangeState(character.StateMachine.Pursuit);
    }
    [ContextMenu("Attack")]
    public void CMAttack()
    {
        character.StateMachine.ChangeState(character.StateMachine.NormalAttack);
    }
    public void ChooseAttackType()
    {
        

    }
    public void AttackEvent()
    {

    }
    private void OnMouseEnter()
    {
        DetectRenderer.enabled = true;
    }
    private void OnMouseExit()
    {
        DetectRenderer.enabled = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("몬스터 감지됨: " + other.name);

            this.character.StateMachine.ChangeState(character.StateMachine.NormalAttack);
        }
    }


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

