using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivation : MonoBehaviour
{
    // Masukkan skrip pancingan (atau skrip lain) yang mau diaktifkan/dimatikan di sini
    [SerializeField] MonoBehaviour skripTarget; 

    void Start()
    {
        // Di awal game, pastikan skrip target dalam keadaan MATI
        if (skripTarget != null)
        {
            skripTarget.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jika Player masuk area, AKTIFKAN skrip target
        if (other.CompareTag("Player") && skripTarget != null)
        {
            skripTarget.enabled = true;
            Debug.Log(skripTarget.GetType().Name + " telah AKTIF.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Jika Player keluar area, MATIKAN skrip target
        if (other.CompareTag("Player") && skripTarget != null)
        {
            skripTarget.enabled = false;
            Debug.Log(skripTarget.GetType().Name + " telah NON-AKTIF.");
        }
    }
}