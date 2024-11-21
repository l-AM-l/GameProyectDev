using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportation : MonoBehaviour
{
    [Header("Scene Names")]
    private string baseSceneName = "BaseScene"; // Nombre de la escena base
    private string explorationSceneName = "PlatformerScene"; // Nombre de la escena de exploración

    [Header("Input Settings")]
    public KeyCode transitionKey = KeyCode.T; // Tecla para cambiar de escena

    [Header("Transition Settings")]
    public Animator transitionAnim; // Referencia al Animator para las transiciones
    private bool isSwitching = false; // Previene múltiples cambios simultáneos

    private void Update()
    {
        if (Input.GetKeyDown(transitionKey) && !isSwitching)
        {
            // Alterna entre las escenas
            if (SceneManager.GetActiveScene().name == baseSceneName)
            {
                SwitchScene(explorationSceneName);
            }
            else if (SceneManager.GetActiveScene().name == explorationSceneName)
            {
                SwitchScene(baseSceneName);
            }
        }
    }

    private void SwitchScene(string targetSceneName)
    {
        isSwitching = true; // Bloquea cambios adicionales hasta que termine el actual
        StartCoroutine(TransitionAndLoadScene(targetSceneName));
    }

    private System.Collections.IEnumerator TransitionAndLoadScene(string targetSceneName)
    {
        // Inicia la animación de final
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("End");
        }

        // Espera a que termine la animación de salida
        yield return new WaitForSeconds(1f); // Ajusta al tiempo de tu animación

        // Carga la escena de forma asíncrona
        var operation = SceneManager.LoadSceneAsync(targetSceneName);
        while (!operation.isDone)
        {
            yield return null; // Espera hasta que la escena esté completamente cargada
        }

        // Inicia la animación de inicio
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("Start");
        }

        isSwitching = false; // Permite más cambios de escena
    }
}
