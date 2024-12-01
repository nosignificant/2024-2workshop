using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]

    public GameObject parent;
    public GameObject GameText;
    public GameObject GameButton;
    public static TextMeshProUGUI space;
    public static TextMeshProUGUI gText;
    public static Dictionary<Vector2, GameObject> MovingTile = new Dictionary<Vector2, GameObject>();
    public static Dictionary<Vector2, GameObject> ObstacleTiles = new Dictionary<Vector2, GameObject>();

    public bool GameEndCheck()
    {

        if (SceneManager.GetActiveScene().name == "tutorial")
            if (!string.IsNullOrEmpty(gText.text))
            {
                GameButton.SetActive(true);
                return true;
            }

        if (SceneManager.GetActiveScene().name == "tutorial 0")
            if (Alphabet.FindConsonant(StartTile.signs) == "ㄱ")
            {
                GameButton.SetActive(true);
                return true;
            }

        return false;
        }

    void Start()
    {
        Debug.Log($"Active Scene: {SceneManager.GetActiveScene().name}");
        Scene scene = SceneManager.GetActiveScene();

        parent = GameObject.Find("parent");
        GameText = GameObject.Find("GameText");
        GameButton = GameObject.Find("NEXT");
        GameButton.SetActive(false);
        space = GameObject.Find("SPACE").GetComponent<TextMeshProUGUI>();
        gText = GameText.GetComponent<TextMeshProUGUI>();

        RegisterMovingTiles();
        RegisterCantMovePlace();
    }

    private void Update()
    {
        if (GameEndCheck())
        {
            StartTile.start = true;
            StartTile.signs.Clear();
        }
    }

    //현재 맵의 타일 등록
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

}
