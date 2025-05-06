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
    public static bool isInit;
    public bool isReady;


    public List<Character> EntryList; /* 0 : Player*/ 
    private readonly float minDistance = 0.5f; /* 캐릭터들이 안 겹치게*/  
    public List<Monster> Monsters;
    public int ReadyCount = 0;


    public ECombatConditionType CombatConditionType = ECombatConditionType.READY;

    private int dungeonNum;



    private WaitForSecondsRealtime waitRead;

  
    
   
    private void Update()
    {
        //HeroPosUpdate();
    }
   

 
    public void HeroPosUpdate()
    {
        foreach (var character in EntryList)
        {
            foreach(var otherCharacter in EntryList)
            {
                if (character == otherCharacter) continue;

                float distance = Vector3.Distance(character.transform.position, otherCharacter.transform.position);
                if (distance < minDistance)
                {
                    Vector3 direction = (character.transform.position - otherCharacter.transform.position).normalized;
                    character.transform.position += direction * (minDistance - distance) / 2;
                    otherCharacter.transform.position -= direction * (minDistance - distance) / 2;
                }
            }
        }
        
    }
  

   

   
  
    public bool CheckHeroReady()
    {
        //if (!player.gameObject.activeSelf) return false;
        foreach(Character hero in EntryList)
        {
            if (!hero.gameObject.activeSelf) continue;
            IState state = hero.StateMachine.currentState;
            if(!(state is CharacterIdleState))
            {
                return false;
            }
        }
        return true;
    }

    public List<Hero> GetHeroEntry()
    {
        return EntryList.OfType<Hero>().ToList(); 
    }

    
    
    public IEnumerator ChangeEntryCoroutine()
    {
        isReady = false;
       
        CombatConditionType = ECombatConditionType.READY;
        for (int i = 1; i < EntryList.Count; i++)
        {
            EntryList[i].gameObject.SetActive(false);
        }
        EntryList.RemoveAll(x => x.GetType() == typeof(Hero));
        
        isReady = true;
        yield return new WaitUntil(CheckHeroReady);
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