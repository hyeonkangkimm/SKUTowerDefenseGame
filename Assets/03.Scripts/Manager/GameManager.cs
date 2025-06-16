using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{

    public GameObject alertPanel;     // 알림 UI 오브젝트 (예: Panel)
    public TextMeshProUGUI alertText;
    private Coroutine currentAlertCoroutine;

    public int ReadyCount = 0;
    
    [SerializeField]public String SelectedPrefabRcode;
    public Tile CurrentTile;
    public ECombatConditionType CombatConditionType = ECombatConditionType.READY;
    public LayerMask layerMask;
    [SerializeField]Camera cam;
    GameObject AlertObj;
    RaycastHit hit;

    private WaitForSecondsRealtime waitRead;

    private void Start()
    {
        Application.targetFrameRate = 60;
        cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
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
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in heroes)
        {
            // 현재 계층에서 활성화되어 있다면 비활성화
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
        }
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject obj in tiles)
        {
            Tile tile = obj.GetComponent<Tile>();
            tile.CurrentPlacedObject = null;
        }



    }

    public void ShowAlert(string message, EAlertType type)
    {
        if (currentAlertCoroutine != null)
        {
            StopCoroutine(currentAlertCoroutine);
        }

        currentAlertCoroutine = StartCoroutine(ShowAlertCoroutine(message, type));
    }

    private IEnumerator ShowAlertCoroutine(string message, EAlertType type)
    {
        // 메시지 설정
        alertText.text = message;

        // 사운드 재생
        switch (type)
        {
            case EAlertType.DENY:
                AudioManager.Instance.PlaySFX("LACK");
                break;
            case EAlertType.WARNING:
                //AudioManager.Instance.PlaySFX("Warning");
                break;
        }

        // 알림 표시
        alertPanel.SetActive(true);

        // 0.5 ~ 1초 유지
        float duration = 0.8f;
        yield return new WaitForSeconds(duration);

        // 알림 숨김
        alertPanel.SetActive(false);
        currentAlertCoroutine = null;
    }
}