using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneLVL1()
    {
        SceneManager.LoadScene("level1");
        //SceneManager 메서드의 LoadScene 함수를 통해 acredev1.scene으로 씬 전환
    }
}
