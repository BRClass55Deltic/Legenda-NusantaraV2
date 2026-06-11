using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Kita tetap pakai Instance agar skrip Enemy/DeathManager bisa memanggil dengan mudah
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgmMusic;
    public AudioClip deathSFX;

    private void Awake()
    {
        // Langsung tetapkan instance ke objek yang ada di scene saat ini
        instance = this;
    }

    private void Start()
    {
        // Setiap kali scene di-load atau di-restart, fungsi ini PASTI jalan 
        // dan otomatis memutar BGM khusus scene ini dari awal
        PlayBGM();
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