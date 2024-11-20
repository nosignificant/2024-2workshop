
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class PlayerMove : Tile
{
    public GameObject[] rotatePrefabs = new GameObject[3]; // 1 2 3 
    public GameObject[] counterPrefabs = new GameObject[3]; // 4 5 6
    public GameObject diagonal; // 7
    public GameObject[] paustop = new GameObject[2]; // 8 
    public GameObject[] dotPrefabs = new GameObject[2]; // 9 10

    public GameObject moveParent;

    public static int playerMoveX, playerMoveY;
    public static Vector2 playerMove;
    private SpriteRenderer spriteRenderer;
    private bool readyToMove = true;

    Vector2 startPos;
    Vector2 endPos;

    public static Dictionary<Vector2, GameObject> tilePrefabs = new Dictionary<Vector2, GameObject>();
    public static Vector2 lastDirection = Vector2.zero;

    void Start()
    {
        moveParent = GameObject.Find("moveParent");
        StartCoroutine(BlinkCursor());
    }
    void Update()
    {
        PlayerNumInput(base.GetPos());
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

        int targetX = (int)base.GetPos().x + (int)playerMove.x;
        int targetY = (int)base.GetPos().y + (int)playerMove.y;

        Vector3 newPosition = new Vector3(targetX, targetY, 1);
        transform.position = newPosition;

        yield return new WaitForSeconds(0.1f);
        readyToMove = true;
    }

  /*  bool isOccupiedFar(Vector2 farPos)
    {
        bool[] farBool = GameManager.mapArray[(int)farPos.x, (int)farPos.y].GetIsSign();
        foreach (bool flag in farBool)
        {
            if (flag)
                return true;
        }
        return false;
    }*/

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
