using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module 
{
    [SerializeField]
    public ModuleTile moduletile;
    public int moduleNum, startX, startY, size;
    private int[,] adjArray;

    public Module(int moduleNum, int startX, int startY, int size)
    {
        this.moduleNum = moduleNum;
        this.startX = startX;
        this.startY = startY;
        this.size = size;
        adjArray = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++) { adjArray[i, j] = 1; }
        }
    }

    public void CreateModule()
    {
        for (int i = startX; i < size + startX; i++)
        {
            for (int j = startY; j < size + startY; j++)
            {
                GameManager.mapArray[i,j].moduleNum = moduleNum;
                GameManager.mapArray[i, j].YesModuleNum();
            }
        }
    }

    public void SetModuleNum(int a) { this.moduleNum = a; }
    public int GetModuleNum() { return this.moduleNum; }

    public int GetModuleStartX() { return this.startX; }
    public int GetModuleSize()
    { return this.size; }
}
