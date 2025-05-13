public class CharacterCriticAttackState : CharacterCombatState
{
    public CharacterCriticAttackState(CharacterStateMachine stateMachine) : base(stateMachine)
    {

    }
    public override void Enter()
    {
        StartAnimation(stateMachine.Character.DataAnim.CriticAttackParameterHash);
        base.Enter();

    }
    public override void Update()
    {

        //stateMachine.Character.Controller.CallAttack();

        base.Update();
    }
    public override void Exit()
    {
        StopAnimation(stateMachine.Character.DataAnim.CriticAttackParameterHash);
        base.Exit();
    }

}
