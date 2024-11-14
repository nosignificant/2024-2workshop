using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    public Tile tilePrefab; // Tile 타입으로 직접 설정
    public ModuleTile moduleTilePrefab;

    public GameObject parent;
    public GameObject GameendText;


    public static List<Module> moduleMap = new List<Module>();

    public static Tile[,] mapArray;
    
    public static int col, row;
    public static bool GameEnd = false;


    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameendText.SetActive(false);
        parent = GameObject.Find("parent");

        Map.SetMap(scene.name);



        // 지도 생성
        CreateMap(row, col);
        for(int i = 0; i < moduleMap.Count; i++)
        {
            moduleMap[i].CreateModule();    
            ModuleTileInstiate(i,parent.transform);
        }
        // 모듈 생성
    }

    private void Update()
    {
        if (GameEnd) { GameendText.SetActive(true); }
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


}
