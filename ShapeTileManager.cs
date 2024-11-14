using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTileManager : MonoBehaviour
{
    public static List<ShapeTile> allShapeTiles = new List<ShapeTile>();
    public static float moveInterval = 0.5f;
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
                    if (tile != hasOpShape && tile.thisA != hasOpShape.thisA) // thisA가 다른 경우
                    {
                        if (StacksAreEqual(hasOpShape.moveDirectionStack, tile.moveDirectionStack)) // 스택이 같은 경우
                        {
                            ChangePathColor(hasOpShape);
                            ChangePathColor(tile);
                            return true;
                        }
                    }
                }
            }
        }

        // 조건을 만족하는 쌍이 없으면 false 반환
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

        // 이동 방향 스택을 역으로 순회
        Stack<int> reversedStack = new Stack<int>(tile.moveDirectionStack);
        while (reversedStack.Count > 0)
        {
            int direction = reversedStack.Pop();

            // 이동 방향을 반대로 계산하여 이전 위치 찾기
            switch (direction)
            {
                case 0: currentPos += Vector2.down; break;  // 위쪽 이동을 반대로
                case 1: currentPos += Vector2.up; break;    // 아래쪽 이동을 반대로
                case 2: currentPos += Vector2.left; break;  // 오른쪽 이동을 반대로
                case 3: currentPos += Vector2.right; break; // 왼쪽 이동을 반대로
            }

            // 이전 위치 추가
            path.Add(currentPos);
        }

        return path; // 되짚은 경로 반환
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
