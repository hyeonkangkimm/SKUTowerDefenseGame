using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    private Action OnDeath;
    private void OnEnable()
    {
        OnDeath += Death;
    }
    private void OnDisable()
    {
        OnDeath -= Death;

    }
    void Death()
    {
        
        //CurrencyManager.Instance.AddCurrency(ECurrencyType.ManaStoneFragment, amount);
    }
}
