using UnityEngine;
using Cinemachine;

public class UITransisiManager : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Kamera pertama yang aktif saat game dimulai")]
    public CinemachineVirtualCamera currentCamera;

    // Tentukan angka prioritas secara absolut agar lebih mudah dilacak
    private readonly int activePriority = 20;
    private readonly int inactivePriority = 10;

    public void Start()
    {
        // Pastikan currentCamera tidak kosong untuk mencegah error
        if (currentCamera != null)
        {
            currentCamera.Priority = activePriority;
        }
        else
        {
            Debug.LogWarning("Current Camera belum di-assign di Inspector!");
        }
    }

    public void UpdateCamera(CinemachineVirtualCamera targetCamera)
    {
        // Cegah eksekusi jika kamera tujuan sama dengan kamera saat ini
        if (targetCamera == currentCamera) return;

        // Turunkan prioritas kamera lama kembali ke default
        if (currentCamera != null)
        {
            currentCamera.Priority = inactivePriority;
        }

        // Jadikan kamera tujuan sebagai kamera baru dan naikkan prioritasnya
        currentCamera = targetCamera;
        currentCamera.Priority = activePriority;
    }
}