using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public void SceneChangeToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SceneChangeToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
