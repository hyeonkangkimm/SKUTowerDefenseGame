using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //캐릭터 탐색을 한다
    //public GameObject Player;

    public float cellSize = 10f;
    public Dictionary<Vector2Int, List<Hero>> grid = new Dictionary<Vector2Int, List<Hero>>();

    public Vector2Int GetCell(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.z / cellSize)
        );
    }
    public void Register(Hero hero)
    {
        var cell = GetCell(hero.transform.position);
        if (!grid.ContainsKey(cell))
            grid[cell] = new List<Hero>();
        grid[cell].Add(hero);
    }

    public void Unregister(Hero hero)
    {
        var cell = GetCell(hero.transform.position);
        if (grid.ContainsKey(cell))
            grid[cell].Remove(hero);
    }

    public List<Hero> GetHeroesNear(Vector3 position, int range = 1)
    {
        List<Hero> result = new();
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

    public void UpdateHeroPosition(Hero hero)
    {
        Unregister(hero);
        Register(hero);
    }
    public void OnDisable()
    {
        grid.Clear();
    }
}
