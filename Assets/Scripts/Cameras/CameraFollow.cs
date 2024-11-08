
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;  // Referencia al Transform del jugador
    [SerializeField] public float followSpeed = 0.1f;   // Velocidad de seguimiento
    [SerializeField] public Vector3 offset;             // Offset de la cámara respecto al jugador

    void Update()
    {
        if (playerTransform != null)
        {
            // Mueve la cámara suavemente hacia la posición del jugador con el offset
            Vector3 targetPosition = playerTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }
}

