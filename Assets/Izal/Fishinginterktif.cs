using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishinginterktif : MonoBehaviour
{
    public GameObject pancinganInteraktif;
    public GameObject pancingan;

    public bool playerMasuk;

    void Start()
    {
        pancingan.SetActive(false);
    }

    void Update()
    {
        if (playerMasuk && Input.GetKeyDown(KeyCode.E))
        {
            pancinganInteraktif.SetActive(false);
            pancingan.SetActive(true);

            Debug.Log("Memancing dimulai");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMasuk = true;
            Debug.Log("player masuk");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMasuk = false;
            Debug.Log("player keluar");
        }
    }
}