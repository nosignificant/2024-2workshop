using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class ShapeTile : Tile
{
    public Stack<int> moveDirectionStack = new Stack<int>();
    private Queue<Vector2> pathQueue = new Queue<Vector2>();
    private AudioSource audioSource;
    private List<ShapeTile> sameATiles;

    public bool hasLOp = false;
    public bool IsTriggered = false;
    public bool canGO = false;
    private bool isMoving = false;

    public GameObject collisionOp;

    Module currentModule;
    Vector2 currentPos;
    Vector2 endPos;
    public int thisA = 65;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        List<ShapeTile> sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
    }
    void Update()
    {
        currentPos = this.GetPos();
    }
    public void StartMoveSignal()
    {
        if (!isMoving && canGO)
        {
            isMoving = true;
            StartCoroutine(MoveCoroutine());

            List<ShapeTile> sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
            foreach (ShapeTile tile in sameATiles)
            {
                if (tile != this && !tile.isMoving)
                {
                    tile.RecieveSignal();
                }
            }
        }
    }
    public void RecieveSignal()
    {
        if (!isMoving)
            this.canGO = true;
    }

    public void RecieveStopSignal()
    {
        StopAllCoroutines();
        canGO = false;
    }
    private IEnumerator MoveCoroutine()
    {
        int randDir;
        Vector2 newEndPos;
        ModuleTile targetTile;
        ModuleTile previousTile = currentModule.SearchModuleTile(moduleNum, currentPos);

        while (true)
        {
            if (!currentModule.CheckForUnvisitedNeighbors(currentPos))
            {
                //Debug.Log("Resetting all tiles to unvisited as no unvisited neighbors found.");
                currentModule.ResetVisited();
                ShapeTileManager.ResetTilesIfConditionMet();
            }

            int x = 0, y = 0;
            randDir = Random.Range(0, 4);

            switch (randDir)
            {
                case 0: y = 1; break;   // Up
                case 1: y = -1; break;  // Down
                case 2: x = 1; break;   // Right
                case 3: x = -1; break;  // Left
            }

            newEndPos = currentPos + new Vector2(x, y) * 1.0f;
            targetTile = currentModule.SearchModuleTile(moduleNum, newEndPos);

            if (targetTile != null && targetTile.isModule && !targetTile.visited)
            {
                float frequency = CalculateFrequency(thisA, newEndPos);
                audioSource.pitch = frequency / 440.0f;
                audioSource.Play();
                break; // 유효한 타일을 찾았으면 루프 종료
            }
        }
        Debug.Log("target tile module num: " + targetTile.moduleNum);
        yield return new WaitForSeconds(0.3f);
        audioSource.Play();  ///////////////////뭔가 여기 버그있는 것 같은데 몰르겠당... 

        this.transform.position = newEndPos;
        targetTile.visited = true;
        if (ShapeTileManager.OpSign[0] || ShapeTileManager.OpSign[1]) {
            Debug.Log("move direction recorded:" + randDir);
            moveDirectionStack.Push(randDir); }
        

        if (previousTile != null)
            StartCoroutine(previousTile.FlashWhite(0.1f));

        isMoving = false;
    }

    private float CalculateFrequency(int thisA, Vector2 position)
    {
        return 220.0f + (thisA * 10) + (position.x - position.y) * 100.0f;
    }

    public void CollisionOp(GameObject gameObject)
    {
        string name = gameObject.name;
        Debug.Log("collision operator name is " + name);
        if(sameATiles != null) 
            foreach (ShapeTile tile in sameATiles) { tile.RecieveStopSignal(); }
        switch (name)
            {   
                case "plus":
                    thisA++;
                    CheckThisA();
                break;

                case "minus":
                    thisA--;
                    CheckThisA();
                break;

                case "and":
                if (ShapeTileManager.OpSign[1])
                    ShapeTileManager.OpSign[1] = false;

                ShapeTileManager.OpSign[0] = true;
                hasLOp = true;
                Debug.Log("and met, set OpSign[0] true");
                break;

                case "or":
                    if (ShapeTileManager.OpSign[0])
                    ShapeTileManager.OpSign[0] = false;

                ShapeTileManager.OpSign[1] = true;
                Debug.Log("OR met, set OpSign[1] true");
                hasLOp = true;

                    break;
            }
        Destroy(gameObject);
        sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("module"))
        {
            IsTriggered = true;
            Vector2 collisionPos = other.transform.position;
            for(int i = 0; i < GameManager.row;  i++) {
                for (int j = 0; j < GameManager.col; j++) {
                    Vector2 gameManagerPos = new Vector3(i, j, 5);
                if(gameManagerPos == collisionPos)
                    {
                        this.moduleNum = GameManager.mapArray[i,j].moduleNum;
                        this.currentModule = GameManager.GetModuleInfo(this.moduleNum);

                        break;
                    }
                }
            }
        }
        else if(other.gameObject.name == "canGO")
        {
            canGO = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("operator"))
        {
            if (other != null)
            {
                
                if (other.gameObject.name != "canGO")
                {
                    Debug.Log("Destroy Operator");
                    CollisionOp(other.gameObject);
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "canGO")
        {
            sameATiles = ShapeTileManager.GetTilesWithSameA(thisA);
            foreach (ShapeTile tile in sameATiles)
                tile.RecieveStopSignal();
            canGO = false;
        }
    }

    public void ClearMoveHistory()
    {
        moveDirectionStack.Clear();
    }

    private void Awake()
    {
        ShapeTileManager.RegisterShapeTile(this);
    }

    private void OnDestroy()
    {
        ShapeTileManager.UnregisterShapeTile(this);
    }

    public void CheckThisA()
    {
        if (thisA > 90) thisA = 65;
        else if (thisA < 65) thisA = 90;
    }

}
