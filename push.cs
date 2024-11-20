using System.Collections;
using UnityEngine;

public class Push : Tile
{
   /*public bool isTrigger = false; // 타일이 플레이어에 의해 밀리는 상태
    private Vector2 direction; // 밀리는 방향
    public int thisSign = 1; // 현재 타일의 sign 값

    private Tile currentTile; // 현재 위치한 타일 참조

    void Start()
    {
        base.sign[thisSign] = true;
        UpdateCurrentTile(); // 시작 시 현재 타일 갱신
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTrigger = true;
            direction = PlayerMove.lastDirection; // 플레이어의 마지막 방향을 가져옴
        }
    }

    void Update()
    {
        if (isTrigger)
        {
            StartCoroutine(Move(direction));
            isTrigger = false; // 한 번만 실행하도록 플래그 초기화
        }
    }

    private IEnumerator Move(Vector2 playerMove)
    {
        if (currentTile != null)
        {
            currentTile.SetAllFalseSign();
        }

        int currentX = (int)base.GetPos().x; // 현재 X 인덱스
        int currentY = (int)base.GetPos().y; // 현재 Y 인덱스

        int targetX = currentX + (int)playerMove.x; // 이동 후 X 인덱스
        int targetY = currentY + (int)playerMove.y; // 이동 후 Y 인덱스

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
        // 현재 타일을 갱신하고 sign 업데이트
        currentTile = GameManager.mapArray[(int)base.GetPos().x, (int)base.GetPos().y];
        if (currentTile != null)
        {
            currentTile.SetAllFalseSign(); // 모든 sign을 초기화
            currentTile.SetIsSign(thisSign, true); // 현재 타일만 활성화
        }
    }*/
}
