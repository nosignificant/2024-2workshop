using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void tutorial()
    {
        SceneManager.LoadScene("tutorial");
        GameManager.isTutorial = true;
        Debug.Log("tutorial true");
    }
    public void tutorial1()
    {
        SceneManager.LoadScene("tutorial1");
        GameManager.isTutorial = true;
    }

    public void tutorial2()
    {
        SceneManager.LoadScene("tutorial2");
        GameManager.isTutorial = true;
    }
    public void SceneLVL1()
    {
        SceneManager.LoadScene("lvl1");
        GameManager.isTutorial = false;
    }

    public void mainS()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
