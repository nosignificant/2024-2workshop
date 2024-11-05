using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();
    private List<ShapeTile> sameATiles;
    private bool[] HasL_Op = { false, false }; // 0번은 and, 1번은 or 

    public bool IsTriggered = false;
    public bool canGO = false;
    public GameObject collisionOp;
    Module currentModule;
    Vector2 currentPos;
    Vector2 endPos;
    public int thisA = 65;

    private bool isMoving = false;
    private Stack<int> moveDirectionStack = new Stack<int>();

    void Update()
    {
        currentPos = this.GetPos();
    }
    public void StartMoveSignal()
    {
        if (!isMoving && canGO)
        {
            isMoving = true;
            StartCoroutine(MoveCoroutine());

            List<ShapeTile> sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
            foreach (ShapeTile tile in sameATiles)
            {
                if (tile != this && !tile.isMoving)
                {
                    tile.RecieveSignal();
                }
            }
        }
    }
    public void RecieveSignal()
    {
        //Debug.Log("Received Signal: " + this.gameObject.name);
        if (!isMoving)
            this.canGO = true;
    }

    public void RecieveStopSignal()
    {
        //Debug.Log("Received Signal: " + this.gameObject.name);
        StopAllCoroutines();
        canGO = false;
    }
    private IEnumerator MoveCoroutine()
    {
        int randDir;
        Vector2 newEndPos;
        ModuleTile targetTile;
        ModuleTile previousTile = currentModule.SearchModuleTile(moduleNum, currentPos);

        while (true)
        {
            if (!currentModule.CheckForUnvisitedNeighbors(currentPos))
            {
                //Debug.Log("Resetting all tiles to unvisited as no unvisited neighbors found.");
                currentModule.ResetVisited();
                ShapeTileManager.ResetTilesIfConditionMet();

            }

            int x = 0, y = 0;
            randDir = Random.Range(0, 4);

            switch (randDir)
            {
                case 0: y = 1; break;   // Up
                case 1: y = -1; break;  // Down
                case 2: x = 1; break;   // Right
                case 3: x = -1; break;  // Left
            }

            newEndPos = currentPos + new Vector2(x, y) * 1.0f;
            targetTile = currentModule.SearchModuleTile(moduleNum, newEndPos);
            //Debug.Log($"Attempting to move from {currentPos} to {newEndPos}");

            if (targetTile != null && targetTile.moduleNum == currentModule.moduleNum && !targetTile.visited)
            {
                break; // 유효한 타일을 찾았으면 루프 종료
            }
        }

        yield return null;

        this.transform.position = newEndPos;
        targetTile.visited = true;
        moveDirectionStack.Push(randDir);

        if (previousTile != null)
            StartCoroutine(previousTile.FlashWhite(0.2f));

        isMoving = false;
    }


    public void CollisionOp(GameObject gameObject)
    {
        string name = gameObject.name;
        Debug.Log("collision operator name is " + name);
            switch (name)
            {   
                case "plus":
                    thisA++;
                    //foreach (ShapeTile tile in sameATiles) { tile.RecieveStopSignal(); }
                    Debug.Log("o");
                    //sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
                    break;

                case "minus":
                    thisA--;
                    //foreach (ShapeTile tile in sameATiles) { tile.RecieveStopSignal(); }
                    //sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
                    break;

                case "and":
                    if (HasL_Op[1])
                        HasL_Op[1] = false;
                    HasL_Op[0] = true;
                Debug.Log("and met");
                break;

                case "or":
                    if (HasL_Op[0])
                        HasL_Op[0] = false;
                    HasL_Op[1] = true;

                    break;
            }
        Destroy(gameObject);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("module"))
        {
            IsTriggered = true;
            Vector2 collisionPos = other.transform.position;
            for(int i = 0; i < GameManager.row;  i++) {
                for (int j = 0; j < GameManager.col; j++) {
                    Vector2 gameManagerPos = new Vector3(i, j, 5);
                if(gameManagerPos == collisionPos)
                    {
                        this.moduleNum = GameManager.mapArray[i,j].moduleNum;
                        this.currentModule = GameManager.GetModuleInfo(this.moduleNum);

                        break;
                    }
                }
            }
        }
        else if(other.gameObject.name == "canGO")
        {
            canGO = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("operator"))
        {
            if (other != null)
            {
                
                if (other.gameObject.name != "canGO")
                {
                    Debug.Log("Destroy Operator");
                    CollisionOp(other.gameObject);
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "canGO")
        {
            sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
            foreach (ShapeTile tile in sameATiles)
                tile.RecieveStopSignal();
            canGO = false;
        }
    }

    public void ClearMoveHistory()
    {
        moveDirectionStack.Clear();
    }

    private void Awake()
    {
        ShapeTileManager.RegisterShapeTile(this);
    }

    private void OnDestroy()
    {
        ShapeTileManager.UnregisterShapeTile(this);
    }

}
