
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject[] rotatePrefabs = new GameObject[3]; // 1 2 3 
    public GameObject[] counterPrefabs = new GameObject[3]; // 4 5 6
    public GameObject diagonal; // 7
    public GameObject[] paustop = new GameObject[2]; // 8 0
    public GameObject[] dotPrefabs = new GameObject[2]; // 9 10
    public static Dictionary<Vector2, GameObject> tilePrefabs = new Dictionary<Vector2, GameObject>();

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
        PlayerNumInput(this.transform.position);
        if (readyToMove)
        {
            playerMove = ApplyUserInput();

            // 이동 입력이 있을 경우에만 이동 시도
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
    public void PlayerNumInput(Vector2 pos)
    {
        GameObject existingPrefab = null;
        GameObject prefabToInstantiate = null;

        tilePrefabs.TryGetValue(pos, out existingPrefab);
        if (GameManager.isTutorial)
        {
            if (Input.GetKeyDown(KeyCode.E)) //E
            {
                Debug.Log("Alpha1 pressed");
                prefabToInstantiate = GetNextPrefab(rotatePrefabs, existingPrefab != null ? existingPrefab.GetComponent<Tile>().isTrueSignHere() : 0);
                HandlePrefab(pos, existingPrefab);
            }
            else if (Input.GetKeyDown(KeyCode.Q)) //Q
            {
                Debug.Log("Alpha2 pressed");
                prefabToInstantiate = GetNextPrefab(counterPrefabs, existingPrefab != null ? existingPrefab.GetComponent<Tile>().isTrueSignHere() : 0);
                HandlePrefab(pos, existingPrefab);
            }
            else if (Input.GetKeyDown(KeyCode.R)) //R
            {
                prefabToInstantiate = diagonal;
                HandlePrefab(pos, existingPrefab);
            }
            else if (Input.GetKeyDown(KeyCode.F)) //Space
            {
                prefabToInstantiate = GetNextPrefab(paustop, existingPrefab != null ? existingPrefab.GetComponent<Tile>().isTrueSignHere() : 0);
                HandlePrefab(pos, existingPrefab);
            }
            //프리팹 활성화 코드 
            if (prefabToInstantiate != null)
            {
                GameObject newPrefab = Instantiate(prefabToInstantiate, moveParent.transform);
                newPrefab.transform.position = new Vector3(pos.x, pos.y, 0);
                Debug.Log("Instantiated " + prefabToInstantiate.name + " at position " + pos);
                tilePrefabs[pos] = newPrefab;

                foreach (var kvp in tilePrefabs)
                {
                    Debug.Log($"Tile prefab at {kvp.Key}: {kvp.Value.name}");
                }
            }
 
        }
    }


    private GameObject GetNextPrefab(GameObject[] prefabArray, int currentSign)
    {
        if (currentSign == 0) return prefabArray[0];
        for (int i = 0; i < prefabArray.Length; i++)
        {
            if (prefabArray[i].GetComponent<Tile>().isTrueSignHere() == currentSign)
                return i + 1 < prefabArray.Length ? prefabArray[i + 1] : null;
        }
        return null;
    }

    private void HandlePrefab(Vector2 pos, GameObject existingPrefab)
    {
        if (existingPrefab != null)
        {
            Debug.Log("Destroying " + existingPrefab.name);
            Destroy(existingPrefab);
            tilePrefabs.Remove(pos);
        }
    }

    private IEnumerator BlinkCursor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            Color blinkColor = Color.black; // 깜빡일 때의 색상

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
