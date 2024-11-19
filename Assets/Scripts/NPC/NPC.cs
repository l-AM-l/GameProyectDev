using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public NPCHapiness npcHapiness;

    public NPCtype nPCtype;
    public string job;
    public GameObject npcInfoUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI npcJobText;
    public TMP_Dropdown jobDropdown;
    private Building assignedBuilding;
    private bool isPlayerNearby = false;
    private BuildingUI ui;

    public Vector3 targetPosition;
    public float moveSpeed = 2f;
    private bool isMovingToJob = false;

    private void Start()
    {
        npcInfoUI.SetActive(false);
        npcHapiness = GetComponent<NPCHapiness>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador cerca del NPC");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador se fue");
            isPlayerNearby = false;
            npcInfoUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isMovingToJob)
        {
            MoveToTarget();
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleNPCInfoUI();
        }
    }

    private void MoveToTarget()
    {
        if (targetPosition == null)
        {
            Debug.Log("Target position is not set!");
            return;
        }

        // Keep the Y position constant
        Vector3 horizontalTargetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, horizontalTargetPosition, step);

        if (Vector3.Distance(transform.position, horizontalTargetPosition) < 0.1f)
        {
            isMovingToJob = false;
            Debug.Log("NPC has arrived to the building");
            npcHapiness.StartIncreasingHappiness();
        }
    }

    public void SetJobPosition(Vector3 position)
    {
        targetPosition = position;
        isMovingToJob = true;
        Debug.Log("Npc is moving horizontally to job at position: " + targetPosition);
    }

    private void ToggleNPCInfoUI()
    {
        npcInfoUI.SetActive(!npcInfoUI.activeSelf);
        if (npcInfoUI.activeSelf)
        {
            ShowNPCInfo();
        }
    }

    private void ShowNPCInfo()
    {
        npcNameText.text = nPCtype.ToString();
        npcJobText.text = string.IsNullOrEmpty(job) ? "Unemployed" : job;

        List<string> jobs = GetBuildingTypes();
        jobs.Insert(0, "Unemployed");
        
        jobDropdown.ClearOptions();
        jobDropdown.AddOptions(jobs);
        jobDropdown.value = string.IsNullOrEmpty(job) ? 0 : jobs.IndexOf(job);

        jobDropdown.onValueChanged.RemoveAllListeners();
        jobDropdown.onValueChanged.AddListener(delegate { ChangeJob(jobDropdown); });

        npcInfoUI.SetActive(true);
    }

    private void ChangeJob(TMP_Dropdown dropdown)
    {
        Debug.Log("Cambio de trabajo solicitado");

        string selectedJob = dropdown.options[dropdown.value].text;

        if (selectedJob == "Unemployed")
        {
            job = string.Empty;
            npcJobText.text = "Unemployed";
            assignedBuilding = null;
            npcHapiness.StopIncreasingHappiness();
            return;
        }

        Building newBuilding = FindBuildingByJob(selectedJob);

        if (newBuilding != null)
        {
            if (!newBuilding.isUnlocked)
            {
                job = string.Empty;
                npcJobText.text = "Unemployed";
                assignedBuilding = null;
                Debug.Log("El edificio " + selectedJob + " aún no está desbloqueado.");
                return;
            }

            if (newBuilding.assignedNPCs.Count >= newBuilding.maxCapacity)
            {
                Debug.Log("Capacidad máxima alcanzada en " + newBuilding.buildingType.ToString());
                job = string.Empty;
                npcJobText.text = "Unemployed";
                return;
            }

            job = selectedJob;
            npcJobText.text = job;
            assignedBuilding = newBuilding;

            newBuilding.assignedNPCs.Add(this);
            SetJobPosition(newBuilding.transform.position);

            if (ui != null)
            {
                ui.UpdateBuildingUI(newBuilding.assignedNPCs, newBuilding.maxCapacity, newBuilding.buildingType);
            }
        }
        else
        {
            Debug.Log("No se encontró ningún edificio que coincida con el trabajo: " + selectedJob);
        }
    }

    public List<string> GetBuildingTypes()
    {
        List<string> buildingTypes = new List<string>();
        foreach (BuildingType type in System.Enum.GetValues(typeof(BuildingType)))
        {
            buildingTypes.Add(type.ToString());
        }
        return buildingTypes;
    }

    private Building FindBuildingByJob(string job)
    {
        Building[] allBuildings = FindObjectsOfType<Building>();
        
        foreach (Building building in allBuildings)
        {
            if (building.buildingType.ToString() == job)
            {
                return building;
            }
        }

        return null;
    }
}
