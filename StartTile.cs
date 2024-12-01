using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartTile : Tile
{
    public static bool start = true;
    public static List<int> signs = new List<int>();
    public static List<string> displayedConsonants = new List<string>();
    //상하좌우 맵핑
    public static Dictionary<int, Vector2> directionMapping = new Dictionary<int, Vector2>
    {
        { 1, Vector2.right },        // →
        { 2, new Vector2(1, -1) },    // ↘
        { 3, Vector2.down },   // ↓
        { 4, new Vector2(-1, -1) },  // ↙
        { 5, Vector2.left },   // ←
        { 6, new Vector2(-1, 1) },   // ↖
        { 7, Vector2.up },     // ↑
        { 8, new Vector2(1, 1) },    // ↗
    };
    
    private Vector2 currentDir => directionMapping[currentDirKey];
    public Vector2 startPos;
    public Vector2 endPos;

    private SpriteRenderer spriteRenderer;
    private int currentDirKey = 1;

    void Start()
    {
        currentDirKey = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (start)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                MoveForward();
                Debug.Log("start set false");
                
            }
        }
    }

    private Tile PrefabAssign()
    {
        Vector2 currentPos = GetPos();
        Tile currentTile = null;

        if (GameManager.MovingTile.TryGetValue(currentPos, out GameObject curtPrefab))
        {
            currentTile = curtPrefab.GetComponent<Tile>();
            Debug.Log("prefab Assign - current pos: " + currentPos + "currentTile: " + currentTile.name);
            currentTile.SetReaded(true);
            return currentTile;
        }

        return currentTile;
    }
    private IEnumerator CheckSign(Tile currentTile)
    {
        if (currentTile != null)
        {
            int signNow = currentTile.isTrueSignHere();
            signs.Add(signNow);
            Debug.Log("add " + signNow);

            switch (signNow)
            {
                case 0:
                    start = true;
                    Debug.Log("set start true");
                    displayedConsonants.Add(Alphabet.FindConsonant(signs));
                    GameManager.gText.text = string.Concat(displayedConsonants);
                    signs.Clear();

                    break;
                case 1:
                    yield return Clock(); break;
                case 2:
                    yield return DoubleClock(); break;
                case 3:
                    yield return UTurn(); break;
                case 4:
                    yield return Counter(); break;
                case 5:
                    yield return DoubleCounter(); break;
                case 6:
                    yield return ReverseUTurn(); break;
                case 7:
                    yield return Diagonal(); break;
                case 8:
                //안쓸거임 pause prefab지우셈 
                case 9: //yield return dot();

                case 10:
                //yield return dot2();

                default:
                    yield return Pause();
                    signs.Add(8);
                    break;
            }
        }
        else { Debug.Log("current sign null"); }
        yield return new WaitForSeconds(0.5f);
    }






    /// <summary>
    /// 회전에 쓰는 이뉴머레이터들 ///
    /// </summary>
    private IEnumerator Clock()
    {
        RotateClock();
        yield return new WaitForSeconds(0.5f);
        MoveForward();
    }

    private IEnumerator DoubleClock()
    {
        RotateClock();
        yield return new WaitForSeconds(0.5f);
        MoveForward();

        yield return new WaitForSeconds(0.5f);
        RotateClock();
        yield return new WaitForSeconds(0.5f);
        MoveForward();
    }

    private IEnumerator UTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            RotateClock();
            yield return new WaitForSeconds(0.3f);
        }
        MoveForward();
    }

    private IEnumerator Counter()
    {
        RotateCounter(); // 반시계 방향으로 90도 회전
        yield return new WaitForSeconds(0.5f);
        MoveForward();
    }

    private IEnumerator Diagonal()
    {
        RotateDiagonal();
        yield return new WaitForSeconds(0.5f);
        MoveForward();
    }

    private IEnumerator DoubleCounter()
    {
        RotateCounter();
        yield return new WaitForSeconds(0.5f);
        MoveForward();

        yield return new WaitForSeconds(0.5f);
        RotateCounter();
        yield return new WaitForSeconds(0.5f);
        MoveForward();
    }

    private IEnumerator ReverseUTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            RotateCounter();
            yield return new WaitForSeconds(0.3f);
        }
        MoveForward();
    }

    private IEnumerator Pause()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.black;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
        }
        yield return new WaitForSeconds(0.25f);
        MoveForward();
    }

    /// <summary>
    /// 회전함수들 ///
    /// </summary>
    void MoveForward()
    {
        startPos = base.GetPos();
        start = false;
        endPos = startPos + currentDir * 1.0f;
        transform.position = endPos;

        StartCoroutine(CheckSign(PrefabAssign()));
    }

    private void RotateClock()
    {
        currentDirKey = currentDirKey + 1 > 8 ? 1 : currentDirKey + 2;
        UpdateSprite();
    }

    private void RotateCounter()
    {
        currentDirKey = currentDirKey - 1 < 1 ? 8 : currentDirKey - 2;
        UpdateSprite();
    }

    private void RotateDiagonal()
    {
        currentDirKey = currentDirKey + 1 > 8 ? 1 : currentDirKey + 1; // 1 → 8까지 순환
        UpdateSprite();
    }

    //스프라이트 회전 함수 
    private void UpdateSprite()
    {
        float rotationAngle = currentDirKey switch
        {
            1 => 0f,   // →
            2 => 45f,  // ↘
            3 => 90f,  // ↓
            4 => 135f, // ↙
            5 => 180f, // ←
            6 => -135f, // ↖
            7 => -90f,  // ↑
            8 => -45f,  // ↗
            _ => 0f    // 기본값
        };

        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }
    //키 변환 함수 
    private void SetDirection(int directionKey)
    {
        if (directionMapping.ContainsKey(directionKey))
            currentDirKey = directionKey;
    }

    //상하좌우 확인 딕셔너리
    public Dictionary<string, Vector2> GetRelativeDirections()
    {
        // 진행 방향에 따라 상대 방향 반환
        return currentDirKey switch
        {
            1 => new Dictionary<string, Vector2>
        { // 진행 방향이 →
            { "Right", Vector2.right },
            { "Up", Vector2.up },
            { "Left", Vector2.left },
            { "Down", Vector2.down },
            { "UpRight", new Vector2(1, 1) },
            { "UpLeft", new Vector2(-1, 1) },
            { "DownRight", new Vector2(1, -1) },
            { "DownLeft", new Vector2(-1, -1) }
        },
            2 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↘
            { "Right", new Vector2(1, -1) },
            { "Up", Vector2.right },
            { "Left", Vector2.down },
            { "Down", Vector2.left },
            { "UpRight", Vector2.right },
            { "UpLeft", Vector2.down },
            { "DownRight", Vector2.left },
            { "DownLeft", Vector2.up }
        },
            3 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↓
            { "Right", Vector2.down },
            { "Up", Vector2.right },
            { "Left", Vector2.up },
            { "Down", Vector2.left },
            { "UpRight", new Vector2(1, -1) },
            { "UpLeft", new Vector2(1, 1) },
            { "DownRight", new Vector2(-1, -1) },
            { "DownLeft", new Vector2(-1, 1) }
        },
            4 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↙
            { "Right", new Vector2(-1, -1) },
            { "Up", Vector2.down },
            { "Left", Vector2.left },
            { "Down", Vector2.up },
            { "UpRight", Vector2.down },
            { "UpLeft", Vector2.left },
            { "DownRight", Vector2.up },
            { "DownLeft", Vector2.right }
        },
            5 => new Dictionary<string, Vector2>
        { // 진행 방향이 ←
            { "Right", Vector2.left },
            { "Up", Vector2.down },
            { "Left", Vector2.right },
            { "Down", Vector2.up },
            { "UpRight", new Vector2(-1, -1) },
            { "UpLeft", new Vector2(-1, 1) },
            { "DownRight", new Vector2(1, -1) },
            { "DownLeft", new Vector2(1, 1) }
        },
            6 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↖
            { "Right", new Vector2(-1, 1) },
            { "Up", Vector2.left },
            { "Left", Vector2.up },
            { "Down", Vector2.right },
            { "UpRight", Vector2.left },
            { "UpLeft", Vector2.up },
            { "DownRight", Vector2.right },
            { "DownLeft", Vector2.down }
        },
            7 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↑
            { "Right", Vector2.up },
            { "Up", Vector2.left },
            { "Left", Vector2.down },
            { "Down", Vector2.right },
            { "UpRight", new Vector2(-1, 1) },
            { "UpLeft", new Vector2(1, 1) },
            { "DownRight", new Vector2(-1, -1) },
            { "DownLeft", new Vector2(1, -1) }
        },
            8 => new Dictionary<string, Vector2>
        { // 진행 방향이 ↗
            { "Right", new Vector2(1, 1) },
            { "Up", Vector2.up },
            { "Left", Vector2.right },
            { "Down", Vector2.down },
            { "UpRight", Vector2.up },
            { "UpLeft", Vector2.right },
            { "DownRight", Vector2.down },
            { "DownLeft", Vector2.left }
        },
            _ => new Dictionary<string, Vector2>
        { // 기본값
            { "Right", Vector2.right },
            { "Up", Vector2.up },
            { "Left", Vector2.left },
            { "Down", Vector2.down },
            { "UpRight", new Vector2(1, 1) },
            { "UpLeft", new Vector2(-1, 1) },
            { "DownRight", new Vector2(1, -1) },
            { "DownLeft", new Vector2(-1, -1) }
        }
        };

    }
}
