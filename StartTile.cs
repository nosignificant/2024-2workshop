using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StartTile : Tile
{
    public static List<int> signs = new List<int>();
    public static List<string> displayedConsonants = new List<string>();

    public Vector2 currentDir;
    public Vector2 startPos;
    public Vector2 endPos;

    private SpriteRenderer spriteRenderer;
    private bool start = true;
    private bool isMoving = false; // ������ ���� �÷���

    void Start()
    {
        currentDir = Vector2.right;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start)
            {
                MoveForward();
                start = false;
            }
        }

        if (!start && !isMoving)
            StartCoroutine(CheckSign(PrefabAssign()));
    }

    private Tile PrefabAssign()
    {
        Vector2 currentPos = base.GetPos();
        Tile currentTile = null; // null�� �ʱ�ȭ

        if (GameManager.isTutorial && PlayerMove.tilePrefabs.TryGetValue(currentPos, out GameObject currentPrefab))
        {
            currentTile = currentPrefab.GetComponent<Tile>();
            return currentTile;
        }
        else
        {
            if (GameManager.MovingTile.TryGetValue(currentPos, out GameObject curtPrefab))
            {
                currentTile = curtPrefab.GetComponent<Tile>();
                return currentTile;
            }
        }

        return currentTile;
    }
    private IEnumerator CheckSign(Tile currentTile)
    {
        isMoving = true; // ������ ���� �÷��� ��

        if (currentTile != null)
        {
            int signNow = currentTile.isTrueSignHere();
            switch (signNow)
            {
                case 0:
                    start = true;
                    if (Alphabet.FindConsonant(signs) != null)
                    {
                        displayedConsonants.Add(Alphabet.FindConsonant(signs));
                        GameManager.text.text = string.Concat(displayedConsonants);
                    }
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
                signs.Add(signNow);
            Debug.Log("add " + signNow);
        }

        yield return new WaitForSeconds(0.5f); // ���� �����ӱ��� ���
        isMoving = false; // ������ ���� �÷��� ����
    }
    /// <summary>
    /// IENUMERATOR ///
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
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.black;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
        }
        yield return new WaitForSeconds(0.25f);
        MoveForward();
    }

    /// <summary>
    /// FUNCTIONS FOR IENUMERATOR ///
    /// </summary>
    void MoveForward()
    {
        startPos = base.GetPos();
        endPos = startPos + currentDir * 1.0f;
        transform.position = endPos;
    }

    private void RotateClock()
    {
        // �ð�������� 90�� ȸ��
        if (currentDir == Vector2.right)
            currentDir = Vector2.down;
        else if (currentDir == Vector2.down)
            currentDir = Vector2.left;
        else if (currentDir == Vector2.left)
            currentDir = Vector2.up;
        else if (currentDir == Vector2.up)
            currentDir = Vector2.right;

        Debug.Log("current direction" +  currentDir);
    }

    private void RotateCounter()
    {
        // �ݽð� �������� 90�� ȸ��
        if (currentDir == Vector2.right)
            currentDir = Vector2.up;
        else if (currentDir == Vector2.up)   
            currentDir = Vector2.left;
        else if (currentDir == Vector2.left) 
            currentDir = Vector2.down;
        else if (currentDir == Vector2.down) 
            currentDir = Vector2.right;

        Debug.Log("current direction" + currentDir);
    }

    private void RotateDiagonal()
    {
        // �밢�� ���� �ݽð� ���� ȸ��
        if (currentDir == Vector2.right)         // �� ��
            currentDir = new Vector2(1, 1);
        else if (currentDir == new Vector2(1, 1)) // �� ��
            currentDir = Vector2.up;
        else if (currentDir == Vector2.up)       // �� ��
            currentDir = new Vector2(-1, 1);
        else if (currentDir == new Vector2(-1, 1)) // �� ��
            currentDir = Vector2.left;
        else if (currentDir == Vector2.left)     // �� ��
            currentDir = new Vector2(-1, -1);
        else if (currentDir == new Vector2(-1, -1)) // �� ��
            currentDir = Vector2.down;
        else if (currentDir == Vector2.down)     // �� ��
            currentDir = new Vector2(1, -1);
        else if (currentDir == new Vector2(1, -1)) // �� ��
            currentDir = Vector2.right;

        Debug.Log("diagonal move, current direction: " + currentDir);
    }

}
