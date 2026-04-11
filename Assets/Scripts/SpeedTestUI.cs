using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedTestUI : MonoBehaviour
{
    public Rigidbody playerRb;
    public TextMeshProUGUI speedText;

    // Update is called once per frame
    void Update()
    {
        Vector3 flatVel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        float currentSpeed = flatVel.magnitude;

        speedText.text = "Speed: " + currentSpeed.ToString("F1") + " m/s";
        
        if (currentSpeed > 8f) 
            speedText.color = Color.red;
        else 
            speedText.color = Color.white;
    }
}
