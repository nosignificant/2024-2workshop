using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField]
    public Tile moduletile;
    public int moduleNum;
    private int[,] adjArray; // ��� �� �̵� ���� ���θ� �����ϴ� �迭

    public void CreateModule(int Size, int startPos, Transform pos)
    {
        adjArray = new int[Size, Size];
        for (int i = startPos; i < Size + startPos; i++)
        {
            int a = 0;
            int b = 0;
            for (int j = startPos; j < Size + startPos; j++)
            {
                
                Tile newTile = Instantiate(moduletile, pos);
                newTile.SetTileNum(a, b);
                newTile.transform.position = new Vector3(i, j, 2);
                GameManager.mapArray[i, j] = newTile;

                // ���� ���, ��� Ÿ���� �̵� �����ϵ��� ����
                adjArray[i - startPos, j - startPos] = 1;
                b++;
            }
            a++;
        }
    }

    public void SetModuleNum(int a) { this.moduleNum = a; }
    public int GetModuleNum(int a) { return this.moduleNum; }
    public Queue<Vector2> FindPathInModule(int startX, int startY, int endX, int endY)
    {
        Go pathFinder = new Go();
        return pathFinder.FindPaths(startX, startY, endX, endY, adjArray);
    }
}