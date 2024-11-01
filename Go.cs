using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go
{
    public Queue<Vector2> FindPaths(Vector2 currentPos, Vector2 endPos)
    {
        int startX = (int)currentPos.x;
        int startY = (int)currentPos.y;
        int endX = (int)endPos.x;
        int endY = (int)endPos.y;

        bool[,] visited = new bool[GameManager.row, GameManager.col];
        Queue<Vector2> queue = new Queue<Vector2>();
        Queue<Vector2> pathQueue = new Queue<Vector2>();
        Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>(); // 경로 추적용

        // 시작 위치 설정
        Vector2 start = new Vector2(startX, startY);
        queue.Enqueue(start);
        visited[startX, startY] = true;

        bool pathFound = false;

        // BFS 탐색
        while (queue.Count > 0 && !pathFound)
        {
            Vector2 current = queue.Dequeue();

            // 목표에 도달하면 BFS 종료
            if (current.x == endX && current.y == endY)
            {
                pathFound = true;
                break;
            }

            // 상하좌우 방향 탐색
            Vector2[] directions = new Vector2[]
            {
                new Vector2(0, 1), // Up
                new Vector2(1, 0), // Right
                new Vector2(0, -1), // Down
                new Vector2(-1, 0) // Left
            };

            foreach (var dir in directions)
            {
                int nextX = (int)current.x + (int)dir.x;
                int nextY = (int)current.y + (int)dir.y;
                Vector2 nextPos = new Vector2(nextX, nextY);

                // 다음 위치가 유효하고 아직 방문하지 않았으면 큐에 추가
                if (IsValidPosition(nextX, nextY) && !visited[nextX, nextY])
                {
                    queue.Enqueue(nextPos);
                    visited[nextX, nextY] = true;
                    parentMap[nextPos] = current; // 경로 추적을 위해 부모 노드 저장
                }
            }
        }

        // 경로가 발견된 경우, 목표 지점부터 시작 위치까지 경로를 역추적하여 Queue에 저장
        if (pathFound)
        {
            Vector2 step = new Vector2(endX, endY);
            while (step != start)
            {
                pathQueue.Enqueue(step);
                step = parentMap[step];
            }
            pathQueue.Enqueue(start); // 시작 위치 추가

            // Queue가 역순으로 되어 있으므로 순서를 뒤집음
            pathQueue = new Queue<Vector2>(new Stack<Vector2>(pathQueue));
        }

        return pathQueue;
    }

    private bool IsValidPosition(int x, int y)
    {
        // 전체 맵 크기 내에서 유효한지 확인
        return x >= 0 && x < GameManager.row && y >= 0 && y < GameManager.col;
    }
}
