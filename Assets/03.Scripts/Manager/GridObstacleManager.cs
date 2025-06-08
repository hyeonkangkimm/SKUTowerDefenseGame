using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;
using System.Collections.Generic;

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

        // 임시로 장애물 설치해보기
        gridMap[pos.x, pos.y] = 1;

        // 경로가 막히면 설치 취소
        //bool reachable = IsReachable(startCell, goalCell);
        bool reachable = IsPathFromLeftToRight();
        if (!reachable)
        {
            Debug.Log("경로 차단됨, 설치 불가!");
            gridMap[pos.x, pos.y] = 0; // 되돌림
            return false;
        }

        Debug.Log("설치 완료!");
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
    public bool IsPathFromLeftToRight()
    {
        int width = gridMap.GetLength(0);  // 5
        int height = gridMap.GetLength(1); // 9

        bool[,] visited = new bool[width, height];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // 1. 왼쪽 열(X = 0)에서 시작 가능한 지점들을 큐에 넣기
        for (int y = 0; y < height; y++)
        {
            if (gridMap[0, y] == 0)
            {
                queue.Enqueue(new Vector2Int(0, y));
                visited[0, y] = true;
            }
        }

        // 2. BFS로 탐색
        Vector2Int[] dirs = {
        new Vector2Int(0,1), new Vector2Int(1,0),
        new Vector2Int(0,-1), new Vector2Int(-1,0),
        new Vector2Int(1,1), new Vector2Int(-1,1),
        new Vector2Int(1,-1), new Vector2Int(-1,-1)
    };

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();

            // 도착 판정: 오른쪽 열 도달
            if (cur.x == width - 1) return true;

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

        return false; // 하나도 못 도달
    }
    bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public Vector2Int WorldToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt((position.x - origin.x) / cellSize);
        int z = Mathf.RoundToInt((position.z - origin.y) / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = origin.x + gridPos.x * cellSize;
        float z = origin.y + gridPos.y * cellSize;
        return new Vector3(x, 0, z);
    }
}