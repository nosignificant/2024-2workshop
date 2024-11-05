using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();
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
        currentPos = this.transform.position;

        if (canGO)
        {
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(MoveCoroutine());
            }
        }
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
                Debug.Log("Resetting all tiles to unvisited as no unvisited neighbors found.");
                currentModule.ResetVisited();
                GameManager.ResetTilesIfConditionMet();

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

            // 새로운 위치를 계산
            newEndPos = currentPos + new Vector2(x, y) * 1.0f;

            targetTile = currentModule.SearchModuleTile(moduleNum, newEndPos);

            // 타일 유효성 및 방문 여부 확인
            if (targetTile != null && targetTile.moduleNum == currentModule.moduleNum && !targetTile.visited)
            {
                break; // 유효한 타일을 찾았으면 루프 종료
            }
        }

        yield return new WaitForSeconds(1.0f);

        // 새로운 위치로 이동
        this.transform.position = newEndPos;
        targetTile.visited = true;
        moveDirectionStack.Push(randDir);

        // 이전 타일을 흰색으로 깜빡임 처리
        if (previousTile != null)
        {
            StartCoroutine(previousTile.FlashWhite(0.1f));
        }

        isMoving = false;
    }


    public void CollisionOp()
    {
        string name = collisionOp.name;
        switch (name)
        {
            case "plus":
                thisA++;
                Destroy(collisionOp);
                break;

            case "minus":
                thisA--;
                Destroy(collisionOp);
                break;

            case "and":
                if (HasL_Op[1])
                    HasL_Op[1] =false;
                HasL_Op[0] = true;
                Destroy(collisionOp);
                break;

            case "or":
                if (HasL_Op[0])
                    HasL_Op[0] = false;
                HasL_Op[1] = true;
                Destroy(collisionOp);
                break;

            case "canGO":
                canGO = true;
                break;
        }

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
        if (other.gameObject.CompareTag("operator"))
        {
            collisionOp = other.gameObject;
            CollisionOp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "canGO")
        {
            canGO = false;
        }
    }

    // GameManager에서 호출할 수 있는 이동 기록 초기화 메서드
    public void ClearMoveHistory()
    {
        moveDirectionStack.Clear();
    }

    private void Awake()
    {
        // 생성될 때 GameManager에 등록
        GameManager.RegisterShapeTile(this);
    }

    private void OnDestroy()
    {
        // 파괴될 때 GameManager에서 제거
        GameManager.UnregisterShapeTile(this);
    }

}
