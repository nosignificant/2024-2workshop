using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Tile tilePrefab; // Tile Ÿ������ ���� ����
    public GameObject modulePrefab;
    public static Tile[,] mapArray;
    public static int[] moduleSize;
    public GameObject parent;
    public ModuleTile moduleTilePrefab;
    public static Module[] moduleArray; // ��� ��ȣ ���� array

    public static int col, row;

    void Start()
    {
        moduleArray = new Module[3];
        parent = GameObject.Find("parent");

        // �迭�� ũ�⸦ �����ϰ� �ʱ�ȭ
        row = 20;
        col = 20;
        mapArray = new Tile[row, col];

        //public Module(int moduleNum, int startX, int startY, int size)
        moduleArray[1] = new Module(1,1,1,10); 
        moduleArray[2] = new Module(2, 11, 11, 1);
        // ���� ����
        CreateMap(row, col);

        moduleArray[1].CreateModule();
        moduleArray[2].CreateModule();
        ModuleTileInstiate(1, parent.transform);
        ModuleTileInstiate(2, parent.transform);
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

    public static Vector2 SetRandEndPos(int moduleNum)
    {
        while (true)
        {
            int rand1 = Random.Range(0, row);
            int rand2 = Random.Range(0, col);

            if (mapArray[rand1, rand2].moduleNum == moduleNum)
            {
                return mapArray[rand1, rand2].GetPos();
            }
        }
    }

    void ModuleTileInstiate(int moduleNum, Transform pos) // �� ���� �ҷ��ͼ� ��������ո��� 
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
