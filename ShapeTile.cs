using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        Vector2 newEndPos;
        Tile targetTile;
        ModuleTile previousTile = GameManager.SearchTile(currentPos) as ModuleTile;  // 이전 위치의 ModuleTile

        do
        {
            int x = 0, y = 0;
            int randDir = Random.Range(0, 4);

            switch (randDir)
            {
                case 0: y = 1; break;   // Up
                case 1: y = -1; break;  // Down
                case 2: x = 1; break;   // Right
                case 3: x = -1; break;  // Left
            }
            newEndPos = currentPos + new Vector2(x, y) * 1.0f;

            targetTile = GameManager.SearchTile(newEndPos);
            if (targetTile == null)
            {
                Debug.Log("null reference");
                break;
            }
        }
        while (targetTile == null || targetTile.moduleNum != currentModule.moduleNum);

        yield return new WaitForSeconds(1.0f);

        this.transform.position = newEndPos;
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
                Destroy(collisionOp);
                break;
            case "or":
                Destroy(collisionOp);
                break;
            case "canGO":
                canGO = true;
                Debug.Log("cnaGo");
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

    public Vector2 SetRandEndPos()
    {
        int size = currentModule.size;
        int startX = currentModule.startX;
        int startY = currentModule.startY;
        if (currentPos != new Vector2(startX, startY)) {
        return new Vector2(startX + size, startY + size);
        }
        else { return new Vector2(startX, startY); }
    }
}
