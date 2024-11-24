using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour
{
    public NPCHapiness npcHapiness;

    public NPCtype nPCtype; 
    public string job;  
    public GameObject npcInfoUI;  
    public TextMeshProUGUI npcNameText;  
    public TextMeshProUGUI npcJobText;  
    public TMP_Dropdown jobDropdown; 
    private Building building; 
     public Vector3 targetPosition;
    public float moveSpeed = 2f;
    private bool isMovingToJob = false;


    private bool isPlayerNearby = false;
     
    

    private void Start()
    {
     
        npcInfoUI.SetActive(false);  
        npcHapiness = GetComponent<NPCHapiness>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("jugador cerca");
            isPlayerNearby = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("jugador se fue");
            isPlayerNearby = false;
            npcInfoUI.SetActive(false);  
        }
    }

    private void Update()
    {
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
                Debug.Log("mostrar ui ");
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
    npcNameText.text = "Nombre: " + nPCtype.ToString();
    npcJobText.text = "Trabajo: " + (string.IsNullOrEmpty(job) ? "Sin trabajo" : job);

    List<string> jobs = GetBuildingTypes();
    jobs.Insert(0, "Sin trabajo"); 
    
    jobDropdown.ClearOptions();
    jobDropdown.AddOptions(jobs); 

    if (!string.IsNullOrEmpty(job))
    {
        jobDropdown.value = jobs.IndexOf(job);
    }
    else
    {
        jobDropdown.value = 0; 
    }

    // Registrar el cambio de trabajo
    jobDropdown.onValueChanged.RemoveAllListeners(); // Elimina los listeners anteriores
    jobDropdown.onValueChanged.AddListener(delegate { ChangeJob(jobDropdown); });

    npcInfoUI.SetActive(true);  // Mostrar la UI
}
    

   
    


  private void ChangeJob(TMP_Dropdown dropdown)
{
    Debug.Log("Cambio de trabajo exitoso");

    // Si el NPC ya está asignado a un edificio, lo removemos antes de asignarlo a otro
    if (building != null && building.assignedNPCs.Contains(this))
    {
        building.assignedNPCs.Remove(this);  
        building.UpdateBuildingUI(); 
    }

    if (dropdown.options[dropdown.value].text == "Sin trabajo")
{
    job = string.Empty;
    npcJobText.text = "Trabajo: Sin trabajo";
    building = null;
    npcHapiness.StopIncreasingHappiness();
    StopEnergyGeneration(); // Detener generación de energía
}
else
{
    job = dropdown.options[dropdown.value].text;
    npcJobText.text = "Trabajo: " + job;

    Building newBuilding = FindBuildingByJob(job);

    if (newBuilding != null)
    {
        if (newBuilding.assignedNPCs.Count >= newBuilding.maxCapacity)
        {
            job = string.Empty;
            npcJobText.text = "Trabajo: Sin trabajo";
            Debug.Log("Capacidad máxima alcanzada en " + newBuilding.buildingType);
            return;
        }

        building = newBuilding;
        newBuilding.assignedNPCs.Add(this);
        building.UpdateBuildingUI();
        MoveToAssignedBuilding(newBuilding);

        npcHapiness.StartIncreasingHappiness();
        StartEnergyGeneration(); // Iniciar generación de energía
    }
}
}
private Coroutine energyGenerationCoroutine;

private void StartEnergyGeneration()
{
    if (energyGenerationCoroutine == null)
    {
        energyGenerationCoroutine = StartCoroutine(GenerateEnergyOverTime());
    }
}

private void StopEnergyGeneration()
{
    if (energyGenerationCoroutine != null)
    {
        StopCoroutine(energyGenerationCoroutine);
        energyGenerationCoroutine = null;
    }
}

private IEnumerator GenerateEnergyOverTime()
{
    while (!string.IsNullOrEmpty(job)) // Solo genera energía si el NPC tiene un trabajo
    {
        // Cálculo de energía basado en felicidad
        float happinessPercentage = npcHapiness.GetCurrentHappiness() / 100f;
        int energyGenerated = Mathf.CeilToInt(happinessPercentage * 5); // 5 es la base de energía generada
        GameManager.instance.AddEnergy(energyGenerated);

        Debug.Log($"{nPCtype} generó {energyGenerated} de energía.");

        // Intervalo de generación (cada 5 segundos, por ejemplo)
        yield return new WaitForSeconds(10f);
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