using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public RandomObjManager ObjManager;

    [Header("UI Reference (For Level 3)")]
    public GameObject myUIElement; // Kosongkan jika Level 1

    [Header("Audio Settings")]
    public AudioClip pickupSFX; 
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Interaction Settings")]
    public GameObject interactionPrompt;
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            DoCollect();
        }
    }

    private void DoCollect()
    {
        if (ObjManager != null)
        {
            // Memanggil fungsi yang sudah di-rename di Manager
            ObjManager.CollectItem(myUIElement);
        }

        if (pickupSFX != null)
        {
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, volume);
        }

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}