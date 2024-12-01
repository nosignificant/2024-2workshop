using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject GameButton;
    public void tutorial()
    {
        SceneManager.LoadScene("tutorial");
        GameButton = GameObject.Find("NEXT");
        GameButton.SetActive(false);
        if (GameManager.gText.text != null)
            GameButton.SetActive(true);
    }
    public void tutorial1()
    {
        SceneManager.LoadScene("tutorial 1");
    }

    public void tutorial2()
    {
        SceneManager.LoadScene("tutorial 2");

    }
    public void SceneLVL1()
    {
        SceneManager.LoadScene("lvl1");
    }

    public void mainS()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
