using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform targetObject; // Kameran�n takip edece�i karakterin referans�
    

    public Vector3 cameraOffset; // Kamera ve karakter aras�ndaki mesafe

    public float smoothFactor = 0.5f;

    public bool lookAtTarget = false;
    void Start()
    {
        // Ba�lang��ta kamera ile karakter aras�ndaki mesafeyi hesapla
        cameraOffset = transform.position - targetObject.transform.position;
    }

    void LateUpdate()
    {
        // Kamera pozisyonunu karakterin pozisyonuna g�re g�ncelle
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtTarget)
        {
            transform.LookAt(targetObject);
        }
    }
}

