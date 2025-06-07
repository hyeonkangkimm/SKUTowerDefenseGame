
public class CharacterNormalAttackState : CharacterCombatState
{
    public CharacterNormalAttackState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    public override void Enter()
    {
        StartAnimation(stateMachine.Character.DataAnim.NormalAttackParameterHash);
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        stateMachine.Character.Controller.CallAttack();

    }
    public override void Exit()
    {
        StopAnimation(stateMachine.Character.DataAnim.NormalAttackParameterHash);
        base.Exit();
    }
    
}
