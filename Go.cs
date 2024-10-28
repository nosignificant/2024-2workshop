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
            // Stack�� Queue�� ��ȯ�Ͽ� ��� ��ȯ
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

        // ����Ƽ ���� ���� Ÿ�� ��ġ�� �����ɴϴ�.
        Vector2 tilePos = GetModuleTilePos(x, y);
        Vector2 endPos = GetModuleTilePos(endX, endY);
        pathStack.Push(tilePos);

        // ��ǥ�� ������ ��� ��� ��ȯ
        if (tilePos == endPos)
        {
            return true;
        }

        // �����¿� Ž��
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

            // ��� ���� �ְ�, ���� �湮���� �ʾҰ�, �̵� ������ ���
            if (IsValidPosition(nextX, nextY, adjArray) && !visited[nextX, nextY] && adjArray[nextX, nextY] == 1)
            {
                if (DFS(nextX, nextY, endX, endY, adjArray))
                {
                    return true; // ��ǥ�� ã���� true ��ȯ
                }
                visited[nextX, nextY] = false; // ��� ��θ� Ž���ϱ� ���� �湮 ǥ�� ����
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
