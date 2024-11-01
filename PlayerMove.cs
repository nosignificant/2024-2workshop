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

    // �������� ������ ����
    public static Vector2 lastDirection = Vector2.zero;

    void Update()
    {
        if (readyToMove)
        {
            playerMove = ApplyUserInput();

            // �̵� �Է��� ���� ��쿡�� �̵� �õ�
            if (playerMove != Vector2.zero)
            {
                readyToMove = false;
                lastDirection = playerMove; // ������ �Էµ� ���� ����
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
            playerMoveY = 1; // �������� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerMoveX = 1; // ���������� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerMoveY = -1; // �Ʒ������� �̵�
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerMoveX = -1; // �������� �̵�
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

                // thisPos�� tilePos�� ������ ��ġ�� �ִ��� Ȯ��
                float distanceX = Mathf.Abs(thisPos.x - tilePos.x);
                float distanceY = Mathf.Abs(thisPos.y - tilePos.y);
                float threshold = 0.3f; // ������ �����Ͽ� ��� ���� �������� Ȯ��

                if (distanceX <= threshold && distanceY <= threshold)
                {
                    this.transform.position = new Vector3(tilePos.x, tilePos.y, this.transform.position.z);
                    return;
                }
            }
        }
    }
}
