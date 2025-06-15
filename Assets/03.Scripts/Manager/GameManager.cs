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
    public Tile CurrentTile;
    public ECombatConditionType CombatConditionType = ECombatConditionType.READY;
    public LayerMask layerMask;
    [SerializeField]Camera cam;
    
    RaycastHit hit;

    private WaitForSecondsRealtime waitRead;

    private void Start()
    {
        Application.targetFrameRate = 60;
        cam = Camera.main;
    }


    private void Update()
        
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
        Tile foundTile = null;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                foundTile = tile;
                break;
            }
        }

        if (foundTile != null)
        {
            
            if (CurrentTile!=null&&CurrentTile != foundTile)
            {
                CurrentTile.OnHoverExit();
            }
            CurrentTile = foundTile;
            CurrentTile.OnHovering();
        }
        else //타일밖으로 가면
        {
            if (CurrentTile != null)
                CurrentTile.OnHoverExit();
            CurrentTile = null;
            Debug.Log("Tile 밖");
        }

                


    }
   

    //기존에 있던 히어로 disable
    public void OnNextWaveStart()
    {
        Hero[] heroes = FindObjectsByType<Hero>(FindObjectsSortMode.None);
        foreach (Hero hero in heroes)
        {
            GameObject obj = hero.gameObject;
            // 현재 계층에서 활성화되어 있다면 비활성화
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }
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