using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;

    // Desplazamiento de la cámara (modificable desde el inspector)
    public Vector3 CameraOffset;

    // Transform de la cámara para el efecto de shake.
    private Transform camTransform;

    // Duración del shake.
    public float shakeDuration = 0f;

    // Amplitud del shake.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        Cursor.visible = false;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if (Target != null)
        {
            // Aplica el desplazamiento al objetivo antes de calcular la nueva posición
            Vector3 targetPosition = Target.position + CameraOffset;
            targetPosition.z = -1; // Asegúrate de que la cámara esté en el plano correcto
            transform.position = Vector3.Slerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
        }

        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.2f;
    }
}
