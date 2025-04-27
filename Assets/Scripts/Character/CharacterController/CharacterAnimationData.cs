using System;
using UnityEngine;

[Serializable]
public class CharacterAnimationData
{
    [Header("Works Anywhere, Trigger")]
    [SerializeField] private string hurtParameterName = "Hurt";
    [SerializeField] private string deathParameterName = "Death";
        
    [Header("Sub-StateMachine")]
    [SerializeField] private string combatParameterName = "Combat";


    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string pursuitParameterName = "Pursuit";
    [SerializeField] private string normalParameterName = "NormalAttack";
    [SerializeField] private string criticParameterName = "CriticAttack";
    [SerializeField] private string skillParameterName = "Skill";

    [SerializeField] private string CurMotionTimeParameterName = "CurMotionTime";
    [SerializeField] private string CurAttackMotionSpeedParameterName = "CurAttackMotionSpeed";


    public int IdleParameterHash { get; private set; }
    public int PursuitParameterHash { get; private set; }


    public int NormalAttackParameterHash { get; private set; }
    public int CriticAttackParameterHash { get; private set; }
    public int SkillParameterHash { get; private set; }

    public int DeathParameterHash { get; private set; }
    public int HurtParameterHash { get; private set; }
    public int CombatParameterHash { get; private set; }
    public int CurMotionTimeParameterHash { get; private set; }
    public int CurAttackMotionSpeedParameterHash { get; private set; }


    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        PursuitParameterHash = Animator.StringToHash(pursuitParameterName);

        NormalAttackParameterHash = Animator.StringToHash(normalParameterName);
        CriticAttackParameterHash = Animator.StringToHash(criticParameterName);
        SkillParameterHash = Animator.StringToHash(skillParameterName);

        DeathParameterHash = Animator.StringToHash(deathParameterName);
        HurtParameterHash = Animator.StringToHash(hurtParameterName);
        CombatParameterHash = Animator.StringToHash(combatParameterName);
        CurMotionTimeParameterHash = Animator.StringToHash(CurMotionTimeParameterName);
        CurAttackMotionSpeedParameterHash = Animator.StringToHash(CurAttackMotionSpeedParameterName);
    }
}