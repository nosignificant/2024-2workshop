using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void tutorial()
    {
        SceneManager.LoadScene("tutorial");
    }
    public void tutorial1()
    {
        SceneManager.LoadScene("tutorial1");
    }

    public void tutorial2()
    {
        SceneManager.LoadScene("tutorial2");
    }
    public void tutorial3()
    {
        SceneManager.LoadScene("tutorial3");
    }
    public void SceneLVL1()
    {
        SceneManager.LoadScene("level1");
    }

    public void mainS()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
