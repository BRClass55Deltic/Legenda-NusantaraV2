using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishinginterktif : MonoBehaviour
{
    public bool playerMasuk;

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