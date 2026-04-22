using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Mengecek jika yang menyentuh trigger ini adalah Musuh
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Debug.Log("Musuh Despawn (Dihancurkan)");
        }
    }
}