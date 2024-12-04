using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject parent;
    public GameObject GameText;
    public GameObject GameButton;
    public static TextMeshProUGUI space;
    public static TextMeshProUGUI gText;
    public static Dictionary<Vector2, GameObject> MovingTile = new Dictionary<Vector2, GameObject>();
    public static Dictionary<Vector2, GameObject> ObstacleTiles = new Dictionary<Vector2, GameObject>();

    private bool isGameEnded = false;



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        StartTile.start = true; // 초기화
        StartTile.signs.Clear(); // 초기화
        isGameEnded = false; // GameManager 상태 초기화

        // 타일과 장애물 재등록
        MovingTile.Clear();
        ObstacleTiles.Clear();
        RegisterMovingTiles();
        RegisterCantMovePlace();
    }

    public bool GameEndCheck()
    {
        if (isGameEnded) return false;

        if (SceneManager.GetActiveScene().name == "tutorial" && !string.IsNullOrEmpty(gText.text))
        {
            GameButton.SetActive(true);
            isGameEnded = true;
            return true;
        }

        if (SceneManager.GetActiveScene().name == "tutorial 0" && Alphabet.FindConsonant(StartTile.signs) == "ㄱ")
        {
            GameButton.SetActive(true);
            isGameEnded = true;
            return true;
        }

        if (SceneManager.GetActiveScene().name == "tutorial 1" && Alphabet.FindConsonant(StartTile.signs) == "ㄱ")
        {
            GameButton.SetActive(true);
            isGameEnded = true;
            return true;
        }

        if (SceneManager.GetActiveScene().name == "tutorial 2" && Alphabet.FindConsonant(StartTile.signs) == "ㄴ")
        {
            GameButton.SetActive(true);
            isGameEnded = true;
            return true;
        }
        if (SceneManager.GetActiveScene().name == "tutorial 3" && Alphabet.FindConsonant(StartTile.signs) == "ㄱㄴ")
        {
            GameButton.SetActive(true);
            isGameEnded = true;
            return true;
        }
        return false;
    }

    void Start()
    {
        Debug.Log($"Active Scene: {SceneManager.GetActiveScene().name}");

        parent = GameObject.Find("parent");
        GameText = GameObject.Find("GameText");
        GameButton = GameObject.Find("NEXT");
        GameButton.SetActive(false);
        space = GameObject.Find("SPACE").GetComponent<TextMeshProUGUI>();
        gText = GameText.GetComponent<TextMeshProUGUI>();
        SetResolution();
    }

    private void Update()
    {
        if (!isGameEnded && GameEndCheck())
        {
            StartTile.start = true;
            StartTile.signs.Clear();
        }

        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();
    }

    public static void RegisterMovingTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.name != "go")
            {
                Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
                if (!MovingTile.ContainsKey(position))
                    MovingTile.Add(position, tile.gameObject);
            }
        }
    }

    public static void RegisterCantMovePlace()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Vector2 position = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y);

            if (!ObstacleTiles.ContainsKey(position))
                ObstacleTiles.Add(position, obstacle);
        }
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        // 씬 로드 시 초기화를 위해 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetResolution()
    {
        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, true);
    }
}
