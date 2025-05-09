using UnityEngine;

public class GridTile : MonoBehaviour
{
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 0); // 투명
    }

    void OnMouseEnter()
    {
        Debug.Log("working");
        sprite.color = new Color(0.5f, 1f, 0.5f, 0.3f); // 연초록 반투명
    }

    void OnMouseExit()
    {
        sprite.color = new Color(1, 1, 1, 0); // 다시 투명
    }
}