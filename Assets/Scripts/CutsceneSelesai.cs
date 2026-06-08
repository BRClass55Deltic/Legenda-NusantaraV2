using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSelesai : MonoBehaviour
{
    public int scene;
    public void Cutscene()
    {
        SceneManager.LoadScene(scene);
    }
    public void ExitGame()
    {
        Debug.Log("Game Keluar!");
        Application.Quit();
    }
}
