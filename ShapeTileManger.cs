using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTileManger : MonoBehaviour
{
    private static List<ShapeTile> allShapeTiles = new List<ShapeTile>();
    public static void RegisterShapeTile(ShapeTile tile)
    {
        if (!allShapeTiles.Contains(tile))
        {
            allShapeTiles.Add(tile);
        }
    }

    // ShapeTile을 리스트에서 제거하는 메서드
    public static void UnregisterShapeTile(ShapeTile tile)
    {
        if (allShapeTiles.Contains(tile))
        {
            allShapeTiles.Remove(tile);
        }
    }

    // 모든 ShapeTile의 이동 기록을 초기화하는 메서드
    public static void ClearAllShapeTileMoveHistories()
    {
        foreach (ShapeTile tile in allShapeTiles)
        {
            tile.ClearMoveHistory();
        }
    }

    // 특정 조건에서 호출하는 예제
    public static void ResetTilesIfConditionMet()
    {
        // 조건을 확인한 후 모든 ShapeTile의 이동 기록을 초기화
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }
}
