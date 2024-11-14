using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTile : Tile
{
    public SpriteRenderer render;
    public bool visited = false;
    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }
}
