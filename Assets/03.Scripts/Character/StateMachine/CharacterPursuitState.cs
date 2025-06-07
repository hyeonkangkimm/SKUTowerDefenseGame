using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class CharacterPursuitState : CharacterBaseState
{
    private float speedModifier;
    private float range;
    //private float curTime = -1f;
    private Transform characterTransform;
    private Vector3 startPos;
    private Vector3 currentPosition;
    //private Transform goBack;
    public CharacterPursuitState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
        speedModifier = stateMachine.Character.StatHandler.baseStat.MoveSpeed;
        range = stateMachine.Character.StatHandler.baseStat.AttackRange;
        characterTransform = stateMachine.Character.transform;
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Character.DataAnim.PursuitParameterHash);
        //curTime = -1f;
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        switch (GameManager.Instance.CombatConditionType)
        {
            case ECombatConditionType.START:
                if (stateMachine.Character.Target != null)
                {
                }
                break;
            case ECombatConditionType.END:
                stateMachine.ChangeState(stateMachine.Idle);
                break;
            case ECombatConditionType.READY:
                if (true)
                {

                }
                else
                {

                }
                break;
        }
    }
    public float DistanceToTarget(Transform Target)
    {
        return (characterTransform.position - Target.position).magnitude;
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Character.DataAnim.PursuitParameterHash);
    }
}
