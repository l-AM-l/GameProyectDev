using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public GameObject guide; // Asigna el guía en el inspector.
    public GameObject guide1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra es el jugador.
        {
            if (guide != null)
            {
                guide.SetActive(true); // Activa el objeto guía.
                guide1.SetActive(false);
            }
        }
    }
}

