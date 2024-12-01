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

    public void tutorial0()
    {
        SceneManager.LoadScene("tutorial 0");
        
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
