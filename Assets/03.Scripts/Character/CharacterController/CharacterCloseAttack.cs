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
    public Transform meleePos;
    public Vector2 boxSize;
    [SerializeField]protected CharacterControllerH characterController;
    public CharacterStateMachine stateMachine;
    public Animator MyAnimator;
    public float timeSinceLastAttack;
    public float AttackReadyTime;
    public float AttackAnimationLength;
    public AnimationClip clip;
    float attackRange => characterController.character.StatHandler.curStat.AttackRange;
    float CurAtk => characterController.character.StatHandler.curStat.Atk;
    protected float CurAs => characterController.character.StatHandler.curStat.AttackSpeed;
    protected float CurAsMul => characterController.character.StatHandler.curStat.AttackSpeedMultiplier;
    public float CriticalChance=0f;
    public virtual void  Awake()
    {
        characterController  = GetComponentInParent<CharacterControllerH>();
        timeSinceLastAttack = 0f;
        AttackReady = new WaitForSeconds(AttackReadyTime == 0f ? 1.35f : AttackReadyTime);


    }
    public virtual void Start()
    {
        stateMachine = characterController.character.StateMachine;
        AttackAnimationLength = clip.length;
    }
    public void FixedUpdate()
    {
        CriticalChance= characterController.character.StatHandler.curStat.GetCurCriticalInfo().CriRate;
        //크리티컬적용확인용
    }
    public void OnDisable()
    {
        characterController.OnAttack -= onAttack;
        //characterController.OnAttackSpeedChange -= ChangeAttackMotionSpeed;

    }
    public void OnEnable()
    {
        characterController.OnAttack += onAttack;
        //characterController.OnAttackSpeedChange += ChangeAttackMotionSpeed;
    }

    public void Update()
    {
        if(timeSinceLastAttack > AttackAnimationLength / (CurAs + CurAsMul))
        {
            characterController.isAttacking = false;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
    private void onAttack()
    {
        ChangeAttackMotionSpeed(); //AS에 따라 애니메이션 속도 변경
        if (characterController.character.Animator.GetBool(characterController.character.DataAnim.NormalAttackParameterHash))
        {
            timeSinceLastAttack = 0f;
            characterController.isAttacking = true;
            StartCoroutine(CloseAttack());
           /*공격 동작(준비/공격/마무리)은 시작했는데,준비동작에서 데미지를 입히면 이상하니까 
            공격동작에서 데미지를 입히게끔 코루틴을 사용하였음*/
        }
        
    }
    private IEnumerator CloseAttack()
    {
        //if (characterController.character.Target != null)
            //MoveMeleePos(characterController.character.Target.position, attackRange);
        yield return AttackReady;
        if (!characterController.isDead)
        {
            if(characterController.DetectCollider is BoxCollider box)
            {
                Vector3 worldCenter = box.transform.TransformPoint(box.center);
                Vector3 worldSize = Vector3.Scale(box.size, box.transform.lossyScale) * 0.5f;
                Quaternion rotation = box.transform.rotation;
                Collider[] hits = Physics.OverlapBox(worldCenter, worldSize, rotation);
            
                 foreach (Collider colider in hits)
                 {
                     if (true)
                     {
                         IDamageable damagable = colider.GetComponent<IDamageable>();
                        damagable?.TakeDamage(characterController.character.StatHandler.curStat.GetCurAtk());
                        Debug.Log("근접공격");
                        //var (damage, isCritical) = characterController.CalculateDamage(characterController.character.StatHandler.curStat.GetCurAtk());
                         //damagable?.TakeDamage(damage, isCritical);
                     }
                 }
            }
        }
    }

    /// <summary>
    /// 타겟위치로 근접공격 위치 재설정
    /// </summary>
    /// <param name="target"></param>
    private void MoveMeleePos(Vector3 target ,float range =1f)
    {
        Vector3 direction = Vector3.Normalize(target - characterController.transform.position);
        Vector3 newMeleePos = direction * range;
        meleePos.localPosition = newMeleePos;
    }


    private void SetAttackMotionSpeed(float attackSpeed=1f)
    {
        //MyAnimator.SetFloat(Animator.StringToHash("CurAttackMotionSpeed"), attackSpeed);

        MyAnimator.SetFloat(stateMachine.Character.DataAnim.CurAttackMotionSpeedParameterHash, attackSpeed);
    }
    public void ChangeAttackMotionSpeed()
    {
        //Debug.Log(CurAs + "/" + CurAsMul);
        SetAttackMotionSpeed(CurAs+ CurAsMul);//애니메이션 빠르게
        AttackReady = new WaitForSeconds(AttackReadyTime * (1 / (CurAs + CurAsMul)))
        ; //공격 적용시점도 빠르게
    }
}
