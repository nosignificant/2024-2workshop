using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTileManager : MonoBehaviour
{
    private static List<ShapeTile> allShapeTiles = new List<ShapeTile>();
    public float moveInterval = 1.0f;

    private void Start()
    {
        StartCoroutine(SyncAllShapeTiles());
    }
    private IEnumerator SyncAllShapeTiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);
            foreach (ShapeTile tile in allShapeTiles)
            {
                if (tile.canGO)
                    tile.StartMoveSignal();
            }
        }
    }

    public static List<ShapeTile> GetTilesWithSameA(int value)
    {
        List<ShapeTile> sameATiles = new List<ShapeTile>();
        foreach (ShapeTile tile in allShapeTiles)
        {
            if (tile.thisA == value) 
                sameATiles.Add(tile);
        }
        return sameATiles;
    }
    public static void RegisterShapeTile(ShapeTile tile)
    {
        if (!allShapeTiles.Contains(tile))
        {
            allShapeTiles.Add(tile);
        }
    }

    public static void UnregisterShapeTile(ShapeTile tile)
    {
        if (allShapeTiles.Contains(tile))
        {
            allShapeTiles.Remove(tile);
        }
    }

    public static void ClearAllShapeTileMoveHistories()
    {
        foreach (ShapeTile tile in allShapeTiles)
        {
            tile.ClearMoveHistory();
        }
    }

    public static void ResetTilesIfConditionMet()
    {
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }

}
