using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro.Examples;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartTile : Tile
{
    public static bool start = true;
    public static List<int> signs = new List<int>();
    public static List<string> displayedConsonants = new List<string>();
    //�����¿� ����
    public static Dictionary<int, Vector2> directionMapping = new Dictionary<int, Vector2>
    {
        { 1, Vector2.right },        // ��
        { 2, new Vector2(1, -1) },    // ��
        { 3, Vector2.down },   // ��
        { 4, new Vector2(-1, -1) },  // ��
        { 5, Vector2.left },   // ��
        { 6, new Vector2(-1, 1) },   // ��
        { 7, Vector2.up },     // ��
        { 8, new Vector2(1, 1) },    // ��
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
        if (start && Input.GetKeyUp(KeyCode.Space))
                MoveForward();
    }

    // Ÿ�� �Ҵ� �Լ�
    private Tile PrefabAssign()
    {
        Vector2 currentPos = GetPos();
        Tile currentTile = null;

        if (GameManager.MovingTile.TryGetValue(currentPos, out GameObject curtPrefab))
        {
            currentTile = curtPrefab.GetComponent<Tile>();
            //Debug.Log("prefab Assign - current pos: " + currentPos + "currentTile: " + currentTile.name);
            currentTile.SetReaded(true);
            return currentTile;
        }

        return currentTile;
    }

    //sign Ȯ�� �Լ� 
    private IEnumerator CheckSign(Tile currentTile)
    {
        if (currentTile != null)
        {
            int signNow = currentTile.isTrueSignHere();
            signs.Add(signNow);

            switch (signNow)
            {
                case 0:
                    start = true;
                    Debug.Log("start: " + start);
                    GameManager.space.color = UnityEngine.Color.white;
                    if(Alphabet.FindConsonant(signs) != null)
                    {
                        GameManager.gText.text += Alphabet.FindConsonant(signs);
                        Debug.Log("now string: " + GameManager.gText.text);
                    }
                    yield return new WaitForSeconds(0.5f);  
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
                //�Ⱦ����� pause prefab����� 
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
    /// ȸ���� ���� �̴��ӷ����͵� ///
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
        RotateCounter(); // �ݽð� �������� 90�� ȸ��
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
            UnityEngine.Color originalColor = spriteRenderer.color;
            spriteRenderer.color = UnityEngine.Color.black;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
        }
        yield return new WaitForSeconds(0.25f);
        MoveForward();
    }

    /// <summary>
    /// ȸ���Լ��� ///
    /// </summary>
    void MoveForward()
    {
        startPos = base.GetPos();
        start = false;
        Debug.Log("start: " + start);
        GameManager.space.color = UnityEngine.Color.gray;
        endPos = startPos + currentDir * 1.0f;
        transform.position = endPos;

        StartCoroutine(CheckSign(PrefabAssign()));
    }

    private void RotateClock()
    {
        currentDirKey = currentDirKey + 1 > 7 ? 1 : currentDirKey + 2;
        UpdateSprite();
    }

    private void RotateCounter()
    {
        currentDirKey = currentDirKey - 1 < 1 ? 7 : currentDirKey - 2;
        UpdateSprite();
    }

    private void RotateDiagonal()
    {
        currentDirKey = currentDirKey + 1 > 8 ? 1 : currentDirKey + 1; // 1 �� 8���� ��ȯ
        UpdateSprite();
    }

    //��������Ʈ ȸ�� �Լ� 
    private void UpdateSprite()
    {
        float rotationAngle = currentDirKey switch
        {
            1 => 0f,   // ��
            2 => 45f,  // ��
            3 => 90f,  // ��
            4 => 135f, // ��
            5 => 180f, // ��
            6 => -135f, // ��
            7 => -90f,  // ��
            8 => -45f,  // ��
            _ => 0f    // �⺻��
        };

        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }
    //Ű ��ȯ �Լ� 
    private void SetDirection(int directionKey)
    {
        if (directionMapping.ContainsKey(directionKey))
            currentDirKey = directionKey;
    }

    //�����¿� Ȯ�� ��ųʸ�
    public Dictionary<string, Vector2> GetRelativeDirections(int range = 1)
    {
        // ���� ���⿡ ���� ��� ���� ��ȯ, range�� Ȯ���� �Ÿ�
        return currentDirKey switch
        {
            1 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", Vector2.right * range },
            { "Up", Vector2.up * range },
            { "Left", Vector2.left * range },
            { "Down", Vector2.down * range },
            { "UpRight", new Vector2(1, 1) * range },
            { "UpLeft", new Vector2(-1, 1) * range },
            { "DownRight", new Vector2(1, -1) * range },
            { "DownLeft", new Vector2(-1, -1) * range }
        },
            2 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", new Vector2(1, -1) * range },
            { "Up", Vector2.right * range },
            { "Left", Vector2.down * range },
            { "Down", Vector2.left * range },
            { "UpRight", Vector2.right * range },
            { "UpLeft", Vector2.down * range },
            { "DownRight", Vector2.left * range },
            { "DownLeft", Vector2.up * range }
        },
            3 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", Vector2.down * range },
            { "Up", Vector2.right * range },
            { "Left", Vector2.up * range },
            { "Down", Vector2.left * range },
            { "UpRight", new Vector2(1, -1) * range },
            { "UpLeft", new Vector2(1, 1) * range },
            { "DownRight", new Vector2(-1, -1) * range },
            { "DownLeft", new Vector2(-1, 1) * range }
        },
            4 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", new Vector2(-1, -1) * range },
            { "Up", Vector2.down * range },
            { "Left", Vector2.left * range },
            { "Down", Vector2.up * range },
            { "UpRight", Vector2.down * range },
            { "UpLeft", Vector2.left * range },
            { "DownRight", Vector2.up * range },
            { "DownLeft", Vector2.right * range }
        },
            5 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", Vector2.left * range },
            { "Up", Vector2.down * range },
            { "Left", Vector2.right * range },
            { "Down", Vector2.up * range },
            { "UpRight", new Vector2(-1, -1) * range },
            { "UpLeft", new Vector2(-1, 1) * range },
            { "DownRight", new Vector2(1, -1) * range },
            { "DownLeft", new Vector2(1, 1) * range }
        },
            6 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", new Vector2(-1, 1) * range },
            { "Up", Vector2.left * range },
            { "Left", Vector2.up * range },
            { "Down", Vector2.right * range },
            { "UpRight", Vector2.left * range },
            { "UpLeft", Vector2.up * range },
            { "DownRight", Vector2.right * range },
            { "DownLeft", Vector2.down * range }
        },
            7 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", Vector2.up * range },
            { "Up", Vector2.left * range },
            { "Left", Vector2.down * range },
            { "Down", Vector2.right * range },
            { "UpRight", new Vector2(-1, 1) * range },
            { "UpLeft", new Vector2(1, 1) * range },
            { "DownRight", new Vector2(-1, -1) * range },
            { "DownLeft", new Vector2(1, -1) * range }
        },
            8 => new Dictionary<string, Vector2>
        { // ���� ������ ��
            { "Right", new Vector2(1, 1) * range },
            { "Up", Vector2.up * range },
            { "Left", Vector2.right * range },
            { "Down", Vector2.down * range },
            { "UpRight", Vector2.up * range },
            { "UpLeft", Vector2.right * range },
            { "DownRight", Vector2.down * range },
            { "DownLeft", Vector2.left * range }
        },
            _ => new Dictionary<string, Vector2>
        { // �⺻��
            { "Right", Vector2.right * range },
            { "Up", Vector2.up * range },
            { "Left", Vector2.left * range },
            { "Down", Vector2.down * range },
            { "UpRight", new Vector2(1, 1) * range },
            { "UpLeft", new Vector2(-1, 1) * range },
            { "DownRight", new Vector2(1, -1) * range },
            { "DownLeft", new Vector2(-1, -1) * range }
        }
        };
    }

}
