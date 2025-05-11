using System;
using UnityEngine.PlayerLoop;

public class CharacterStateMachine : StateMachine
{
    public Character Character;
    public CharacterIdleState Idle;
    public CharacterDeathState Death;

    public CharacterPursuitState Pursuit;
    public CharacterNormalAttackState NormalAttack;
    public CharacterCriticAttackState CriticAttack;
    public CharacterSkillState Skill;


    public CharacterStateMachine(Character Character)
    {
        this.Character = Character;
        Idle = new CharacterIdleState(this);
        Pursuit = new CharacterPursuitState(this);
        NormalAttack = new CharacterNormalAttackState(this);
        CriticAttack = new CharacterCriticAttackState(this);
        Skill = new CharacterSkillState(this);
        Death = new CharacterDeathState(this);
    }
    
    public void Initialize()
    {
        currentState = Idle;
    }
}
