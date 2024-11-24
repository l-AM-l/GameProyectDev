using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// GameManager manages global game states, including energy management
/// and ensuring that the game object persists across scenes. 
/// It provides utility methods for interacting with energy levels and tracks which character to activate in scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager instance;


    // Energy system properties
    public int maxEnergy = 100; // Maximum energy value
    public Slider energySlider; // UI slider to represent current energy
    public TextMeshProUGUI energyText; // UI text to display current energy as a value


    /// <summary>
    /// Initializes the energy slider and updates the energy UI when the game starts.
    /// </summary>
    private void Start()
    {
        if (energySlider == null)
    {
        energySlider = FindObjectOfType<Slider>();
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
        }
        else
        {
            Debug.LogWarning("Energy Slider not found in the scene.");
        }
    }
        energySlider.maxValue = maxEnergy; // Set the maximum slider value to maxEnergy
        UpdateEnergyUI(); // Update the UI to reflect the initial energy level
    }
   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Reasignar referencias del Slider y TextMeshProUGUI en la nueva escena
    
    energySlider = GameObject.FindWithTag("EnergySlider")?.GetComponent<Slider>();
energyText = GameObject.FindWithTag("EnergyText")?.GetComponent<TextMeshProUGUI>();

    if (energySlider != null)
    {
        energySlider.maxValue = maxEnergy;
    }
    else
    {
        Debug.LogWarning("Energy Slider not found in the new scene.");
    }

    if (energyText == null)
    {
        Debug.LogWarning("Energy Text not found in the new scene.");
    }

    UpdateEnergyUI();
}

private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

    /// <summary>
    /// Updates the energy slider and text UI to reflect the current energy level.
    /// </summary>
    public void UpdateEnergyUI()
{
    if (energySlider != null)
    {
        energySlider.value = maxEnergy;
        Debug.Log($"Slider updated: {energySlider.value}");
    }
    else
    {
        Debug.LogWarning("Energy Slider is not assigned or found.");
    }

    if (energyText != null)
    {
        energyText.text = "Energía: " + maxEnergy;
    }
    else
    {
        Debug.LogWarning("Energy Text is not assigned.");
    }
}


    /// <summary>
    /// Ensures there is only one instance of GameManager and makes it persistent across scenes.
    /// Destroys duplicate instances if they exist.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Assign the instance
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Checks if the player has enough energy for a specified requirement.
    /// </summary>
    /// <param name="requiredEnergy">The energy required for the action.</param>
    /// <returns>True if the player has enough energy; otherwise, false.</returns>
    public bool HasEnoughEnergy(int requiredEnergy)
    {
        return maxEnergy >= requiredEnergy;
    }

    /// <summary>
    /// Reduces the player's energy by the specified amount, ensuring it does not go below 0.
    /// Updates the energy UI after consumption.
    /// </summary>
    /// <param name="amount">The amount of energy to use.</param>
    public void UseEnergy(int amount)
{
    if (maxEnergy <= 0)
    {
        maxEnergy = 0;
    }
    else
    {
        maxEnergy -= amount;
    }

    Debug.Log($"Energy used: {amount}, remaining: {maxEnergy}");
    UpdateEnergyUI();
}


    /// <summary>
    /// Increases the player's energy by the specified amount, ensuring it does not exceed the maximum energy limit.
    /// Updates the energy UI after addition.
    /// </summary>
    /// <param name="amount">The amount of energy to add.</param>
    public void AddEnergy(int amount)
    {

        maxEnergy = Mathf.Min(maxEnergy + amount, 100); // Ensure energy does not exceed the maximum
        Debug.Log($"Current energy: {maxEnergy}");
        UpdateEnergyUI(); // Update the energy UI
    }
}
