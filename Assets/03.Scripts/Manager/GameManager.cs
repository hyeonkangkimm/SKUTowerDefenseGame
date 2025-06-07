using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
   

    public int ReadyCount = 0;
    
    [SerializeField]public String SelectedPrefabRcode;
    public Tile CurrentTIle;
    public ECombatConditionType CombatConditionType = ECombatConditionType.READY;




    private WaitForSecondsRealtime waitRead;

  
    
   
    private void Update()
    {
        //HeroPosUpdate();
    }
   

    //card클래스의 정보를 받아와서 prefab생성
    public void UseHeroCard()
    {

    }

    public void ShowAlert(string message,EAlertType type)
    {
        //AlertObj.ShowAlert(message,type);
    }
    public void ShowAlert()
    {
        //AlertObj.ShowAlert("개발 예정입니다", EAlertType.NOTIMPLEMENTED);
    }
}