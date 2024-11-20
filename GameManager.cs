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


    public static int col, row;


    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        parent = GameObject.Find("parent");
        GameText = GameObject.Find("GameText");
        text = GameText.GetComponent<TextMeshProUGUI>();

       // Map.SetMap(scene.name);

    }

}
