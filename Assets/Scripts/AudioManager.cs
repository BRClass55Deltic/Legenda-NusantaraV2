using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <-- 1. TAMBAHKAN INI UNTUK SCENE MANAGEMENT

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmMusic;
    public AudioClip deathSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 2. Daftarkan fungsi ke sistem Unity agar tahu kapan scene berubah
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 3. Bersihkan pendaftaran saat objek dihancurkan (opsional, tapi praktik yang baik)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Tetap mainkan BGM saat pertama kali game dibuka
        PlayBGM();
    }

    // 4. FUNGSI BARU: Otomatis dipanggil setiap kali scene di-restart atau pindah level
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cek apakah BGM sedang mati, jika ya, mainkan lagi dari awal
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            PlayBGM();
        }
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmMusic != null)
        {
            bgmSource.clip = bgmMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void TriggerGameOverAudio()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        if (sfxSource != null && deathSFX != null)
        {
            sfxSource.PlayOneShot(deathSFX);
        }
    }
}