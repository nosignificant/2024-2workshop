using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTileManager : MonoBehaviour
{
    private static List<ShapeTile> allShapeTiles = new List<ShapeTile>();
    public static float moveInterval = 1.0f;
    public static bool[] OpSign = { false, false };  // 0번은 and, 1번은 or 

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
        CheckLOp();
        Debug.Log("Check move path. result: " + CheckLOp());
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }

    public static bool CheckLOp()
    {
        ShapeTile hasOpShape = null;

        // LOp를 가진 타일을 찾음
        foreach (ShapeTile tile in allShapeTiles)
        {
            if (tile.hasLOp)
            {
                hasOpShape = tile;
                break;
            }
        }

        if (hasOpShape != null)
        {
            // AND
            if (OpSign[0])
            {
                Debug.Log("OpSign 0, and operator");
                foreach (ShapeTile tile in allShapeTiles)
                {
                    if (tile != hasOpShape && tile.thisA == hasOpShape.thisA) // thisA가 같은 경우
                    {
                        if (StacksAreEqual(hasOpShape.moveDirectionStack, tile.moveDirectionStack)) // 스택도 같은 경우
                        {
                            return true;
                        }
                    }
                }
            }

            // OR
            if (OpSign[1])
            {
                Debug.Log("OpSign 1, OR operator");
                foreach (ShapeTile tile in allShapeTiles)
                {
                    if (tile != hasOpShape && tile.thisA != hasOpShape.thisA) // thisA가 다른 경우
                    {
                        if (StacksAreEqual(hasOpShape.moveDirectionStack, tile.moveDirectionStack)) // 스택이 같은 경우
                        {
                            return true;
                        }
                    }
                }
            }
        }

        // 조건을 만족하는 쌍이 없으면 false 반환
        return false;
    }


    private static bool StacksAreEqual(Stack<int> stack1, Stack<int> stack2)
    {
        if (stack1.Count != stack2.Count) return false;

        var array1 = stack1.ToArray();
        var array2 = stack2.ToArray();

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i]) return false;
        }

        return true;
    }
}
