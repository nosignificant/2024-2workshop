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
    public Tile tilePrefab; // Tile Ÿ������ ���� ����
    public ModuleTile moduleTilePrefab;

    public GameObject parent;

    public static Module[] moduleMap; // ��� ��ȣ ���� array
    public static Tile[,] mapArray;
    
    public static int col, row;

    void Start()
    {
        moduleMap = new Module[4];
        parent = GameObject.Find("parent");

        // �迭�� ũ�⸦ �����ϰ� �ʱ�ȭ
        row = 20;
        col = 20;
        mapArray = new Tile[row, col];

        //public Module(int moduleNum, int startX, int startY, int sizeX, int sizeY)
        moduleMap[1] = new Module(1,1,1,3,4);
        moduleMap[2] = new Module(2,5,5,3,2);
        moduleMap[3] = new Module(3, 1, 6, 10, 1);

        // ���� ����
        CreateMap(row, col);
        for(int i = 1; i < 4; i++)
        {
            moduleMap[i].CreateModule();
            ModuleTileInstiate(i,parent.transform);
        }


        // ��� ����
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

    void ModuleTileInstiate(int moduleNum, Transform pos) // �� ���� �ҷ��ͼ� ��������ո��� 
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

    // ShapeTile�� ����Ʈ���� �����ϴ� �޼���
    public static void UnregisterShapeTile(ShapeTile tile)
    {
        if (allShapeTiles.Contains(tile))
        {
            allShapeTiles.Remove(tile);
        }
    }

    // ��� ShapeTile�� �̵� ����� �ʱ�ȭ�ϴ� �޼���
    public static void ClearAllShapeTileMoveHistories()
    {
        foreach (ShapeTile tile in allShapeTiles)
        {
            tile.ClearMoveHistory();
        }
    }

    // Ư�� ���ǿ��� ȣ���ϴ� ����
    public static void ResetTilesIfConditionMet()
    {
        // ������ Ȯ���� �� ��� ShapeTile�� �̵� ����� �ʱ�ȭ
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }
}
