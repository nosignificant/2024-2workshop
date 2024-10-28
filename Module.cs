using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField]
    public Tile moduletile;
    private int[,] adjArray; // 모듈 내 이동 가능 여부를 저장하는 배열

    public void CreateModule(int Size, int startPos, Transform pos)
    {
        adjArray = new int[Size, Size];
        for (int i = startPos; i < Size + startPos; i++)
        {
            for (int j = startPos; j < Size + startPos; j++)
            {
                Tile newTile = Instantiate(moduletile, pos);
                newTile.SetTileNum(i, j);
                newTile.transform.position = new Vector3(i, j, 2);
                GameManager.mapArray[i, j] = newTile;

                // 예를 들어, 모든 타일이 이동 가능하도록 설정
                adjArray[i - startPos, j - startPos] = 1;
            }
        }
    }

    public Queue<Vector2> FindPathInModule(int startX, int startY, int endX, int endY)
    {
        Go pathFinder = new Go();
        return pathFinder.FindPaths(startX, startY, endX, endY, adjArray);
    }
}