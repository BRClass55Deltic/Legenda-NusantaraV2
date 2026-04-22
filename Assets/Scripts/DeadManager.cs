using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject deathPanel; // Tarik Panel UI "YOU DIED" ke sini

    private void Start()
    {
        // Pastikan panel mati saat mulai
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    // --- LOGIK DETEKSI (Trigger) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDeathScreen();
        }
    }

    void ShowDeathScreen()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true); // Munculkan panel
            Time.timeScale = 0f;        // Pause game
            Debug.Log("Player masuk Dead Zone. Game Paused.");
        }
    }

    // --- LOGIK TOMBOL (UI) ---
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Jalankan waktu lagi (WAJIB)
        
        // Load ulang scene yang sedang aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Level Restarted");
    }
}