using System.Collections;
using System.Collections.Generic; // Queue<> �ڷ����� ����ϱ� ���� ���ӽ����̽� �߰�
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();
    private bool[] HasL_Op = { false, false }; // 0���� and, 1���� or 

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

            // ���ο� ��ġ�� ���
            newEndPos = currentPos + new Vector2(x, y) * 1.0f;

            targetTile = currentModule.SearchModuleTile(moduleNum, newEndPos);

            // Ÿ�� ��ȿ�� �� �湮 ���� Ȯ��
            if (targetTile != null && targetTile.moduleNum == currentModule.moduleNum && !targetTile.visited)
            {
                break; // ��ȿ�� Ÿ���� ã������ ���� ����
            }
        }

        yield return new WaitForSeconds(1.0f);

        // ���ο� ��ġ�� �̵�
        this.transform.position = newEndPos;
        targetTile.visited = true;
        moveDirectionStack.Push(randDir);

        // ���� Ÿ���� ������� ������ ó��
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

    // GameManager���� ȣ���� �� �ִ� �̵� ��� �ʱ�ȭ �޼���
    public void ClearMoveHistory()
    {
        moveDirectionStack.Clear();
    }

    private void Awake()
    {
        // ������ �� GameManager�� ���
        GameManager.RegisterShapeTile(this);
    }

    private void OnDestroy()
    {
        // �ı��� �� GameManager���� ����
        GameManager.UnregisterShapeTile(this);
    }

}
