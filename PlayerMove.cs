
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameObject square;
    public GameObject moveParent;
    private SpriteRenderer spriteRenderer;

    private bool readyToMove = true;
    public static int playerMoveX, playerMoveY;
    Vector2 startPos;
    Vector2 endPos;
    public static Vector2 playerMove;
    public static Vector2 lastDirection = Vector2.zero;

    void Start()
    {
        square = GameObject.Find("Square");
        moveParent = GameObject.Find("moveParent");
        StartCoroutine(BlinkCursor());
    }
    void Update()
    {
        if (readyToMove)
        {
            playerMove = ApplyUserInput();

            if (playerMove != Vector2.zero)
            {
                readyToMove = false;
                lastDirection = playerMove; 
                StartCoroutine(Move(playerMove));
            }
        }
    }

    private IEnumerator Move(Vector2 playerMove)
    {
        Vector2 currentPos = this.transform.position;
        Vector2 targetPos = currentPos + playerMove;

        if (IsValidPosition(targetPos))
        {
            yield return null;
            this.transform.position = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);
        }
        else
        {
            Debug.Log($"Cannot move {this.name} to {targetPos} (invalid position or already occupied).");
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        readyToMove = true;
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

                return withinBounds && !obstacles;
            }
            else
                Debug.LogError("Square object does not have a BoxCollider2D.");
        }

        return false; // 기본적으로 false 반환
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

    private IEnumerator BlinkCursor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            Color blinkColor = Color.white; // 깜빡일 때의 색상

            while (true) // 무한 반복
            {
                spriteRenderer.color = blinkColor;
                yield return new WaitForSeconds(0.5f); // 0.5초 대기
                spriteRenderer.color = originalColor;
                yield return new WaitForSeconds(0.5f); // 0.5초 대기
            }
        }
    }
}
