using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Tile tilePrefab; // Tile 타입으로 직접 설정
    public GameObject modulePrefab;
    public static Tile[,] mapArray;
    public GameObject parent;

    public static int col,row;

    void Start()
    {
        parent = GameObject.Find("parent");

        // 배열의 크기를 설정하고 초기화
        row = 10;
        col = 10;
        mapArray = new Tile[row, col];

        GameObject moduleInstance = Instantiate(modulePrefab);
        Module module1 = moduleInstance.GetComponent<Module>();
        module1.SetModuleNum(1);

        // 지도 생성
        CreateMap(row, col);
        module1.CreateModule(3, 3, moduleInstance.transform);

        // 경로 찾기 및 설정
        Queue<Vector2> path = module1.FindPathInModule(0, 0, 2, 2); // 시작점 (0,0), 목적지 (2,2)

        ShapeTile shapeTile = GameObject.FindObjectOfType<ShapeTile>(); // ShapeTile 객체 찾기
        if (shapeTile != null)
        {
            shapeTile.SetPath(path); // 찾은 경로를 ShapeTile에 설정
        }
    }




    void CreateMap(int row, int col)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++) {
                Tile tileInstance = Instantiate(tilePrefab, parent.transform);
                tileInstance.transform.position = new Vector3(i, j, 10);
                tileInstance.SetTileNum(i, j);
                mapArray[i, j] = tileInstance;
            }
        }
    }

    public void NextTilePos(int x, int y)
    {
        int a = mapArray[x, y].GetTileNumX();
        int b = mapArray[x, y].GetTileNumY();

        mapArray[a + PlayerMove.playerMoveX, b + PlayerMove.playerMoveY].GetPos();
    }

    /*public void SearchTilePos(Vector2 pos)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                
                if(pos == mapArray[i, j].GetPos()) {
                return 
                }
                    
            }
        }
    }*/
}
