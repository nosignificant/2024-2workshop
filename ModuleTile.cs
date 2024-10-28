using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleTile : Tile
{
    public static bool IsTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        IsTriggered = true; 
    }
}
