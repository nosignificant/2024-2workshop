// PlayerMove.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Tile
{
    private bool readyToMove = true;
    public static int playerMoveX, playerMoveY;
    public static Vector2 playerMove;
    Vector2 startPos;
    Vector2 endPos;

    // 전역으로 유지될 변수
    public static Vector2 lastDirection = Vector2.zero;

    void Update()
    {
        if (readyToMove)
        {
            playerMove = ApplyUserInput();

            // 이동 입력이 있을 경우에만 이동 시도
            if (playerMove != Vector2.zero)
            {
                readyToMove = false;
                lastDirection = playerMove; // 마지막 입력된 방향 저장
                StartCoroutine(Move(playerMove));
            }
        }

        SearchPos();
    }

    public static Vector2 ApplyUserInput()
    {
        playerMoveX = 0;
        playerMoveY = 0;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerMoveY = 1; // 위쪽으로 이동
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerMoveX = 1; // 오른쪽으로 이동
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerMoveY = -1; // 아래쪽으로 이동
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerMoveX = -1; // 왼쪽으로 이동
        }
        return new Vector2(playerMoveX, playerMoveY);
    }

    private IEnumerator Move(Vector2 playerMove)
    {
        startPos = base.GetPos();
        endPos = startPos + playerMove * 1.0f;
        this.transform.position = endPos;

        yield return new WaitForSeconds(0.1f);
        readyToMove = true;
    }

    public void SearchPos()
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
                    this.transform.position = new Vector3(tilePos.x, tilePos.y, this.transform.position.z);
                    return;
                }
            }
        }
    }
}
