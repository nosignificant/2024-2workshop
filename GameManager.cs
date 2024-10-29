using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Tile tilePrefab; // Tile 타입으로 직접 설정
    public GameObject modulePrefab;
    public static Tile[,] mapArray;
    public static int[] moduleSize;
    public GameObject parent;
    public ModuleTile moduleTilePrefab;
    public static Module[] moduleArray;

    public static int col, row;

    void Start()
    {
        moduleArray = new Module[3];
        parent = GameObject.Find("parent");

        // 배열의 크기를 설정하고 초기화
        row = 10;
        col = 10;
        mapArray = new Tile[row, col];

        moduleArray[1] = new Module(1,3,2,3); //public Module(int moduleNum, int startX, int startY, int size)

        // 지도 생성
        CreateMap(row, col);
        moduleArray[1].CreateModule();
        ModuleTileInstiate(1, parent.transform);

        // 모듈 생성
    }

    void CreateMap(int row, int col)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Tile tileInstance = Instantiate(tilePrefab, parent.transform);
                tileInstance.transform.position = new Vector3(i, j, 10);
                tileInstance.SetTileNum(i, j);
                mapArray[i, j] = tileInstance;
            }
        }
    }

    void ModuleTileInstiate(int moduleNum, Transform pos) // 맵 정보 불러와서 모듈프리팹만듦 
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(mapArray[i, j].isModule == true)
                {
                    ModuleTile newTile = Instantiate(moduleTilePrefab, pos);
                    newTile.transform.position = new Vector3(i, j, 10);
                    newTile.isModule = true;
                    newTile.moduleNum = moduleNum;
                    mapArray[i, j] = newTile;
                }
            }
        }
    }

    public static int SearchTileNumX(Vector2 pos)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (pos == mapArray[i, j].GetPos())
                {
                    return mapArray[i, j].GetTileNumX();
                }
            }
        }
        return -1;
    }

    public static Module GetModuleInfo(int moduleNum)
    {
        return moduleArray[moduleNum];
    }

    public static int SearchTileNumY(Vector2 pos)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (pos == mapArray[i, j].GetPos())
                {
                    return mapArray[i, j].GetTileNumY();
                }
            }
        }
        return -1;
    }
}
