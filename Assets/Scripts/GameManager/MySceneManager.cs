using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MySceneManager handles scene transitions and ensures smooth transitions between scenes using animations.
/// It also manages a singleton instance for global accessibility and prevents duplicate objects across scenes.
/// </summary>
public class MySceneManager : MonoBehaviour
{
    // Singleton instance for global access
    public static MySceneManager instance;

    // Animator for managing scene transition animations
    public Animator transitionAnim;

    /// <summary>
    /// Ensures there is only one instance of MySceneManager in the game.
    /// Keeps the object persistent across scenes.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Assign the instance
            DontDestroyOnLoad(gameObject); // Make the object persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Starts the process to load the next scene with a transition animation.
    /// </summary>
    public void NextScene()
    {
        StartCoroutine(LoadLevel()); // Start the coroutine to load the next scene
    }

    /// <summary>
    /// Detects when the player collides with the trigger and initiates the scene transition.
    /// </summary>
    /// <param name="other">The collider that triggered the event.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player triggered the event
        {
            NextScene(); // Start the scene transition
        }
    }

    /// <summary>
    /// Coroutine to handle the scene transition process.
    /// Plays the end animation, waits for it to complete, and then loads the next scene.
    /// </summary>
    public IEnumerator LoadLevel()
    {
        // Trigger the end transition animation
        transitionAnim.SetTrigger("End");

        // Wait for the animation to finish (adjust time as per animation length)
        yield return new WaitForSeconds(1);

        // Load the next scene asynchronously
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Trigger the start transition animation for the new scene
        transitionAnim.SetTrigger("Start");
    }
}
