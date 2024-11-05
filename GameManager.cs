using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{

    private static List<ShapeTile> allShapeTiles = new List<ShapeTile>();

    [SerializeField]
    public Tile tilePrefab; // Tile 타입으로 직접 설정
    public ModuleTile moduleTilePrefab;

    public GameObject parent;

    public static Module[] moduleMap; // 모듈 번호 저장 array
    public static Tile[,] mapArray;
    
    public static int col, row;

    void Start()
    {
        moduleMap = new Module[4];
        parent = GameObject.Find("parent");

        // 배열의 크기를 설정하고 초기화
        row = 20;
        col = 20;
        mapArray = new Tile[row, col];

        //public Module(int moduleNum, int startX, int startY, int sizeX, int sizeY)
        moduleMap[1] = new Module(1,1,1,3,4);
        moduleMap[2] = new Module(2,5,5,3,2);
        moduleMap[3] = new Module(3, 1, 6, 10, 1);

        // 지도 생성
        CreateMap(row, col);
        for(int i = 1; i < 4; i++)
        {
            moduleMap[i].CreateModule();
            ModuleTileInstiate(i,parent.transform);
        }


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
        int a = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(mapArray[i, j].moduleNum == moduleNum && mapArray[i, j].isModule == true)
                {
                    ModuleTile newTile = Instantiate(moduleTilePrefab, pos);
                    newTile.transform.position = new Vector3(i, j, 10);
                    newTile.isModule = true;
                    newTile.moduleNum = mapArray[i,j].moduleNum;
                    mapArray[i, j] = newTile;
                    moduleMap[newTile.moduleNum].moduleArray[a++] = newTile;
                }
            }
        }
    }

    public static Tile SearchTile(Vector2 pos)
    {
        foreach (Tile tile in mapArray)
        {
            if (tile.GetPos() == pos) return tile;
        }
        return null;
    }
    public static Module GetModuleInfo(int moduleNum)
    {
        return moduleMap[moduleNum];
    }

    public static void RegisterShapeTile(ShapeTile tile)
    {
        if (!allShapeTiles.Contains(tile))
        {
            allShapeTiles.Add(tile);
        }
    }

    // ShapeTile을 리스트에서 제거하는 메서드
    public static void UnregisterShapeTile(ShapeTile tile)
    {
        if (allShapeTiles.Contains(tile))
        {
            allShapeTiles.Remove(tile);
        }
    }

    // 모든 ShapeTile의 이동 기록을 초기화하는 메서드
    public static void ClearAllShapeTileMoveHistories()
    {
        foreach (ShapeTile tile in allShapeTiles)
        {
            tile.ClearMoveHistory();
        }
    }

    // 특정 조건에서 호출하는 예제
    public static void ResetTilesIfConditionMet()
    {
        // 조건을 확인한 후 모든 ShapeTile의 이동 기록을 초기화
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }
}
