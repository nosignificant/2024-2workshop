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
        Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>(); // ��� ������

        // ���� ��ġ ����
        Vector2 start = new Vector2(startX, startY);
        queue.Enqueue(start);
        visited[startX, startY] = true;

        bool pathFound = false;

        // BFS Ž��
        while (queue.Count > 0 && !pathFound)
        {
            Vector2 current = queue.Dequeue();

            // ��ǥ�� �����ϸ� BFS ����
            if (current.x == endX && current.y == endY)
            {
                pathFound = true;
                break;
            }

            // �����¿� ���� Ž��
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

                // ���� ��ġ�� ��ȿ�ϰ� ���� �湮���� �ʾ����� ť�� �߰�
                if (IsValidPosition(nextX, nextY) && !visited[nextX, nextY])
                {
                    queue.Enqueue(nextPos);
                    visited[nextX, nextY] = true;
                    parentMap[nextPos] = current; // ��� ������ ���� �θ� ��� ����
                }
            }
        }

        // ��ΰ� �߰ߵ� ���, ��ǥ �������� ���� ��ġ���� ��θ� �������Ͽ� Queue�� ����
        if (pathFound)
        {
            Vector2 step = new Vector2(endX, endY);
            while (step != start)
            {
                pathQueue.Enqueue(step);
                step = parentMap[step];
            }
            pathQueue.Enqueue(start); // ���� ��ġ �߰�

            // Queue�� �������� �Ǿ� �����Ƿ� ������ ������
            pathQueue = new Queue<Vector2>(new Stack<Vector2>(pathQueue));
        }

        return pathQueue;
    }

    private bool IsValidPosition(int x, int y)
    {
        // ��ü �� ũ�� ������ ��ȿ���� Ȯ��
        return x >= 0 && x < GameManager.row && y >= 0 && y < GameManager.col;
    }
}
