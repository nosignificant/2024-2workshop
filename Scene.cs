using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneLVL1()
    {
        SceneManager.LoadScene("level1");
        //SceneManager �޼����� LoadScene �Լ��� ���� acredev1.scene���� �� ��ȯ
    }
}
