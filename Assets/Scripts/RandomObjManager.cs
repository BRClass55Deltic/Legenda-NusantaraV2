using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjManager : MonoBehaviour
{
    public int totalCrystal = 3;
    private int currentCrystal = 0;

    [Header("Win Trigger")]
    public GameObject winTrigger;

    void Start()
    {
        if (winTrigger != null)
            winTrigger.SetActive(false);
    }

    // Fungsi menerima parameter UI dari CrystalCollectable
    public void CollectCrystal(GameObject specificCrystalUI)
    {
        currentCrystal++;

        // Aktifkan UI yang dibawa oleh kristal tersebut
        if (specificCrystalUI != null)
        {
            specificCrystalUI.SetActive(true);
        }

        // Cek kemenangan
        if (currentCrystal >= totalCrystal)
        {
            Debug.Log("Semua kristal terkumpul!");
            ActivateWinCondition();
        }
    }

    void ActivateWinCondition()
    {
        if (winTrigger != null)
            winTrigger.SetActive(true);
    }
}