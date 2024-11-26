using System;
using UnityEngine;

/// <summary>
/// PlayerPersistence maneja el guardado y la restauración de la posición del jugador
/// utilizando PlayerPrefs para asegurar que el jugador comience en la misma posición
/// después de una transición de escena o recarga del juego. Limpia los datos guardados cuando el juego se cierra.
/// </summary>
public class PlayerPersistence : MonoBehaviour
{
    /// <summary>
    /// Establece la posición del jugador al inicio del juego o escena.
    /// Recupera los valores de posición guardados (x, y, z) de PlayerPrefs.
    /// </summary>
    private void Start()
    {
        // Restaura la posición del jugador utilizando los valores guardados
        // La posición predeterminada es (0, 0, 0) si no existen valores en PlayerPrefs
        transform.position = new Vector3(
            PlayerPrefs.GetFloat("x", -26f), 
            PlayerPrefs.GetFloat("y", 0f), 
            PlayerPrefs.GetFloat("z", 0f)
        );
    }

    /// <summary>
    /// Guarda continuamente la posición actual del jugador en PlayerPrefs.
    /// Actualiza la posición cada fotograma para asegurar precisión.
    /// </summary>
    private void Update()
    {
        // Guarda la posición actual del jugador (x, y, z) en PlayerPrefs
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);
    }

    /// <summary>
    /// Limpia los datos de posición guardados de PlayerPrefs cuando el juego se cierra.
    /// </summary>
    // Este método se encarga de eliminar los datos guardados al salir del juego
}
