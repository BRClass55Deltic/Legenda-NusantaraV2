using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCrystal : MonoBehaviour
{
    public RandomObjManager ObjManager;

    [Header("UI Reference")]
    public GameObject myUIElement; // Drag UI spesifik kristal ini di Inspector

    [Header("Audio Settings")]
    public AudioClip pickupSFX; // Masukkan file audio kristal di sini
    [Range(0f, 1f)] public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Kirim data UI ke manager
            if (ObjManager != null)
            {
                ObjManager.CollectCrystal(myUIElement);
            }

            // 2. Putar suara secara mandiri
            // PlayClipAtPoint membuat objek audio sementara di posisi kristal
            if (pickupSFX != null)
            {
                AudioSource.PlayClipAtPoint(pickupSFX, transform.position, volume);
            }

            // 3. Hancurkan kristal
            Destroy(gameObject);
        }
    }
}