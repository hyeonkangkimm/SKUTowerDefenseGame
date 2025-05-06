using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterAnimationTester))]
public class CharacterAnimationTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterAnimationTester tester = (CharacterAnimationTester)target;

        GUILayout.Space(10);

        if (GUILayout.Button("?? Attack 애니메이션 실행"))
        {
            tester.PlayAttack();
        }

        if (GUILayout.Button("?? Idle 애니메이션 실행"))
        {
            tester.PlayIdle();
        }
        if (GUILayout.Button("?? Walk 애니메이션 실행"))
        {
            tester.PlayWalk();
        }
    }
}