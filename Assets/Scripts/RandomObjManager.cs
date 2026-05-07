using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomObjManager : MonoBehaviour
{
    public int totalItems = 3;
    private int currentItems = 0;

    [Header("Win Settings")]
    public GameObject winTrigger;
    public Arrow_Pointer arrowPointer;

    [Header("Level 1 Settings (Text Mode)")]
    public TextMeshProUGUI counterText; // Taruh UI Text di sini
    public string itemPrefix = "Sayur: "; // Contoh: "Sayur: "

    void Start()
    {
        if (winTrigger != null) winTrigger.SetActive(false);
        if (arrowPointer != null) arrowPointer.SetVisible(false);
        
        UpdateTextUI(); // Update teks di awal
    }

    // Fungsi utama yang dipanggil item
    public void CollectItem(GameObject specificUI)
    {
        currentItems++;

        // MODE LEVEL 3: Jika ada referensi Image UI, nyalakan
        if (specificUI != null)
        {
            specificUI.SetActive(true);
        }

        // MODE LEVEL 1: Update Teks Counter
        UpdateTextUI();

        // Cek Kemenangan
        if (currentItems >= totalItems)
        {
            ActivateWinCondition();
        }
    }

    void UpdateTextUI()
    {
        if (counterText != null)
        {
            counterText.text = itemPrefix + currentItems + "/" + totalItems;
        }
    }

    void ActivateWinCondition()
    {
        if (winTrigger != null) winTrigger.SetActive(true);
        if (arrowPointer != null)
        {
            arrowPointer.SetVisible(true);
            arrowPointer.SetTarget(winTrigger.transform);
        }
    }
}