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

    // ShapeTile�� ����Ʈ���� �����ϴ� �޼���
    public static void UnregisterShapeTile(ShapeTile tile)
    {
        if (allShapeTiles.Contains(tile))
        {
            allShapeTiles.Remove(tile);
        }
    }

    // ��� ShapeTile�� �̵� ����� �ʱ�ȭ�ϴ� �޼���
    public static void ClearAllShapeTileMoveHistories()
    {
        foreach (ShapeTile tile in allShapeTiles)
        {
            tile.ClearMoveHistory();
        }
    }

    // Ư�� ���ǿ��� ȣ���ϴ� ����
    public static void ResetTilesIfConditionMet()
    {
        // ������ Ȯ���� �� ��� ShapeTile�� �̵� ����� �ʱ�ȭ
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }
}
