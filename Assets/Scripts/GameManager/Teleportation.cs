using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Teleportation manages scene transitions between two predefined scenes.
/// It includes smooth transitions with animations and prevents multiple simultaneous transitions.
/// </summary>
public class Teleportation : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Name of the base scene to switch to.")]
    private string baseSceneName = "BaseScene"; // Name of the base scene
    [Tooltip("Name of the exploration scene to switch to.")]
    private string explorationSceneName = "PlatformerScene"; // Name of the exploration scene

    [Header("Input Settings")]
    [Tooltip("Key used to trigger scene transitions.")]
    public KeyCode transitionKey = KeyCode.T; // Key to switch between scenes

    [Header("Transition Settings")]
    [Tooltip("Reference to the Animator controlling transition animations.")]
    public Animator transitionAnim; // Reference to the Animator for transitions
    private bool isSwitching = false; // Prevents multiple transitions at the same time

    /// <summary>
    /// Detects input to switch between scenes and initiates the transition process.
    /// </summary>
    private void Update()
    {
        // Check if the transition key is pressed and no transition is currently active
        if (Input.GetKeyDown(transitionKey) && !isSwitching)
        {
            // Switch to the appropriate scene based on the current scene
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

    /// <summary>
    /// Initiates the scene switching process to the target scene.
    /// </summary>
    /// <param name="targetSceneName">The name of the scene to switch to.</param>
    private void SwitchScene(string targetSceneName)
    {
        // Prevent additional transitions during the current switch
        isSwitching = true;
        StartCoroutine(TransitionAndLoadScene(targetSceneName));
    }

    /// <summary>
    /// Handles the transition animation and asynchronous loading of the target scene.
    /// </summary>
    /// <param name="targetSceneName">The name of the scene to switch to.</param>
    private System.Collections.IEnumerator TransitionAndLoadScene(string targetSceneName)
    {
        // Trigger the end animation if an Animator is assigned
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("End");
        }

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(1f); // Adjust this duration to match your animation

        // Load the target scene asynchronously
        var operation = SceneManager.LoadSceneAsync(targetSceneName);
        while (!operation.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        // Trigger the start animation for the new scene if an Animator is assigned
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("Start");
        }

        // Allow new transitions after the current one completes
        isSwitching = false;
    }
}
