
public class CharacterSkillState : CharacterCombatState
{
    public CharacterSkillState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        StartAnimation(stateMachine.Character.DataAnim.SkillParameterHash);
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        StopAnimation(stateMachine.Character.DataAnim.SkillParameterHash);
        base.Exit();
    }

}