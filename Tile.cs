using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileX; // Ÿ���� X �ε���
    private int tileY; // Ÿ���� Y �ε���
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
