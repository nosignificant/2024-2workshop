using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Module 
{
    [SerializeField]
    public ModuleTile moduletile;
    public int moduleNum, startX, startY, sizeX, sizeY;
    public bool[,] visited;
    public ModuleTile[] moduleArray;

    public Module(int moduleNum, int startX, int startY, int sizeX, int sizeY)
    {
        this.moduleNum = moduleNum;
        this.startX = startX;
        this.startY = startY;
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.moduleArray = new ModuleTile[sizeX * sizeY];
    }

    public void CreateModule()
    {
        for (int i = startX; i < startX + sizeX; i++)
        {
            for (int j = startY; j < startY + sizeY; j++)
            {
                GameManager.mapArray[i,j].moduleNum = moduleNum;
                GameManager.mapArray[i, j].YesModuleNum();
            }
        }
    }

    public ModuleTile SearchModuleTile(int moduleNum, Vector2 pos)
    {
        foreach (var tile in moduleArray)
        {
            if (tile.GetPos() == pos)
                return tile;
        }
        return null;
    }

    public void ResetVisited()
    {
        foreach (ModuleTile tile in moduleArray)
        {
            if (tile != null)
            {
                tile.visited = false;
            }
        }
    }

    // 특정 위치에 해당하는 ModuleTile의 visited 상태를 반환
    public bool IsTileVisited(Vector2 pos)
    {
        foreach (ModuleTile tile in moduleArray)
        {
            if (tile != null && tile.GetPos() == pos)
            {
                return tile.visited;
            }
        }
        return false;
    }

    public bool CheckForUnvisitedNeighbors(Vector2 pos)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            Vector2 neighborPos = pos + dir;
            ModuleTile neighborTile = SearchModuleTile(this.moduleNum, neighborPos);

            if (neighborTile != null && !IsTileVisited(neighborPos))
            {
                return true;
            }
        }
        return false; // 모든 인접 타일이 방문된 경우 false 반환
    }

    public void SetModuleNum(int a) { this.moduleNum = a; }
    public int GetModuleNum() { return this.moduleNum; }

    public int GetModuleStartX() { return this.startX; }
}
