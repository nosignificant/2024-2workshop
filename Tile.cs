using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x; // 타일의 X 인덱스
    public int y; // 타일의 Y 인덱스
    public bool[] sign = new bool[8];

    public Vector2 GetPos()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public int GetTileNumX()
    {
        return x;
    }

    public int GetTileNumY()
    {
        return y;
    }


    public void SetPos(float x, float y)
    {
        this.transform.position = new Vector2(x, y);
    }

    public void SetTileNum(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool[] GetIsSign()
    {
        return sign;
    }

    public void SetIsSign(int index, bool status)
    {
        sign[index] = status;
    }

    public void SetAllFalseSign()
    {
        for (int i = 0; i < sign.Length; i++)
            sign[i] = false;
    }

    public int isTrueSignHere()
    {
        for (int i = 0; i < sign.Length; i++)
            if (sign[i])
            {
                Debug.Log("true: " + i);
                return i;
            }
                
        return 0;

    }
}
