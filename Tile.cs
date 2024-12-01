using TMPro.Examples;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool[] sign = new bool[8];
    public bool isRead = false;
    public Vector2 GetPos()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void SetPos(float x, float y)
    {
        this.transform.position = new Vector2(x, y);
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
                return i;
                
        return 0;

    }

    public void SetReaded(bool readed)
    {
        this.isRead = readed;
    }
}
