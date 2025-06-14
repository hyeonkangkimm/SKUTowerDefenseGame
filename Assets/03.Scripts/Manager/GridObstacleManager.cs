using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObstacleManager : Singleton<GridObstacleManager>
{
    public int gridWidth = 5;
    public int gridHeight = 9;
    public float cellSize = 4f;
    public Vector2 origin = new Vector2(18, 6);

    private int[,] gridMap = new int[5, 9]; // 0: 빈칸, 1: 장애물

    public Vector2Int startCell = new Vector2Int(0, 0);
    public Vector2Int goalCell = new Vector2Int(4, 8);

    public bool TryPlaceObstacle(Vector2Int pos)
    {
        if (!IsInBounds(pos)) return false;

        gridMap[pos.x, pos.y] = 1;

        bool pathStillExists = IsPathFromTopToBottom(); 

        if (!pathStillExists)
        {
            gridMap[pos.x, pos.y] = 0;
            return false;
        }

        return true;
    }

    bool IsReachable(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[gridWidth, gridHeight];
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        Vector2Int[] dirs = {
            new Vector2Int(0,1), new Vector2Int(1,0),
            new Vector2Int(0,-1), new Vector2Int(-1,0),
            new Vector2Int(1,1), new Vector2Int(-1,1),
            new Vector2Int(1,-1), new Vector2Int(-1,-1)
        };

        while (queue.Count > 0)
        {
            Vector2Int cur = queue.Dequeue();
            if (cur == goal) return true;

            foreach (var dir in dirs)
            {
                Vector2Int next = cur + dir;
                if (IsInBounds(next) && !visited[next.x, next.y] && gridMap[next.x, next.y] == 0)
                {
                    visited[next.x, next.y] = true;
                    queue.Enqueue(next);
                }
            }
        }
        return false;
    }
    public bool IsPathFromTopToBottom()
    {
        int width = gridMap.GetLength(0);  // 5 (X index)
        int height = gridMap.GetLength(1); // 9 (Z index)

        bool[,] visited = new bool[width, height];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // 1. 맨 위 Z줄 (zIndex == 0)에서 시작
        for (int x = 0; x < width; x++)
        {
            if (gridMap[x, 0] == 0)
            {
                queue.Enqueue(new Vector2Int(x, 0));
                visited[x, 0] = true;
            }
        }

        // 2. BFS 탐색
        Vector2Int[] dirs = {
            new Vector2Int(0,1),  // 위
            new Vector2Int(1,0),  // 오른쪽
            new Vector2Int(0,-1), // 아래
            new Vector2Int(-1,0)  // 왼쪽
        };

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();

            // 아래쪽 줄(Z 마지막) 도달하면 통과
            if (cur.y == height - 1) return true;

            foreach (var dir in dirs)
            {
                var next = cur + dir;
                if (IsInBounds(next) && !visited[next.x, next.y] && gridMap[next.x, next.y] == 0)
                {
                    visited[next.x, next.y] = true;
                    queue.Enqueue(next);
                }
            }
        }

        return false; // 길 없음
    }
    bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public Vector2Int WorldToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt((position.x - origin.x) / cellSize);
        int z = Mathf.RoundToInt((position.z - origin.y) / cellSize);
        Debug.Log($"{x},{z}");
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = origin.x + gridPos.x * cellSize;
        float z = origin.y + gridPos.y * cellSize;
        return new Vector3(x, 0, z);
    }
}