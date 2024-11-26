using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MySceneManager maneja las transiciones de escena y asegura transiciones suaves entre escenas usando animaciones.
/// También gestiona una instancia singleton para accesibilidad global y previene objetos duplicados entre escenas.
/// </summary>
public class MySceneManager : MonoBehaviour
{
    // Instancia singleton para acceso global
    public static MySceneManager instance;

    // Animator para gestionar las animaciones de transición de escena
    public Animator transitionAnim;

    /// <summary>
    /// Asegura que solo haya una instancia de MySceneManager en el juego.
    /// Mantiene el objeto persistente entre escenas.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asigna la instancia
            DontDestroyOnLoad(gameObject); // Hace que el objeto sea persistente entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruye instancias duplicadas
        }
    }

    /// <summary>
    /// Inicia el proceso para cargar la siguiente escena con una animación de transición.
    /// </summary>
    public void NextScene()
    {
        StartCoroutine(LoadLevel()); // Inicia la coroutine para cargar la siguiente escena
    }

    /// <summary>
    /// Detecta cuando el jugador colisiona con el trigger e inicia la transición de escena.
    /// </summary>
    /// <param name="other">El collider que activó el evento.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador activó el evento
        {
            NextScene(); // Inicia la transición de escena
        }
    }

    /// <summary>
    /// Coroutine para manejar el proceso de transición de escena.
    /// Reproduce la animación de finalización, espera a que se complete y luego carga la siguiente escena.
    /// </summary>
    public IEnumerator LoadLevel()
    {
        // Activa la animación de transición de finalización
        transitionAnim.SetTrigger("End");

        // Espera a que la animación termine (ajusta el tiempo según la duración de la animación)
        yield return new WaitForSeconds(1);

        // Carga la siguiente escena de manera asíncrona
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Activa la animación de transición de inicio para la nueva escena
        transitionAnim.SetTrigger("Start");
    }
}
