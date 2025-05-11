using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //캐릭터 탐색을 한다
    //public GameObject Player;

    public float cellSize = 10f;
    public Dictionary<Vector2Int, List<Character>> grid; 
    protected override void Awake()
    {
        base.Awake();
        grid = new Dictionary<Vector2Int, List<Character>>();
    }
    public Vector2Int GetCell(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.z / cellSize)
        );
    }
    public void Register(Character character)
    {
        var cell = GetCell(character.transform.position);
        if (!grid.ContainsKey(cell))
            grid[cell] = new List<Character>();
        grid[cell].Add(character);
    }

    public void Unregister(Character character)
    {
        var cell = GetCell(character.transform.position);
        if (grid.ContainsKey(cell))
            grid[cell].Remove(character);
    }

    public List<Character> GetHeroesNear(Vector3 position, int range = 1)
    {
        List<Character> result = new();
        var baseCell = GetCell(position);

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dz = -range; dz <= range; dz++)
            {
                Vector2Int neighbor = baseCell + new Vector2Int(dx, dz);
                if (grid.ContainsKey(neighbor))
                    result.AddRange(grid[neighbor]);
            }
        }
        return result;
    }
    public void UpdateHeroPosition(Character character)
    {
        Unregister(character);
        Register(character);
    }
}
