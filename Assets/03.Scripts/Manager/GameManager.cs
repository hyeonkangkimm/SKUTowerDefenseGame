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
        foreach (RaycastHit h in hits)
        {
            Tile tile = h.collider.gameObject.GetComponent<Tile>();
            if (tile != null)
            {
                foundTile = tile;
                break;
            }
        }

        if (foundTile != null)
        {
            if (CurrentTile != foundTile)
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