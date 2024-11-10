
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;  // Referencia al Transform del jugador
    [SerializeField] private float followSpeed = 0.1f;   // Velocidad de seguimiento
    [SerializeField] private Vector3 offset;             // Offset de la cámara respecto al jugador

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Mueve la cámara suavemente hacia la posición del jugador con el offset
            Vector3 targetPosition = playerTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }
}

