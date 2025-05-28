using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState{
    ReadyPhase,
    AttackPhase
}
public class GameFlowManager : MonoBehaviour
{
    public ShopManager shopManager;
    public GameState currentState;

    private void Update()
    {
        //준비 완료 버튼 누르면 AttackPhase로
    }

    public void EnterPreparationPhase()
    {
        shopManager.RollCards();
    }
}
