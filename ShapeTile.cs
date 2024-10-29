using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();

    public bool IsTriggered = false;
    Module currentModule;
    Vector2 currentPos;
    Vector2 endPos;

    Go pathFinder = new Go();
    void Update()
    {
        currentPos = base.GetPos();
        if (moduleNum == 0)
        {
            endPos = currentPos;
        }
        else
        {
            endPos = currentModule.SetRandEndPos(moduleNum);
            Queue<Vector2> path = pathFinder.FindPaths(currentPos, endPos);
            foreach (var queue in path)
            {
                Debug.Log("Path point: " + queue);
            }
            SetPath(path);
        }


        if (pathFinder == null)
        {
            Debug.LogError("Go component is missing on this GameObject");
            return;
        }



        

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

    private IEnumerator MoveCoroutine()
    {
        while (pathQueue.Count > 0)
        {
            Vector2 nextPos = pathQueue.Dequeue();
            Vector2 targetPos = new Vector3(nextPos.x, nextPos.y);

            Debug.Log($"Moving towards: ({nextPos.x}, {nextPos.y})");

            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 2);
                yield return null;
            }

            transform.position = targetPos;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Tile has reached the destination.");
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("module"))
        {
            IsTriggered = true;
            Vector2 collisionPos = other.transform.position;
            for(int i = 0; i < GameManager.row;  i++) {
                for (int j = 0; j < GameManager.col; j++) {
                    Vector2 gameManagerPos = new Vector2(i, j);
                if(gameManagerPos == collisionPos)
                    {
                        this.moduleNum = GameManager.mapArray[i,j].moduleNum;
                        this.currentModule = GameManager.GetModuleInfo(this.moduleNum);

                        break;
                    }
                }
            }
        }
        }


    }
