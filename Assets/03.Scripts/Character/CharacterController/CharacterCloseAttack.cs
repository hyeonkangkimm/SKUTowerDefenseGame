using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.TextCore.Text;
public class CharacterCloseAttack : MonoBehaviour
{
    public WaitForSeconds AttackReady;
    public float detectionRange;
    public LayerMask enemyLayer;
    [SerializeField]protected CharacterControllerH characterController;
    public CharacterStateMachine stateMachine;
    //public Animator MyAnimator;
    public float timeSinceLastAttack;
    public float AttackReadyTime;
    public float AttackAnimationLength;
    //public AnimationClip clip;
    float attackRange => characterController.character.StatHandler.curStat.AttackRange;
    float CurAtk => characterController.character.StatHandler.curStat.Atk;
    protected float CurAs => characterController.character.StatHandler.curStat.AttackSpeed;
    protected float CurAsMul => characterController.character.StatHandler.curStat.AttackSpeedMultiplier;
    float AtkRange => characterController.character.StatHandler.curStat.AttackRange;

    public float CriticalChance=0f;
    private GameObject currentTarget;

    public virtual void  Awake()
    {
        characterController  = GetComponentInParent<CharacterControllerH>();
        timeSinceLastAttack = 0f;
        //AttackReadyTime = AttackReadyTime == 0f ? 1f : AttackReadyTime;
        //AttackReady = new WaitForSeconds(AttackReadyTime);
        enemyLayer = 1 << 7;

    }
    public virtual void Start()
    {
        stateMachine = characterController.character.StateMachine;
        //AttackAnimationLength = clip.length;
        detectionRange = attackRange;
    }
    public void FixedUpdate()
    {
        //크리티컬적용확인용
        
            if ((currentTarget == null || !currentTarget.activeSelf) && characterController.enemiesInRange.Count > 0)
            {
                currentTarget = characterController.enemiesInRange[0];
            }
            if (currentTarget != null)
            {
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                direction.y = 0; // 수평 회전만
                transform.rotation = Quaternion.LookRotation(direction);
                if ((currentTarget.transform.position - transform.position).magnitude > AtkRange)
                    currentTarget = characterController.enemiesInRange.Count > 0 ? characterController.enemiesInRange[0] : null;
            }
        
    }
    public void OnDisable()
    {
        //characterController.OnAttack -= onAttack;
        //characterController.OnAttackSpeedChange -= ChangeAttackMotionSpeed;

    }
    public void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //characterController.OnAttack += onAttack;
        //characterController.OnAttackSpeedChange += ChangeAttackMotionSpeed;
    }

    public void Update()
    {
        
        
    }
    private void onAttack()
    {
        //ChangeAttackMotionSpeed(); //AS에 따라 애니메이션 속도 변경
        if (characterController.character.Animator.GetBool(characterController.character.DataAnim.NormalAttackParameterHash))
        {
            timeSinceLastAttack = 0f;
            characterController.isAttacking = true;
            //StartCoroutine(CloseAttack());
           /*공격 동작(준비/공격/마무리)은 시작했는데,준비동작에서 데미지를 입히면 이상하니까 
            공격동작에서 데미지를 입히게끔 코루틴을 사용하였음*/
        }
        
    }
    public void ClosePunch()
    {
        //if (characterController.character.Target != null)
            //MoveMeleePos(characterController.character.Target.position, attackRange);
        
        if (!characterController.isDead)
        {
            Vector3 origin = transform.position + Vector3.up;
            Vector3 direction = transform.forward;
            Debug.DrawRay(origin, direction * detectionRange, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, detectionRange,enemyLayer);
            foreach(RaycastHit hit in hits)
            {
                IDamageable damagable = hit.collider.GetComponent<IDamageable>();
                damagable?.TakeDamage(characterController.character.StatHandler.curStat.GetCurAtk());
                Debug.Log("근접공격");
            }
            AudioManager.Instance.PlaySFX("PUNCH");
        }
    }
    //private void SetAttackMotionSpeed(float attackSpeed=1f)
    //{
    //    //MyAnimator.SetFloat(Animator.StringToHash("CurAttackMotionSpeed"), attackSpeed);

    //    MyAnimator.SetFloat(stateMachine.Character.DataAnim.CurAttackMotionSpeedParameterHash, attackSpeed);
    //}
    //public void ChangeAttackMotionSpeed()
    //{
    //    //Debug.Log(CurAs + "/" + CurAsMul);
    //    SetAttackMotionSpeed(CurAs+ CurAsMul);//애니메이션 빠르게
    //    AttackReady = new WaitForSeconds(AttackReadyTime / (CurAs + CurAsMul))
    //    ; //공격 적용시점도 빠르게
}

