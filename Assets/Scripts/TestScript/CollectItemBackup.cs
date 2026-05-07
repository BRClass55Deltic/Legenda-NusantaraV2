using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemBackup : MonoBehaviour
{
    //COPAS SKRIP INI JIKA COLLECTITEM MENGALAMI BUG / SKRIP BARU TIDAK SESUAI
    
    public RandomObjManager ObjManager;

    [Header("UI Reference")]
    public GameObject myUIElement; // Drag UI spesifik kristal ini di Inspector

    [Header("Audio Settings")]
    public AudioClip pickupSFX; 
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Interaction Settings")]
    public GameObject interactionPrompt; // Opsional: Munculkan teks "Press E"
    private bool isPlayerNearby = false;

    private void Update()
    {
        // Cek apakah player di dekat kristal dan menekan tombol E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            DoCollect();
        }
    }

    private void DoCollect()
    {
        // 1. Kirim data UI ke manager
        if (ObjManager != null)
        {
            ObjManager.CollectItem(myUIElement);
        }

        // 2. Putar suara secara mandiri
        if (pickupSFX != null)
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, volume);
        }

        // 3. Matikan prompt interaksi sebelum objek dihancurkan
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // 4. Hancurkan kristal
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // Munculkan teks "Press E" jika ada
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Sembunyikan teks "Press E" saat menjauh
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
}