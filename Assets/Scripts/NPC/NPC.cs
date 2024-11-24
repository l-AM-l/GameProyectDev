using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// NPC handles the behavior and interactions of Non-Player Characters in the game.
/// This includes job assignments, movement, happiness management, and energy generation.
/// </summary>
public class NPC : MonoBehaviour
{
    // Happiness management for the NPC
    public NPCHapiness npcHapiness;
    public int npcID;
    // NPC-specific details
    public NPCtype nPCtype; // The type of NPC
    public string job; // Current job of the NPC

    // UI elements for displaying NPC information
    public GameObject npcInfoUI; // UI panel for NPC details
    public TextMeshProUGUI npcNameText; // Text for NPC name
    public TextMeshProUGUI npcJobText; // Text for NPC job
    public TMP_Dropdown jobDropdown; // Dropdown menu for assigning jobs

    // Building-related information
    private Building building; // Current building assigned to the NPC

    // Movement properties
    public Vector3 targetPosition; // Target position for the NPC to move to
    public float moveSpeed = 2f; // Movement speed of the NPC
    private bool isMovingToJob = false; // Indicates if the NPC is currently moving to a job

    // Player interaction tracking
    private bool isPlayerNearby = false;

    /// <summary>
    /// Initializes the NPC and hides the UI at the start.
    /// </summary>
    private void Start()
    {
        npcInfoUI.SetActive(false); // Hide NPC info UI
        npcHapiness = GetComponent<NPCHapiness>(); // Initialize happiness component
        LoadNPCState(npcID);
    }
    /// <summary>
/// Saves the NPC's current position and job to PlayerPrefs.
/// </summary>
public void SaveNPCState(int npcID)
{
    // Save position
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosX", transform.position.x);
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosY", transform.position.y);
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosZ", transform.position.z);

    // Save job
    PlayerPrefs.SetString($"NPC_{npcID}_Job", string.IsNullOrEmpty(job) ? "Unassigned" : job);

    PlayerPrefs.Save();
    Debug.Log($"NPC {npcID} state saved.");
}

/// <summary>
/// Loads the NPC's position and job from PlayerPrefs.
/// </summary>
public void LoadNPCState(int npcID)
{
    // Load position
    float posX = PlayerPrefs.GetFloat($"NPC_{npcID}_PosX", transform.position.x);
    float posY = PlayerPrefs.GetFloat($"NPC_{npcID}_PosY", transform.position.y);
    float posZ = PlayerPrefs.GetFloat($"NPC_{npcID}_PosZ", transform.position.z);
    transform.position = new Vector3(posX, posY, posZ);

    // Load job
    job = PlayerPrefs.GetString($"NPC_{npcID}_Job", "Unassigned");

    if (job != "Unassigned")
    {
        Building assignedBuilding = FindBuildingByJob(job);
        if (assignedBuilding != null)
        {
            assignedBuilding.assignedNPCs.Add(this);
            building = assignedBuilding;
            MoveToAssignedBuilding(assignedBuilding);
        }
    }

    Debug.Log($"NPC {npcID} state loaded. Job: {job}, Position: {transform.position}");
}

    /// <summary>
    /// Detects when the player enters the NPC's area.
    /// </summary>
    /// <param name="other">Collider that triggered the event.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is nearby.");
            isPlayerNearby = true;
        }
    }

    /// <summary>
    /// Detects when the player leaves the NPC's area.
    /// </summary>
    /// <param name="other">Collider that triggered the event.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left.");
            isPlayerNearby = false;
            npcInfoUI.SetActive(false); // Hide UI when the player leaves
        }
    }

    /// <summary>
    /// Handles movement, interaction, and UI toggling.
    /// </summary>
    private void Update()
    {
        if (isMovingToJob)
        {
            Debug.Log("NPC is moving...");
            MoveToTarget();
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (npcInfoUI.activeSelf)
            {
                npcInfoUI.SetActive(false); // Hide UI
            }
            else
            {
                Debug.Log("Showing NPC info.");
                ShowNPCInfo(); // Show UI
            }
        }
    }

 /// <summary>
    /// Moves the NPC towards the assigned target position.
    /// </summary>
    private void MoveToTarget()
    {
        if (targetPosition == null)
        {
            Debug.Log("Target position is not set!");
            return;
        }

        // Move horizontally while keeping Y position constant
        Vector3 horizontalTargetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, horizontalTargetPosition, step);

        // Stop movement when the target is reached
        if (Vector3.Distance(transform.position, horizontalTargetPosition) < 0.1f)
        {
            isMovingToJob = false;
            Debug.Log("NPC has arrived at the building.");
            SaveNPCState(npcID);
            npcHapiness.StartIncreasingHappiness(); // Start happiness increase
        }
    }

    /// <summary>
    /// Assigns a target position for the NPC and starts movement.
    /// </summary>
    /// <param name="position">The target position for the NPC to move to.</param>
    public void SetJobPosition(Vector3 position)
    {
        targetPosition = position;
        isMovingToJob = true;
        Debug.Log("NPC is moving to job at position: " + targetPosition);
    }


    public static List<string> GetBuildingTypes()
{
    List<string> buildingTypes = new List<string>();
    
    // Obtiene todos los valores del enum BuildingType
    foreach (BuildingType type in System.Enum.GetValues(typeof(BuildingType)))
    {
        buildingTypes.Add(type.ToString());
    }

    return buildingTypes;
}

