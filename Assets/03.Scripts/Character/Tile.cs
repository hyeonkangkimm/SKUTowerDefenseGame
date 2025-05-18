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
    public GameObject currentPlacedObject;
    private CharacterControllerH SummonPrefabControl;
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
        GameManager.Instance.CurrentTIle = this;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

    public void PlaceCharacter(string rcode)
    {
        //벽설치, 포탑설치, 캐릭터 설치 대응해야함
        if (currentPlacedObject == null)
        {
            GameObject prefabToPlace = PoolManager.Instance.SpawnFromPool(rcode);
            if (prefabToPlace != null)
            {
                SummonPrefabControl = prefabToPlace.GetComponent<CharacterControllerH>();
                OnSummonCharacter(SummonPrefabControl);
                prefabToPlace.transform.position = this.transform.position;

                var placeable = prefabToPlace.GetComponent<IPlaceable>();
                if (placeable != null)
                    placeable.OnPlaced(this.transform.position);

                currentPlacedObject = prefabToPlace;

            }
            if (prefabToPlace == null)
                Debug.Log("로딩안됨");
        }
    }
    void OnSummonCharacter(CharacterControllerH controller)
    {
        controller.OnDeath += OnSummonCharacterDie;
    }
    void OnSummonCharacterDie()
    {
        currentPlacedObject = null;
        SummonPrefabControl.OnDeath -= OnSummonCharacterDie;
    }
}
