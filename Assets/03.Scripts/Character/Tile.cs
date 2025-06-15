using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 배치하고 난 후에 실행하고 싶은 것 구현
/// ex)배치하면 주변 적 얼음
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
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
        GameManager.Instance.CurrentTile = this;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
        if (GameManager.Instance.CurrentTile == this)
        {
            GameManager.Instance.CurrentTile = null;
        }
    }

        public bool OnPlaceCharacter(string rcode)
        {
            //벽설치, 포탑설치, 캐릭터 설치 대응해야함
            if (CurrentPlacedObject != null && CurrentPlacedObject.activeInHierarchy)
            {
            Debug.Log("이미 오브젝트가 배치된 타일입니다.");
            return false;  // 소환 실패
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

                          return true;  // 소환 성공

                    }
                    if (prefabToPlace == null)
                        Debug.Log("로딩안됨");


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
            if (gridPos.y == 0 || gridPos.y == 7)
            {
                Debug.Log("좌측끝 우측끝에는 벽 설치 불가");
                return false;
            }

            // 1. 장애물 설치 가능 검사
            bool canPlace = GridObstacleManager.Instance.TryPlaceObstacle(gridPos);

            if (!canPlace)
            {
                Debug.Log("경로 차단됨! 벽 설치 취소");
                return false;
            }

            // 2. 설치 가능하면 Pool에서 프리팹 꺼내기
            GameObject prefabToPlace = PoolManager.Instance.SpawnFromPool(rcode);
            prefabToPlace.transform.parent = this.transform;
            if (prefabToPlace != null)
            {
                prefabToPlace.transform.localPosition = Vector3.zero;

                var placeable = prefabToPlace.GetComponent<IPlaceable>();
                if (placeable != null)
                    placeable.OnPlaced(placePosition);

                CurrentPlacedObject = prefabToPlace;
                return true;
            }
            else
            {
                Debug.LogWarning("벽 프리팹 로딩 실패");
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
