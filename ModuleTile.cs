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
        render.color = Color.white;  // 흰색으로 변경
        yield return new WaitForSeconds(duration);
        render.color = Color.black;  // 검은색으로 되돌림
    }
}
