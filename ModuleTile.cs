using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTile : Tile
{
    SpriteRenderer render;
    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }
    public IEnumerator FlashWhite(float duration)
    {
        render.color = Color.white;  // ������� ����
        yield return new WaitForSeconds(duration);
        render.color = Color.black;  // ���������� �ǵ���
    }
}
