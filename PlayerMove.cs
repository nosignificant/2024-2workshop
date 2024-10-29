using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Tile
{
    private bool readyToMove = true;
    private bool isTriggered = false;
    public float moveDistance = 1.0f;
    public static Vector2 moveInput;
    public static int playerMoveX, playerMoveY;
    Vector2 startPos;
    Vector2 endPos;

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
                                        Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();

        if (moveInput.sqrMagnitude > 0.5f && readyToMove)
        {
            readyToMove = false;
            StartCoroutine(Move(moveInput)); 
        }
        SearchPos();
    }

    private IEnumerator Move(Vector2 direction)
    {
        playerMoveX = 0;
        playerMoveY = 0;
        startPos = base.GetPos();

        // 수평 또는 수직 방향으로만 움직이도록 설정
        if (Mathf.Abs(direction.x) < 0.5f)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction.Normalize();

        if (direction.x > 0 && direction.y == 0) { playerMoveX = 1; }
        else if (direction.x < 0 && direction.y == 0) { playerMoveX = -1; }

        else if (direction.y > 0 && direction.x == 0) { playerMoveY = 1; }
        else if (direction.y < 0 && direction.x == 0) { playerMoveY = -1; }

        else if (direction.y > 0 && direction.x > 0) { playerMoveX = 1;  playerMoveY = 1; }

        endPos = startPos + direction * moveDistance;
        transform.position = endPos;
        //CheckMove();

        yield return new WaitForSeconds(0.1f);

        // 이동이 끝났음을 알림
        readyToMove = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.FindWithTag("module"))
        {
            isTriggered = true;
        }
    }

    void SearchPos()
    {
        for (int i = 0; i < GameManager.row; i++)
        {
            for (int j = 0; j < GameManager.col; j++)
            {
                Tile tileComponent = GameManager.mapArray[i, j].GetComponent<Tile>();
                Vector2 tilePos = tileComponent.transform.position;
                Vector2 thisPos = this.transform.position;

                // thisPos가 tilePos와 근접한 위치에 있는지 확인
                float distanceX = Mathf.Abs(thisPos.x - tilePos.x);
                float distanceY = Mathf.Abs(thisPos.y - tilePos.y);
                float threshold = 0.3f; // 범위를 설정하여 어느 정도 근접한지 확인

                if (distanceX <= threshold && distanceY <= threshold)
                {
                    // 근접한 경우 thisPos를 tilePos로 이동
                    this.transform.position = new Vector3(tilePos.x, tilePos.y, this.transform.position.z);
                    return; // 가장 가까운 타일로 이동 후 종료
                }
            }
        }
    }

    void CheckMove()
    {
        int x, y;
        for (int i = 0; i < GameManager.row; i++)
        {
            for (int j = 0; j < GameManager.col; j++)
            {
                Tile tileComponent = GameManager.mapArray[i, j].GetComponent<Tile>();
                x = tileComponent.GetTileNumX();
                y = tileComponent.GetTileNumY();
            }
        }
    }
}
