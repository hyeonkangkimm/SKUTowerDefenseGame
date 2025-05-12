using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;
public interface IDamageable
{
    public void TakeDamage(int value,bool critic=false); 
}

//[RequireComponent(typeof(StatHandler))]
//[RequireComponent(typeof(CharacterDamaged))]
//[RequireComponent(typeof(StatHandler))]
//[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(HealthSystem))]
public abstract class Character :MonoBehaviour , IDamageable, IPlaceable
{
    //public CharacterSO Data;

    public StatHandler StatHandler;
    public Transform Target;
    public CharacterAnimationData DataAnim;
    public CharacterStateMachine StateMachine;
    public CharacterControllerH Controller;
    public LayerMask LayerMask;
    public EEntityType EntityType;
    public EEntityType TargetType;
    public Animator Animator;
    //public BodyEffect BodyEffect;
    public int CurAtk => (int)StatHandler.curStat.Atk; 

    public HealthSystem Health { get; private set; }    
    // 스테이지 재 시작 시 캐릭터 생성 될 위치
    public Vector3 DefalutPos;
    protected List<Character> targetList = new List<Character>();
    private Dictionary<CharacterStat, Coroutine> activeBuffs;

    protected virtual void Awake()
    {        
        DataAnim.Initialize();
        Animator = GetComponentInChildren<Animator>();
        //StatHandler = GetComponent<StatHandler>();
        Controller = GetComponent<CharacterControllerH>();
        //Health = GetComponent<HealthSystem>();
        StateMachine = new CharacterStateMachine(this);
        StateMachine.Initialize();
        StateMachine.ChangeState(StateMachine.Idle);
        //
    }
    protected virtual void Start()
    {
        
        //StatHandler.UpdateStatModifier();
        //activeBuffs = new();
    }
    protected virtual void OnEnable()
    {
        if(StateMachine != null) StateMachine.ChangeState(StateMachine.Idle);

    }
    public virtual void Update()
    {
       //if(Target == null || !Target.gameObject.activeSelf)
       // {
       //     FindTarget();
       // }
    }

    public void InitStat()
    {
        StatHandler.UpdateStatModifier();
        Health.InitHealth(StatHandler.curStat.GetCurHealth());
    }

    public abstract void FindTarget();
    public abstract void SetTarget();
    public virtual void TakeDamage(int value, bool critic)
    {
        
        Controller.CallOnDamage(value,critic);
    }
    
    IEnumerator BuffCoroutine(CharacterStat buffStat, float wait)
    {

        StatHandler.AddStatModifier(buffStat);
        yield return new WaitForSeconds(wait);

        StatHandler.RemoveStatModifier(buffStat);
        activeBuffs.Remove(buffStat);
    }

    public void OnPlaced(Vector3 position)
    {
         this.transform.position = position;
         this.transform.rotation = Quaternion.identity;
    }
}

