using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform targetObject; // Kameranýn takip edeceði karakterin referansý
    

    public Vector3 cameraOffset; // Kamera ve karakter arasýndaki mesafe

    public float smoothFactor = 0.5f;

    public bool lookAtTarget = false;
    void Start()
    {
        // Baþlangýçta kamera ile karakter arasýndaki mesafeyi hesapla
        cameraOffset = transform.position - targetObject.transform.position;
    }

    void LateUpdate()
    {
        // Kamera pozisyonunu karakterin pozisyonuna göre güncelle
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtTarget)
        {
            transform.LookAt(targetObject);
        }
    }
}

