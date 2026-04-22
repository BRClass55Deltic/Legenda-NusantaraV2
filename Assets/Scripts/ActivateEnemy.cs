using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyToActivate; // Seret objek musuh yang "mati" ke sini
    public bool destroyTriggerAfterUse = true;

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang menyentuh trigger adalah Player
        if (other.CompareTag("Player"))
        {
            EnemyActivated();

            if (destroyTriggerAfterUse)
            {
                Destroy(gameObject); // Menghapus objek trigger ini agar tidak boros memori
            }
        }
    }

    void EnemyActivated()
    {
        if (enemyToActivate != null)
        {
            enemyToActivate.SetActive(true); // Mengubah musuh dari Off ke On
            Debug.Log("Musuh sekarang Aktif!");
        }
        else
        {
            Debug.LogWarning("Objek musuh belum dimasukkan ke slot Inspector!");
        }
    }
}