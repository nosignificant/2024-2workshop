using System.Collections;
using UnityEngine;

public class Push : Tile
{
   /*public bool isTrigger = false; // Ÿ���� �÷��̾ ���� �и��� ����
    private Vector2 direction; // �и��� ����
    public int thisSign = 1; // ���� Ÿ���� sign ��

    private Tile currentTile; // ���� ��ġ�� Ÿ�� ����

    void Start()
    {
        base.sign[thisSign] = true;
        UpdateCurrentTile(); // ���� �� ���� Ÿ�� ����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTrigger = true;
            direction = PlayerMove.lastDirection; // �÷��̾��� ������ ������ ������
        }
    }

    void Update()
    {
        if (isTrigger)
        {
            StartCoroutine(Move(direction));
            isTrigger = false; // �� ���� �����ϵ��� �÷��� �ʱ�ȭ
        }
    }

    private IEnumerator Move(Vector2 playerMove)
    {
        if (currentTile != null)
        {
            currentTile.SetAllFalseSign();
        }

        int currentX = (int)base.GetPos().x; // ���� X �ε���
        int currentY = (int)base.GetPos().y; // ���� Y �ε���

        int targetX = currentX + (int)playerMove.x; // �̵� �� X �ε���
        int targetY = currentY + (int)playerMove.y; // �̵� �� Y �ε���

        if (targetX >= 0 && targetX < GameManager.row && targetY >= 0 && targetY < GameManager.col)
        {
            Tile targetTile = GameManager.mapArray[targetX, targetY];

            if (targetTile != null)
            {
                transform.position = new Vector3(targetTile.transform.position.x, targetTile.transform.position.y, 6);
                UpdateCurrentTile();
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    private void UpdateCurrentTile()
    {
        // ���� Ÿ���� �����ϰ� sign ������Ʈ
        currentTile = GameManager.mapArray[(int)base.GetPos().x, (int)base.GetPos().y];
        if (currentTile != null)
        {
            currentTile.SetAllFalseSign(); // ��� sign�� �ʱ�ȭ
            currentTile.SetIsSign(thisSign, true); // ���� Ÿ�ϸ� Ȱ��ȭ
        }
    }*/
}
