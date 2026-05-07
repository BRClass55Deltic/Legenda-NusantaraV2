using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Pointer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target == null) return;

        var targetPosition = target.position;
        // Menjaga agar panah tetap datar (tidak nunduk/ndengak)
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }

    // Fungsi untuk mengganti target dari script lain
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Fungsi untuk mematikan/menyalakan panah
    public void SetVisible(bool state)
    {
        gameObject.SetActive(state);
    }
}