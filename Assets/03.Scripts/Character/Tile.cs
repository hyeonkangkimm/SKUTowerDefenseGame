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

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

    void OnMouseDown()
    {
        //벽설치, 포탑설치, 캐릭터 설치 대응해야함
        if (currentPlacedObject == null)
        {
            GameObject prefabToPlace = GameManager.Instance.GetSelectedPrefab();
            if (prefabToPlace != null)
            {
                GameObject placed = Instantiate(prefabToPlace, transform.position, Quaternion.identity);
                var placeable = placed.GetComponent<IPlaceable>();
                if (placeable != null)
                    placeable.OnPlaced(this.transform.position);

                currentPlacedObject = placed;
            }
        }
    }
}
