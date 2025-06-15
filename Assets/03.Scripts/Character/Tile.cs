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

    void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
        GameManager.Instance.CurrentTile = this;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }
    public void OnHovering() 
    {
        if (CurrentPlacedObject == null || CurrentPlacedObject.activeSelf)
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
            //����ġ, ��ž��ġ, ĳ���� ��ġ �����ؾ���
            if (CurrentPlacedObject != null && CurrentPlacedObject.activeInHierarchy)
            {
            Debug.Log("�̹� ������Ʈ�� ��ġ�� Ÿ���Դϴ�.");
            return false;  // ��ȯ ����
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

                          return true;  // ��ȯ ����

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
            if (gridPos.y == 0 || gridPos.y == 7)
            {
                Debug.Log("������ ���������� �� ��ġ �Ұ�");
                return false;
            }

            // 1. ��ֹ� ��ġ ���� �˻�
            bool canPlace = GridObstacleManager.Instance.TryPlaceObstacle(gridPos);

            if (!canPlace)
            {
                Debug.Log("��� ���ܵ�! �� ��ġ ���");
                return false;
            }

            // 2. ��ġ �����ϸ� Pool���� ������ ������
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
                Debug.LogWarning("�� ������ �ε� ����");
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
