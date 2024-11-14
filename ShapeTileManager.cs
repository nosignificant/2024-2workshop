using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTileManager : MonoBehaviour
{
    public static List<ShapeTile> allShapeTiles = new List<ShapeTile>();
    public static float moveInterval = 0.5f;
    public static bool[] OpSign = { false, false };  // 0���� and, 1���� or 

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
        if (CheckLOp())
        {
            Debug.Log("Check move path. result: " + CheckLOp() + " Stop coroutines");

            foreach (ShapeTile tile in allShapeTiles) { 
                tile.isMoving = true;
                tile.StopAllCoroutines();
            }

            GameManager.GameEnd = true;
        }
        ClearAllShapeTileMoveHistories();
        Debug.Log("Reset all movepath");
    }

    public static bool CheckLOp()
    {
        ShapeTile hasOpShape = null;

        // LOp�� ���� Ÿ���� ã��
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
                    if (tile != hasOpShape && tile.thisA == hasOpShape.thisA) // thisA�� ���� ���
                    {
                        if (StacksAreEqual(hasOpShape.moveDirectionStack, tile.moveDirectionStack)) // ���õ� ���� ���
                        {
                            ChangePathColor(tile);
                            ChangePathColor(hasOpShape);
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
                    if (tile != hasOpShape && tile.thisA != hasOpShape.thisA) // thisA�� �ٸ� ���
                    {
                        if (StacksAreEqual(hasOpShape.moveDirectionStack, tile.moveDirectionStack)) // ������ ���� ���
                        {
                            ChangePathColor(hasOpShape);
                            ChangePathColor(tile);
                            return true;
                        }
                    }
                }
            }
        }

        // ������ �����ϴ� ���� ������ false ��ȯ
        Debug.Log("return false");
        return false;
    }


    private static bool StacksAreEqual(Stack<int> stack1, Stack<int> stack2)
    {if(stack1.Count > 0)
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
        return false;
    }

    private static List<Vector2> GetReversedPath(ShapeTile tile)
    {
        Vector2 currentPos = tile.transform.position;
        List<Vector2> path = new List<Vector2> { currentPos };

        // �̵� ���� ������ ������ ��ȸ
        Stack<int> reversedStack = new Stack<int>(tile.moveDirectionStack);
        while (reversedStack.Count > 0)
        {
            int direction = reversedStack.Pop();

            // �̵� ������ �ݴ�� ����Ͽ� ���� ��ġ ã��
            switch (direction)
            {
                case 0: currentPos += Vector2.down; break;  // ���� �̵��� �ݴ��
                case 1: currentPos += Vector2.up; break;    // �Ʒ��� �̵��� �ݴ��
                case 2: currentPos += Vector2.left; break;  // ������ �̵��� �ݴ��
                case 3: currentPos += Vector2.right; break; // ���� �̵��� �ݴ��
            }

            // ���� ��ġ �߰�
            path.Add(currentPos);
        }

        return path; // ��¤�� ��� ��ȯ
    }

    public static void ChangePathColor(ShapeTile tile)
    {
        List<Vector2> path = GetReversedPath(tile);
        foreach (Vector2 pos in path)
        {
            ModuleTile targetTile = GameManager.moduleMap[tile.currentModule.moduleNum].SearchModuleTile(tile.currentModule.moduleNum, pos);
            if (targetTile != null)
            {
                targetTile.render.color = Color.blue;
            }
        }
    }
}
