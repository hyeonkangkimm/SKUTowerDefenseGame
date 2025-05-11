using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    private CharacterControllerH _characterController;
    private void OnEnable()
    {
        _characterController = GetComponent<CharacterControllerH>();
        _characterController.OnDeath += Death;
    }
    private void OnDisable()
    {
        _characterController.OnDeath -= Death;

    }
    void Death()
    {
        
        //CurrencyManager.Instance.AddCurrency(ECurrencyType.ManaStoneFragment, amount);
    }
}
