// MovableTile.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : Tile
{
    public bool isCollision = false;
    Vector2 startPos;
    Vector2 endPos;
    Vector2 direction;
    public int thisA = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollision = true;
            direction = PlayerMove.lastDirection; // ������ ���� ���
        }
    }

    void Update()
    {
        if (isCollision)
        {
            StartCoroutine(Move(direction));
            isCollision = false;
        }
        SearchPos();
    }

    private IEnumerator Move(Vector2 playerMove)
    {
        startPos = base.GetPos();
        endPos = startPos + playerMove * 1.0f;
        transform.position = endPos;

        yield return new WaitForSeconds(0.1f);
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
                    this.transform.position = new Vector3(tilePos.x, tilePos.y, 6   );
                    return;
                }
            }
        }
    }
}
