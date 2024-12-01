using System.Collections;
using UnityEngine;

public class Push : MonoBehaviour
{
    public bool isTrigger = false; // 타일이 플레이어에 의해 밀리는 상태
    private Vector2 direction; // 밀리는 방향
    public bool isMoving;
    GameObject square;

     private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             isTrigger = true;
             direction = PlayerMove.lastDirection;
            StartCoroutine(Move(direction));
        }
     }

    private void Start()
    {
        square = GameObject.Find("Square");
    }


    //push 이동 
     private IEnumerator Move(Vector2 playerMove)
     {
        isTrigger = false;
        Vector2 currentPos = this.transform.position;
       Vector2 targetPos = currentPos + playerMove;
        //Debug.Log(this.name + "currentPos : " + currentPos + "targetPos : " + targetPos);

        if (IsValidPosition(targetPos)) {
            yield return null;
            this.transform.position = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);
            //딕셔너리 업데이트
            GameManager.MovingTile.Clear(); // 기존 딕셔너리 비우기
            GameManager.RegisterMovingTiles(); // 모든 타일 다시 등록
        }
        else
        {
            //Debug.Log($"Cannot move {this.name} to {targetPos} (invalid position or already occupied).");
            yield break; // 이동 중단
        }
    }


    private bool IsValidPosition(Vector2 pos)
    {
        if (square != null)
        {
            BoxCollider2D squareBounds = square.GetComponent<BoxCollider2D>();
            if (squareBounds != null)
            {
                bool withinBounds = (pos.x >= squareBounds.bounds.min.x && pos.x <= squareBounds.bounds.max.x) &&
                    (pos.y >= squareBounds.bounds.min.y && pos.y <= squareBounds.bounds.max.y);
                bool obstacles = GameManager.ObstacleTiles.ContainsKey(pos);
                bool Occupied = GameManager.MovingTile.ContainsKey(pos);

                return withinBounds && !Occupied && !obstacles;
            }
            else
                Debug.LogError("Square object does not have a BoxCollider2D.");
        }

        return false; // 기본적으로 false 반환
    }
}
