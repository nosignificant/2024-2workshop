using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go
{
    private Stack<Vector2> pathStack = new Stack<Vector2>();
    private bool[,] visited;
    private int startX, startY, endX, endY;
    private int xSize, ySize;

    public Queue<Vector2> FindPaths(Vector2 currentPos, Vector2 endPos)
    {
        startX = (int)currentPos.x;
        startY = (int)currentPos.y;
        endX = (int)endPos.x;
        endY = (int)endPos.y;
        xSize = Mathf.Abs(endX - startX) + 1;
        ySize = Mathf.Abs(endY - startY) + 1;

        visited = new bool[xSize, ySize];
        Queue<Vector2> pathQueue = new Queue<Vector2>();

        if (DFS(startX, startY, endX, endY)) //DFS(int x, int y, int endX, int endY)
        {
            // Stack을 Queue로 변환하여 경로 반환
            foreach (var pos in pathStack)
            {
                pathQueue.Enqueue(pos);
            }
        }

        return pathQueue;
    }

    private bool DFS(int x, int y, int endX, int endY)
    {
        if (!IsValidPosition(x, y) || visited[x - startX, y - startY])
        {
            return false;
        }

        visited[x - startX, y - startY] = true;
        pathStack.Push(new Vector2(x, y));

        // 목표에 도달한 경우 경로 반환
        if (x == endX && y == endY)
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
            int nextX = x + (int)dir.x;
            int nextY = y + (int)dir.y;

            if (DFS(nextX, nextY, endX, endY))
            {
                return true; // 목표를 찾으면 true 반환
            }
        }

        pathStack.Pop();
        return false;
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= startX && x < startX + xSize && y >= startY && y < startY + ySize;
    }
}
