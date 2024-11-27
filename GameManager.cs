using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]

    public GameObject parent;
    public GameObject GameText;
    public static TextMeshProUGUI text;
    public static Dictionary<Vector2, GameObject> MovingTile = new Dictionary<Vector2, GameObject>();
    public static Dictionary<Vector2, GameObject> ObstacleTiles = new Dictionary<Vector2, GameObject>();


    public static bool isTutorial;


    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        parent = GameObject.Find("parent");
        GameText = GameObject.Find("GameText");
        text = GameText.GetComponent<TextMeshProUGUI>();

            RegisterMovingTiles();

    }

    public static void RegisterMovingTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);

            if (!MovingTile.ContainsKey(position))
            {
                MovingTile.Add(position, tile.gameObject);
                Debug.Log($"Registered MovingTile: {tile.gameObject.name} at {position}");
            }
            else
            {
                Debug.LogWarning($"Duplicate Tile position found at {position}. Skipping: {tile.gameObject.name}");
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
            {
                ObstacleTiles.Add(position, obstacle);
                Debug.Log($"Registered Obstacle: {obstacle.name} at {position}");
            }
            else
            {
                Debug.LogWarning($"Duplicate Obstacle position found at {position}. Skipping: {obstacle.name}");
            }
        }
    }
}
