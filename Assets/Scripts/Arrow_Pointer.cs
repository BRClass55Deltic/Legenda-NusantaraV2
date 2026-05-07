using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Pointer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        var targetPosition = target.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
} 