using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;      // Tarik objek Player ke sini di Inspector
    public float moveSpeed = 3f;  // Kecepatan lari musuh
    public float stoppingDistance = 1.2f; // Jarak musuh berhenti mengejar (biar nggak tumpang tindih)

    void Update()
    {
        if (player != null)
        {
            // Menghitung jarak antara musuh dan player
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stoppingDistance)
            {
                // 1. Membuat musuh menghadap ke arah player
                transform.LookAt(player);

                // 2. Menggerakkan musuh maju ke depan
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }
}