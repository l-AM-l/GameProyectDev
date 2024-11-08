using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool isPlayerNearby = false;
    public int maxCapacity;
    public List<NPC> assignedNPCs;
    public BuildingType buildingType;
    public int requiredEnergyToUnlock;
    public bool isUnlocked = false;

    private BuildingUI buildingUI;
    private BuildingUnlockSystem unlockSystem;

    private void Start()
    {
        buildingUI = GetComponent<BuildingUI>();
        unlockSystem = GetComponent<BuildingUnlockSystem>();

        unlockSystem.InitializeUnlockSystem(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            /*if (!isUnlocked)
            {
                unlockSystem.ShowUnlockPanel();
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            buildingUI.HideBuildingInfo();
            unlockSystem.HideUnlockPanel();
        }
    }
    

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isUnlocked)
            {
                buildingUI.ToggleBuildingInfo(assignedNPCs, maxCapacity, buildingType);
            }
            else
            {
                unlockSystem.ShowUnlockPanel();
            }
        }
    }

    public void UnlockBuilding()
    {
        isUnlocked = true;
    }
}
