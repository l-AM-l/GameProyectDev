using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// This script manages the behavior and properties of buildings in the game.
/// It handles unlocking buildings, assigning NPCs, and interacting with the player.
/// Additionally, it updates the UI with relevant information like building name,
/// assigned NPCs, and unlock status.
/// </summary>
public class Building : MonoBehaviour
{
    // Indicates if the player is near the building
    public bool isPlayerNearby = false;

    // Maximum capacity for NPCs in this building
    public int maxCapacity;

    // List of NPCs currently assigned to the building
    public List<NPC> assignedNPCs;

    // UI elements for building info
    public GameObject buildingInfoUI; // Panel displaying building details
    public TextMeshProUGUI buildingNameText; // Text displaying the building name
    public TextMeshProUGUI npcInfoText; // Text displaying NPC assignments

    // Building-specific properties
    public BuildingType buildingType; // Type of the building (e.g., resource, crafting)
    public int requiredEnergyToUnlock; // Energy required to unlock the building
    private bool isUnlocked = false; // Tracks if the building is unlocked

    // UI for unlocking the building
    public GameObject lockUI; // Panel for unlocking the building
    public TextMeshProUGUI costText; // Text displaying the unlock cost
    public Button unlockButton; // Button to unlock the building
  

    
    private void Start()
    {
        // Load the unlock state at the start
        buildingInfoUI.SetActive(false);
    LoadBuildingState();

    // Ensure UI elements are hidden at the start
    
    //lockUI.SetActive(!isUnlocked); // Show lock UI if the building is locked

    // Assign the unlock button's function
    unlockButton.onClick.AddListener(AttemptUnlockBuilding);
    }

    /// <summary>
    /// Unlocks the building if the player has enough energy.
    /// </summary>
    public void UnlockBuilding()
{
    if (!isUnlocked)
    {
        isUnlocked = true; // Mark the building as unlocked
        GameManager.instance.UseEnergy(requiredEnergyToUnlock); // Deduct the required energy
        SaveBuildingState(); // Save the unlock state
        Debug.Log("Building unlocked: " + buildingType.ToString());
        lockUI.SetActive(false); // Hide the unlock panel
    }
}   // <summary>
/// Saves the building's state to PersistentData or PlayerPrefs.
/// </summary>
private void SaveBuildingState()
{
    PlayerPrefs.SetInt($"{buildingType}_isUnlocked", isUnlocked ? 1 : 0);
    PlayerPrefs.Save(); // Ensure the data is written to disk
    
}
private void OnApplicationQuit()
{
    // Clear all PlayerPrefs data
    PlayerPrefs.DeleteAll();

    // Ensure the changes are saved immediately
    PlayerPrefs.Save();

    Debug.Log("All saved data cleared on application quit.");
}

/// <summary>
/// Loads the building's state from PersistentData or PlayerPrefs.
/// </summary>
private void LoadBuildingState()
{
    buildingInfoUI.SetActive(false);
    isUnlocked = PlayerPrefs.GetInt($"{buildingType}_isUnlocked", 0) == 1; // Default to locked
}

    /// <summary>
    /// Attempts to unlock the building when the button is pressed.
    /// Checks if the player has enough energy before unlocking.
    /// </summary>
    public void AttemptUnlockBuilding()
    {
        if (GameManager.instance.HasEnoughEnergy(requiredEnergyToUnlock))
        {
            UnlockBuilding();
        }
        else
        {
            Debug.Log("Not enough energy to unlock " + buildingType.ToString());
        }
    }

    /// <summary>
    /// Displays the unlock panel with the energy cost if the building is locked.
    /// </summary>
    private void ShowUnlockPanel()
    {
        lockUI.SetActive(true);
        costText.text = "Cost: " + requiredEnergyToUnlock + " energy";
    }
/// <summary>
    /// Hides the unlock panel.
    /// </summary>
    private void HideUnlockPanel()
    {
        lockUI.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("jugador cerca");
            isPlayerNearby = true;

            if (!isUnlocked) 
            {
               // ShowUnlockPanel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("jugador se alejó");
            isPlayerNearby = false;
            buildingInfoUI.SetActive(false);  
            HideUnlockPanel();  
        }
    }


    /// <summary>
    /// Updates the building's UI to reflect its current state, including
    /// the assigned NPCs and capacity.
    /// </summary>
    public void UpdateBuildingUI()
    {
        buildingNameText.text = "Building Name: " + this.name;
        npcInfoText.text = "Assigned NPCs: " + assignedNPCs.Count + "/" + maxCapacity;

        // Show additional NPC details
        if (assignedNPCs.Count > 0)
        {
            npcInfoText.text += "\n";
            foreach (NPC npc in assignedNPCs)
            {
                npcInfoText.text += npc.nPCtype.ToString() + "\n";
            }
        }
        else
        {
            npcInfoText.text += "\nNo NPCs assigned.";
        }

        if (assignedNPCs.Count >= maxCapacity)
        {
            Debug.Log("The building has reached maximum capacity.");
        }
    }

/// <summary>
    /// Toggles the display of building info or unlock panel based on the building's state.
    /// </summary>
    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isUnlocked)
            {
                // Toggle building info UI
                if (buildingInfoUI.activeSelf)
                {
                    buildingInfoUI.SetActive(false);
                }
                else
                {
                    ShowBuildingInfo();
                }
            }
            else
            {
                ShowUnlockPanel();
                Debug.Log("The building is locked.");
            }
        }
    }

     /// <summary>
    /// Displays detailed information about the building, including
    /// its name and assigned NPCs.
    /// </summary>
    private void ShowBuildingInfo()
    {
        buildingNameText.text = "Building Name: " + buildingType.ToString();
        npcInfoText.text = "Assigned NPCs: " + assignedNPCs.Count + "/" + maxCapacity;

        if (assignedNPCs.Count > 0)
        {
            npcInfoText.text += "\n";
            foreach (NPC npc in assignedNPCs)
            {
                npcInfoText.text += npc.name + "\n";
            }
        }

        buildingInfoUI.SetActive(true);
    }
}
