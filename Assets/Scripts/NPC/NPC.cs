using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public NPCHapiness npcHapiness;

    public NPCtype nPCtype; // Tipo de NPC
    public string job;  // Trabajo del NPC
    public GameObject npcInfoUI;  // UI para mostrar la info del NPC
    public TextMeshProUGUI npcNameText;  // Texto para mostrar el nombre del NPC
    public TextMeshProUGUI npcJobText;  // Texto para mostrar el trabajo del NPC
    public TMP_Dropdown jobDropdown; // Dropdown para seleccionar el trabajo
    private Building assignedBuilding; // Edificio asignado al NPC
    private bool isPlayerNearby = false; // Si el jugador está cerca del NPC
    private BuildingUI ui;
    //public NPCHapiness happinessManager;

    //berny
    public Vector3 targetPosition;
    public float moveSpeed = 2f;
    private bool isMovingToJob = false;

 
    private void Start()
    {

        npcInfoUI.SetActive(false);  // Esconder la UI al inicio
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
            npcInfoUI.SetActive(false);  // Esconder la UI cuando el jugador se va
        }
    }

    private void Update()
    {
        //berns
        if(isMovingToJob )
        {
            Debug.Log("Npc is moving...");
            MoveToTarget();
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (npcInfoUI.activeSelf)
            {
                npcInfoUI.SetActive(false);  
            }
            else
            {
                Debug.Log("Mostrar información del NPC");
                ShowNPCInfo();  
            }
        }
    }

    private void MoveToTarget()
    {
        if(targetPosition == null)
        {
            Debug.Log("Target position is not set!");
            return;
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
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
        Debug.Log("Npc is moving to job at position: " + targetPosition);
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

    private void ShowNPCInfo()
{
    npcNameText.text = nPCtype.ToString();
    npcJobText.text =  string.IsNullOrEmpty(job) ? "Unemployed" : job;

    List<string> jobs = GetBuildingTypes();
    jobs.Insert(0, "Unemployed"); 
    
    jobDropdown.ClearOptions();
    jobDropdown.AddOptions(jobs); 

    // Setear el valor actual en el dropdown
    jobDropdown.value = string.IsNullOrEmpty(job) ? 0 : jobs.IndexOf(job);

    // Registrar el cambio de trabajo
    jobDropdown.onValueChanged.RemoveAllListeners(); // Elimina los listeners anteriores
    jobDropdown.onValueChanged.AddListener(delegate { ChangeJob(jobDropdown); });

    npcInfoUI.SetActive(true);  // Mostrar la UI
}
private void ChangeJob(TMP_Dropdown dropdown)
{
    Debug.Log("Cambio de trabajo solicitado");

    string selectedJob = dropdown.options[dropdown.value].text;

    // Verifica si el trabajo es "Sin trabajo"
    if (selectedJob == "Unemployed")
    {
        job = string.Empty;
        npcJobText.text = "Unemployed";
        assignedBuilding = null;
        npcHapiness.StopIncreasingHappiness();
        return;
    }

    // Busca el edificio por trabajo
    Building newBuilding = FindBuildingByJob(selectedJob);

    if (newBuilding != null)
    {
        // Si el edificio no está desbloqueado
        if (!newBuilding.isUnlocked)
        {
            job = string.Empty;
            npcJobText.text = "Unemployed";
            assignedBuilding = null;
            Debug.Log("El edificio " + selectedJob + " aún no está desbloqueado.");
            // Aquí puedes mostrar un mensaje al jugador, si lo deseas.
            return;
        }

        // Verifica la capacidad del edificio
        if (newBuilding.assignedNPCs.Count >= newBuilding.maxCapacity)
        {
            Debug.Log("Capacidad máxima alcanzada en " + newBuilding.buildingType.ToString());
            job = string.Empty;
            npcJobText.text = "Unemployed";
            return;
        }

        // Asigna el trabajo al NPC
        job = selectedJob;
        npcJobText.text =  job;
        assignedBuilding = newBuilding;

        newBuilding.assignedNPCs.Add(this);
        MoveToAssignedBuilding(newBuilding);
            

        // Actualiza la UI del edificio, si es necesario
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

    private void MoveToAssignedBuilding(Building building)
    {
        if(building == null)
        {
            Debug.LogError("Building is null!!!");
            return;
        }
        Vector3 buildingPosition = building.transform.position;
        SetJobPosition(buildingPosition);
        Debug.Log("Npc moviendose hacia " + building.buildingType.ToString() + "en la posicion" + buildingPosition);
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

        return null;  // No se encontró ningún edificio que coincida
    }
}
