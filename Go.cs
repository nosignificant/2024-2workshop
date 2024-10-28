using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go : MonoBehaviour
{
    private Stack<Vector2> pathStack = new Stack<Vector2>();
    private bool[,] visited;

    public Queue<Vector2> FindPaths(int startX, int startY, int endX, int endY, int[,] adjArray)
    {
        visited = new bool[adjArray.GetLength(0), adjArray.GetLength(1)];
        Queue<Vector2> pathQueue = new Queue<Vector2>();

        if (DFS(startX, startY, endX, endY, adjArray))
        {
            // Stack을 Queue로 변환하여 경로 반환
            foreach (var pos in pathStack)
            {
                pathQueue.Enqueue(pos);
            }
        }

        return pathQueue;
    }

    private bool DFS(int x, int y, int endX, int endY, int[,] adjArray)
    {
        visited[x, y] = true;

        // 유니티 상의 실제 타일 위치를 가져옵니다.
        Vector2 tilePos = GetModuleTilePos(x, y);
        Vector2 endPos = GetModuleTilePos(endX, endY);
        pathStack.Push(tilePos);

        // 목표에 도달한 경우 경로 반환
        if (tilePos == endPos)
        {
            return true;
        }

        // 상하좌우 탐색
        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1), // Up
            new Vector2(1, 0), // Right
            new Vector2(0, -1), // Down
            new Vector2(-1, 0) // Left
        };

        foreach (var dir in directions)
        {
            int nextX = (int)tilePos.x + (int)dir.x;
            int nextY = (int)tilePos.y + (int)dir.y;

            // 경계 내에 있고, 아직 방문하지 않았고, 이동 가능할 경우
            if (IsValidPosition(nextX, nextY, adjArray) && !visited[nextX, nextY] && adjArray[nextX, nextY] == 1)
            {
                if (DFS(nextX, nextY, endX, endY, adjArray))
                {
                    return true; // 목표를 찾으면 true 반환
                }
                visited[nextX, nextY] = false; // 모든 경로를 탐색하기 위해 방문 표시 해제
            }
        }

        pathStack.Pop();
        return false;
    }

    private bool IsValidPosition(int x, int y, int[,] adjArray)
    {
        return x >= 0 && x < adjArray.GetLength(0) && y >= 0 && y < adjArray.GetLength(1);
    }

    private Vector2 GetModuleTilePos(int x, int y)
    {
        return GameManager.mapArray[x, y].GetPos(); 
    }
}