/// <summary>
    /// Displays the NPC info UI and initializes the job dropdown menu.
    /// </summary>
    private void ShowNPCInfo()
    {
        npcNameText.text = "Name: " + nPCtype.ToString();
        npcJobText.text = "Job: " + (string.IsNullOrEmpty(job) ? "Unassigned" : job);

        // Populate dropdown menu with job options
        List<string> jobs = GetBuildingTypes();
        jobs.Insert(0, "Unassigned");

        jobDropdown.ClearOptions();
        jobDropdown.AddOptions(jobs);

        // Set dropdown value based on current job
        jobDropdown.value = string.IsNullOrEmpty(job) ? 0 : jobs.IndexOf(job);

        // Register job change listener
        jobDropdown.onValueChanged.RemoveAllListeners();
        jobDropdown.onValueChanged.AddListener(delegate { ChangeJob(jobDropdown); });

        npcInfoUI.SetActive(true); // Show UI
    }

 /// <summary>
/// Changes the job of the NPC and updates the building assignment accordingly.
/// Handles both assigning the NPC to a new building and unassigning if no job is selected.
/// </summary>
/// <param name="dropdown">The dropdown menu used to select the NPC's job.</param>
public void ChangeJob(TMP_Dropdown dropdown)
{
    Debug.Log("Job change initiated.");

    // Remove NPC from the current building, if assigned
    if (building != null && building.assignedNPCs.Contains(this))
    {
        building.assignedNPCs.Remove(this); // Remove NPC from the list in the current building
        building.UpdateBuildingUI(); // Update the UI of the building
    }

    // Handle "Unassigned" job selection
    if (dropdown.options[dropdown.value].text == "Unassigned")
    {
        job = string.Empty; // Clear the job
        npcJobText.text = "Job: Unassigned"; // Update the UI text
        building = null; // Clear the current building reference
        npcHapiness.StopIncreasingHappiness(); // Stop the happiness increase process
        StopEnergyGeneration(); // Stop energy generation process
    }
    else
    {
        // Assign the NPC to a new building based on the selected job
        job = dropdown.options[dropdown.value].text; // Set the new job
        npcJobText.text = "Job: " + job; // Update the UI text

        Building newBuilding = FindBuildingByJob(job); // Locate the building corresponding to the new job

        if (newBuilding != null)
        {
            // Check if the building has capacity
            if (newBuilding.assignedNPCs.Count >= newBuilding.maxCapacity)
            {
                Debug.Log("Building capacity reached: " + newBuilding.buildingType);
                job = string.Empty; // Reset the job
                npcJobText.text = "Job: Unassigned"; // Update UI text
                return; // Exit the function
            }

            // Assign the NPC to the new building
            building = newBuilding; // Update the current building reference
            newBuilding.assignedNPCs.Add(this); // Add NPC to the building's assigned list
            building.UpdateBuildingUI(); // Update the building UI
            MoveToAssignedBuilding(newBuilding); // Start NPC movement to the building

            npcHapiness.StartIncreasingHappiness(); // Start the happiness increase process
            StartEnergyGeneration(); // Start the energy generation process
        }
    }
}

private Coroutine energyGenerationCoroutine;

 /// <summary>
    /// Starts the NPC's energy generation process.
    /// </summary>
    private void StartEnergyGeneration()
    {
        if (energyGenerationCoroutine == null)
        {
            energyGenerationCoroutine = StartCoroutine(GenerateEnergyOverTime());
        }
    }

    /// <summary>
    /// Stops the NPC's energy generation process.
    /// </summary>
    private void StopEnergyGeneration()
    {
        if (energyGenerationCoroutine != null)
        {
            StopCoroutine(energyGenerationCoroutine);
            energyGenerationCoroutine = null;
        }
    }
 /// <summary>
    /// Generates energy over time based on the NPC's happiness.
    /// </summary>
    private IEnumerator GenerateEnergyOverTime()
    {
        while (!string.IsNullOrEmpty(job))
        {
            float happinessPercentage = npcHapiness.GetCurrentHappiness() / 100f;
            int energyGenerated = Mathf.CeilToInt(happinessPercentage * 5);
            GameManager.instance.AddEnergy(energyGenerated);

            Debug.Log($"{nPCtype} generated {energyGenerated} energy.");
            yield return new WaitForSeconds(10f);
        }
    }
    /// <summary>
    /// Moves the NPC to the assigned building's position.
    /// </summary>
    /// <param name="building">The building to move to.</param>
    private void MoveToAssignedBuilding(Building building)
    {
        if (building == null)
        {
            Debug.LogError("Building is null!");
            return;
        }

        Vector3 buildingPosition = building.transform.position;
        SetJobPosition(buildingPosition);
    }

    /// <summary>
    /// Finds a building in the scene by job type.
    /// </summary>
    /// <param name="job">The job name to search for.</param>
    /// <returns>The building matching the job name, or null if not found.</returns>
    private Building FindBuildingByJob(string job)
    {
        Building[] allBuildings = FindObjectsOfType<Building>();

        foreach (Building b in allBuildings)
        {
            if (b.buildingType.ToString() == job)
            {
                return b;
            }
        }

        return null;
    }
}