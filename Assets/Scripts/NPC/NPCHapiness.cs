using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// NPCHapiness manages the happiness of an NPC, including its UI updates and
/// the mechanics of happiness increase over time or based on events like job assignment.
/// </summary>
public class NPCHapiness : MonoBehaviour
{
    // References
    private NPC npc; // Reference to the associated NPC component

    // Happiness properties
    private float happiness = 10f; // Current happiness value
    private float maxHappiness = 100f; // Maximum happiness value
    private float happinessIncreaseRate = 10f; // Happiness increment when assigned a job
    private float happinessOverTimeRate = 5f; // Happiness increment over time
    private float timeInterval = 5f; // Time interval for periodic happiness increase

    // UI Elements
    public Slider happinessSlider; // Slider to display the current happiness
    public TextMeshProUGUI happinessText; // Text to display happiness as a percentage

    // Coroutine to handle happiness increase over time
    private Coroutine happinessCoroutine;

    /// <summary>
    /// Retrieves the current happiness value.
    /// </summary>
    /// <returns>The current happiness value as a float.</returns>
    public float GetCurrentHappiness()
    {
        return happiness;
    }

    /// <summary>
    /// Sets the happiness value, clamping it between 0 and maxHappiness.
    /// Updates the happiness UI accordingly.
    /// </summary>
    /// <param name="value">The new happiness value to set.</param>
    public void SetHappiness(float value)
    {
        happiness = Mathf.Clamp(value, 0, maxHappiness); // Ensure happiness stays within bounds
        UpdateHappinessUI();
    }

    /// <summary>
    /// Initializes the happiness system and ensures UI elements are correctly set up.
    /// </summary>
    private void Start()
    {
        npc = GetComponent<NPC>(); // Get the NPC component

        // Validate UI references
        if (happinessSlider == null)
        {
            Debug.Log("Happiness slider is not assigned");
        }
        if (happinessText == null)
        {
            Debug.Log("Happiness text is not assigned");
        }

        // Set slider properties and initial values
        happinessSlider.maxValue = maxHappiness;
        happinessSlider.value = happiness;
        UpdateHappinessUI();
    }

    /// <summary>
    /// Increases the happiness by a fixed amount when the NPC is assigned a job.
    /// </summary>
    public void AssignJob()
    {
        happiness = Mathf.Min(happiness + happinessIncreaseRate, maxHappiness); // Cap happiness at max
        UpdateHappinessUI(); // Update the UI
        Debug.Log("NPC happiness increased after assigning a job: " + happiness);
    }

    /// <summary>
    /// Directly increases the happiness by a specified amount.
    /// </summary>
    /// <param name="amount">The amount of happiness to add.</param>
    public void IncreaseHappiness(float amount)
    {
        happiness = Mathf.Min(happiness + amount, maxHappiness); // Cap happiness at max
        UpdateHappinessUI(); // Update the UI
    }

    /// <summary>
    /// Starts a coroutine to increase happiness periodically over time.
    /// </summary>
    public void StartIncreasingHappiness()
    {
        if (happinessCoroutine == null)
        {
            Debug.Log("Happiness increase started");
            happinessCoroutine = StartCoroutine(IncreaseHappinessOverTime());
        }
    }

    /// <summary>
    /// Stops the coroutine that increases happiness over time.
    /// </summary>
    public void StopIncreasingHappiness()
    {
        if (happinessCoroutine != null)
        {
            Debug.Log("Stopping happiness increase");
            StopCoroutine(happinessCoroutine);
            happinessCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine that periodically increases happiness while the NPC is employed.
    /// Stops if the NPC becomes unemployed or reaches maximum happiness.
    /// </summary>
    private IEnumerator IncreaseHappinessOverTime()
    {
        while (happiness < maxHappiness)
        {
            // Check if the NPC is unemployed
            if (npc.job == "Unemployed")
            {
                Debug.Log("NPC became unemployed, stopping happiness coroutine");
                StopIncreasingHappiness();
                yield break; // Exit the coroutine
            }

            // Wait for the specified interval before increasing happiness
            yield return new WaitForSeconds(timeInterval);

            // Increase happiness and log the updated value
            IncreaseHappiness(happinessOverTimeRate);
            Debug.Log("Happiness increased to: " + happiness);
        }
    }

    /// <summary>
    /// Updates the UI elements (slider and text) to reflect the current happiness.
    /// </summary>
    private void UpdateHappinessUI()
    {
        happinessSlider.value = happiness; // Update the slider value
        happinessText.text = Mathf.RoundToInt(happiness).ToString() + "%"; // Update the text
        Debug.Log("Happiness UI updated: " + happiness + "%");
    }
}
