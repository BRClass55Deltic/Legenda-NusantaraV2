using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
       
        SceneManager.LoadScene(sceneName);
        Debug.Log("You PLAY");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
