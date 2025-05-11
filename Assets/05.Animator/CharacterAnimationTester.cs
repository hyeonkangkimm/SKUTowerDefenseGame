using UnityEngine;

public class CharacterAnimationTester : MonoBehaviour
{
    public Animator animator;

    public void PlayAttack()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator가 연결되지 않았습니다.");
            return;
        }

        animator.SetTrigger("NormalAttack");
        Debug.Log("Attack 애니메이션 실행");
    }

    public void PlayIdle()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator가 연결되지 않았습니다.");
            return;
        }

        animator.Play("Idle"); // "Idle"은 애니메이션 상태 이름
        Debug.Log("Idle 애니메이션 실행");
    }
    public void PlayWalk()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator가 연결되지 않았습니다.");
            return;
        }
        animator.SetBool("Idle", false);
        animator.Play("Pursuit"); 
        Debug.Log("Pursuit 애니메이션 실행");
    }
}