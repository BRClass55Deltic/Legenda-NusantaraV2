using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    

public class Pause : MonoBehaviour
{
    
    public static bool isPaused = false;

    public GameObject pauseMenuUI; // Tarik Panel UI kamu ke sini
    
    void Update()
    {
        // Cek input tombol Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                StartPause();
            }
        }
    }

    // Fungsi untuk melanjutkan game (Bisa dipanggil dari Button Un-pause)
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Menjalankan kembali waktu game
        isPaused = false;

        // Opsional: Mengunci kursor kembali (jika game kamu FPS/TPS)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void StartPause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Menghentikan waktu game (fisika, update, dll)
        isPaused = true;

        // Memunculkan kursor agar bisa klik tombol
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
