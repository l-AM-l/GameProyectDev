using System;
using UnityEngine;

/// <summary>
/// PlayerPersistence handles saving and restoring the player's position
/// using PlayerPrefs to ensure the player starts in the same position
/// after a scene transition or game reload.
/// </summary>
public class PlayerPersistence : MonoBehaviour
{
    /// <summary>
    /// Sets the player's position at the start of the game or scene.
    /// Retrieves the saved position values (x, y, z) from PlayerPrefs.
    /// </summary>
    private void Start()
    {
        // Restore the player's position using saved values
        // Default position is (0, 0, 0) if no values exist in PlayerPrefs
        transform.position = new Vector3(
            PlayerPrefs.GetFloat("x", 0f), 
            PlayerPrefs.GetFloat("y", 0f), 
            PlayerPrefs.GetFloat("z", 0f)
        );
    }

    /// <summary>
    /// Continuously saves the player's current position to PlayerPrefs.
    /// Updates the position every frame to ensure accuracy.
    /// </summary>
    private void Update()
    {
        // Save the player's current position (x, y, z) in PlayerPrefs
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);
    }
}
