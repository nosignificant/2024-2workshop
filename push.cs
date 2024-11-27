using System.Collections;
using UnityEngine;

public class Push : MonoBehaviour
{
    public bool isTrigger = false; // Ÿ���� �÷��̾ ���� �и��� ����
    private Vector2 direction; // �и��� ����
    GameObject square;

     private void OnTriggerEnter2D(Collider2D other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             isTrigger = true;
             direction = PlayerMove.lastDirection;
         }
     }

    private void Start()
    {
        square = GameObject.Find("Square");
    }

    void Update()
     {
         if (isTrigger && !GameManager.isTutorial)
         {
             StartCoroutine(Move(direction));
             isTrigger = false;
         }
     }

    //push �̵� 
     private IEnumerator Move(Vector2 playerMove)
     {
       Vector2 currentPos = this.transform.position;
       Vector2 targetPos = currentPos + playerMove;
        Debug.Log("currentPos : " + currentPos);
        Debug.Log("targetPos : " + targetPos);

        if (IsValidPosition(targetPos)) {
            yield return null;
            this.transform.position = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);
            //��ųʸ� ������Ʈ
            GameManager.MovingTile.Clear(); // ���� ��ųʸ� ����
            GameManager.RegisterMovingTiles(); // ��� Ÿ�� �ٽ� ���
        }
        else
        {
            Debug.Log($"Cannot move {this.name} to {targetPos} (invalid position or already occupied).");
            yield break; // �̵� �ߴ�
        }
        Debug.Log($"{this.name} moved to {targetPos}");
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

        return false; // �⺻������ false ��ȯ
    }
}
