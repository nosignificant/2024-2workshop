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

        // ���� �Ǵ� ���� �������θ� �����̵��� ����
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

        // �̵��� �������� �˸�
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

                // thisPos�� tilePos�� ������ ��ġ�� �ִ��� Ȯ��
                float distanceX = Mathf.Abs(thisPos.x - tilePos.x);
                float distanceY = Mathf.Abs(thisPos.y - tilePos.y);
                float threshold = 0.3f; // ������ �����Ͽ� ��� ���� �������� Ȯ��

                if (distanceX <= threshold && distanceY <= threshold)
                {
                    // ������ ��� thisPos�� tilePos�� �̵�
                    this.transform.position = new Vector3(tilePos.x, tilePos.y, this.transform.position.z);
                    return; // ���� ����� Ÿ�Ϸ� �̵� �� ����
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
