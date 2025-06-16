using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ġ�ϰ� �� �Ŀ� �����ϰ� ���� �� ����
/// ex)��ġ�ϸ� �ֺ� �� ����
/// </summary>
public interface IPlaceable
{
    void OnPlaced(Vector3 position);
}

public class Tile : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public GameObject CurrentPlacedObject;
    private CharacterControllerH SummonPrefabControl;
    LayerMask tileLayer;
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    //void OnMouseEnter()
    //{
    //    rend.material.color = Color.yellow;
    //    GameManager.Instance.CurrentTile = this;
    //}

    //void OnMouseExit()
    //{
    //    rend.material.color = originalColor;
    //}
    public void OnHovering() 
    {
        if (CurrentPlacedObject == null || !CurrentPlacedObject.activeSelf)
        {
            rend.material.color = Color.yellow;

        }
        else
        {
            rend.material.color = Color.red;

        }
    }
    public void OnHoverExit()
    {
        rend.material.color = originalColor;
    }

   
    public bool OnPlaceCharacter(string rcode)
    {
        
        if (CurrentPlacedObject != null && CurrentPlacedObject.activeInHierarchy)
        {
            return false;  //
         }
            GameObject prefabToPlace = PoolManager.Instance.SpawnFromPool(rcode);
       
       
                if (prefabToPlace != null)
                {
                    SummonPrefabControl = prefabToPlace.GetComponent<CharacterControllerH>();
                    OnSummonCharacter(SummonPrefabControl);
                    prefabToPlace.transform.position = this.transform.position;

                    var placeable = prefabToPlace.GetComponent<IPlaceable>();
                
                    if (placeable != null)
                        placeable.OnPlaced(this.transform.position);

                    CurrentPlacedObject = prefabToPlace;
            AudioManager.Instance.PlaySFX("SPAWN");

            return true;  

                }
                if (prefabToPlace == null)
                    Debug.Log("�ε��ȵ�");
            return false;
    }
    public bool OnPlaceWall(string rcode)
    {
        if (CurrentPlacedObject != null && CurrentPlacedObject.activeInHierarchy)
        {
            return false;
        }
            Vector3 placePosition = this.transform.localPosition;
            Vector2Int gridPos = GridObstacleManager.Instance.WorldToGrid(placePosition);
            if (gridPos.y == 0 || gridPos.y == 8)
            {
                Debug.Log("좌측끝 우측끝 설치불가");
            GameManager.Instance.ShowAlert("좌측끝 우측끝 설치불가!", 0);

            return false;
            }

            // 1. 설치여부검사
            bool canPlace = GridObstacleManager.Instance.TryPlaceObstacle(gridPos);

            if (!canPlace)
            {
                GameManager.Instance.ShowAlert("경로는 최소 하나는 유지 되어야 합니다!", 0);
                return false;
            }

            // 2.설치실행
            GameObject prefabToPlace = PoolManager.Instance.SpawnFromPool(rcode);
            prefabToPlace.transform.parent = this.transform;
            if (prefabToPlace != null)
            {
                prefabToPlace.transform.localPosition = Vector3.zero;

                var placeable = prefabToPlace.GetComponent<IPlaceable>();
                if (placeable != null)
                    placeable.OnPlaced(placePosition);

                CurrentPlacedObject = prefabToPlace;
            AudioManager.Instance.PlaySFX("STONE");
                return true;
            }
            else
            {
                Debug.LogWarning("로드실패");
                return false;
            }
        
    }
    void OnSummonCharacter(CharacterControllerH controller)
    {
        controller.OnDeath += OnSummonCharacterDie;
    }
    void OnSummonCharacterDie()
    {
        CurrentPlacedObject = null;
        SummonPrefabControl.OnDeath -= OnSummonCharacterDie;
    }

}
