using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileX; // 타일의 X 인덱스
    private int tileY; // 타일의 Y 인덱스
    public bool isModule = false;
    public int moduleNum = 0;


    public Vector2 GetPos()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public int GetTileNumX()
    {
        return tileX;
    }

    public int GetTileNumY()
    {
        return tileY;
    }


    public void SetPos(float x, float y)
    {
        this.transform.position = new Vector2(x, y);
    }

    public void SetTileNum(int x, int y)
    {
        this.tileX = x;
        this.tileY = y;
    }

    public void YesModuleNum()
    {
        this.isModule = true;
    }

    public bool IsModule() {
        return isModule;
    }
}
