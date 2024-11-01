using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();

    public bool IsTriggered = false;
    public bool canGO = false;
    public GameObject collisionOp;
    Module currentModule;
    Vector2 currentPos;
    Vector2 endPos;
    public int thisA = 65;

    private bool isMoving = false;

    Go pathFinder = new Go();

    private void Start()
    {
        endPos = currentPos;
    }
    void Update()
    {
        currentPos = base.GetPos();
        if (moduleNum == 0)
        {
            endPos = currentPos;
        }
        else
        {
            endPos = GameManager.SetRandEndPos(currentModule.moduleNum);
            Queue<Vector2> path = pathFinder.FindPaths(currentPos, endPos);
            SetPath(path);
        }

        if (pathFinder == null)
        {
            Debug.LogError("Go component is missing on this GameObject");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && pathQueue.Count > 0)
        {
            Debug.Log("Starting movement along path.");
            MoveAlongPath(); // 스페이스바 입력으로 이동 시작
        }
        else if (pathQueue.Count == 0)
        {
            Debug.LogWarning("Path queue is empty. No path found to move.");
        }
    }

    private IEnumerator MoveCoroutine()
    {
        if (pathQueue.Count == 0)
        {
            Debug.LogWarning("MoveCoroutine started but pathQueue is empty.");
            yield break;
        }

        while (pathQueue.Count > 0)
        {
            Vector2 nextPos = pathQueue.Dequeue();
            Vector3 targetPos = new Vector3(nextPos.x, nextPos.y, transform.position.z);

            Debug.Log($"Moving towards: ({nextPos.x}, {nextPos.y})");

            // 타겟 위치에 도달할 때까지 이동
            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 2);
                yield return null;
            }

            // 정확히 타겟 위치에 도달
            transform.position = targetPos;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Tile has reached the destination.");





        if (Input.GetKeyDown(KeyCode.Space) && pathQueue.Count > 0)
        {
            MoveAlongPath(); // 스페이스바 입력으로 이동 시작
        }
    }


    public void SetPath(Queue<Vector2> path)
    {
        pathQueue = path;
    }


    public void MoveAlongPath()
    {
        if (pathQueue.Count > 0)
        {
            StartCoroutine(MoveCoroutine());
        }
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
                Destroy(collisionOp);
                break;
            case "or":
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
}
