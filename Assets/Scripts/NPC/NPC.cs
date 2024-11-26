using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// NPC maneja el comportamiento y las interacciones de los personajes no jugadores en el juego.
/// Esto incluye asignaciones de trabajo, movimiento, gestión de la felicidad y generación de energía.
/// </summary>
public class NPC : MonoBehaviour
{
    // Variables públicas para la felicidad, ID y tipo de NPC
    public NPCHapiness npcHapiness;
    public int npcID;
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
    GameManager gameManager;

    /// <summary>
    /// Inicializa el NPC y oculta la interfaz de usuario al principio.    
    /// </summary>
    private void Start()
    {
        npcInfoUI.SetActive(false); 
        npcHapiness = GetComponent<NPCHapiness>(); 
        LoadNPCState(npcID);
    }

    /// <summary>
    /// Guarda la posición actual y el trabajo del NPC en PlayerPrefs.
/// </summary>
public void SaveNPCState(int npcID)
{
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosX", transform.position.x);
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosY", transform.position.y);
    PlayerPrefs.SetFloat($"NPC_{npcID}_PosZ", transform.position.z);

    PlayerPrefs.SetString($"NPC_{npcID}_Job", string.IsNullOrEmpty(job) ? "Unassigned" : job);

    PlayerPrefs.Save();
    Debug.Log($"NPC {npcID} state saved.");
}

/// <summary>
/// Carga la posición y el trabajo del NPC desde PlayerPrefs.
/// </summary>
public void LoadNPCState(int npcID)
{
    float posX = PlayerPrefs.GetFloat($"NPC_{npcID}_PosX", transform.position.x);
    float posY = PlayerPrefs.GetFloat($"NPC_{npcID}_PosY", transform.position.y);
    float posZ = PlayerPrefs.GetFloat($"NPC_{npcID}_PosZ", transform.position.z);
    transform.position = new Vector3(posX, posY, posZ);

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

        // Reiniciar generación de energía
        if (!string.IsNullOrEmpty(job))
        {
            StartEnergyGeneration(); // Inicia la generación de energía si el trabajo no está vacío
        }
    }

    Debug.Log($"NPC {npcID} state loaded. Job: {job}, Position: {transform.position}");
}

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player is nearby.");
        isPlayerNearby = true; // Marca que el jugador está cerca
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player left.");
        isPlayerNearby = false; // Marca que el jugador se ha alejado
        npcInfoUI.SetActive(false); 
    }
}

private void Update()
{
    if (isMovingToJob)
    {
        Debug.Log("NPC is moving...");
        MoveToTarget(); // Mueve al NPC hacia el objetivo
    }

    if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
    {
        if (npcInfoUI.activeSelf)
        {
            npcInfoUI.SetActive(false); // Oculta la información del NPC
        }
        else
        {
            Debug.Log("Showing NPC info.");
            ShowNPCInfo(); // Muestra la información del NPC
        }
    }
}

/// <summary>
/// Mueve al NPC hacia la posición objetivo asignada.
/// </summary>
private void MoveToTarget()
{
    if (targetPosition == null)
    {
        Debug.Log("Target position is not set!"); // Verifica si la posición objetivo está establecida
        return;
    }

    Vector3 horizontalTargetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
    float step = moveSpeed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, horizontalTargetPosition, step);

    if (Vector3.Distance(transform.position, horizontalTargetPosition) < 0.1f)
    {
        isMovingToJob = false; // Detiene el movimiento al llegar
        Debug.Log("NPC has arrived at the building.");
        SaveNPCState(npcID); // Guarda el estado del NPC al llegar
        npcHapiness.StartIncreasingHappiness(); // Inicia el aumento de felicidad
    }
}

public void SetJobPosition(Vector3 position)
{
    targetPosition = position;
    isMovingToJob = true; // Marca que el NPC se está moviendo hacia el trabajo
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
/// Muestra la interfaz de usuario de información de NPC e inicializa el menú desplegable del trabajo.    
/// </summary>
private void ShowNPCInfo()
{
    npcNameText.text = "Name: " + nPCtype.ToString();
    npcJobText.text = "Job: " + (string.IsNullOrEmpty(job) ? "Unassigned" : job);

    List<string> jobs = GetBuildingTypes();
    jobs.Insert(0, "Unassigned");

    jobDropdown.ClearOptions();
    jobDropdown.AddOptions(jobs);

    jobDropdown.value = string.IsNullOrEmpty(job) ? 0 : jobs.IndexOf(job);

    jobDropdown.onValueChanged.RemoveAllListeners();
    jobDropdown.onValueChanged.AddListener(delegate { ChangeJob(jobDropdown); });

    npcInfoUI.SetActive(true); // Muestra la interfaz de información del NPC
}

/// <summary>
/// Cambia el trabajo del NPC y actualiza la asignación del edificio en consecuencia.
/// Maneja tanto la asignación del NPC a un nuevo edificio como la desasignación si no se selecciona ningún trabajo.
/// </summary>
/// <param name="dropdown">El menú desplegable utilizado para seleccionar el trabajo del NPC.</param>
public void ChangeJob(TMP_Dropdown dropdown)
{
    Debug.Log("Job change initiated.");

    // Eliminar al NPC del edificio anterior, si está asignado
    if (building != null && building.assignedNPCs.Contains(this))
    {
        building.assignedNPCs.Remove(this);
        building.UpdateBuildingUI(); // Actualiza la interfaz del edificio
    }

    // Si se selecciona "Unassigned", desasignar el trabajo
    if (dropdown.options[dropdown.value].text == "Unassigned")
    {
        job = string.Empty; // Desasigna el trabajo
        npcJobText.text = "Job: Unassigned";
        building = null; // Limpia la referencia al edificio
        npcHapiness.StopIncreasingHappiness(); // Detiene el aumento de felicidad
        gameManager.StopEnergyGeneration(npcID); // Detiene la generación de energía
    }
    else
    {
        job = dropdown.options[dropdown.value].text; // Asigna el nuevo trabajo
        npcJobText.text = "Job: " + job;

        // Buscar el edificio asociado al trabajo seleccionado
        Building newBuilding = FindBuildingByJob(job);

        if (newBuilding != null)
        {
            // Verificar si el edificio está desbloqueado
            if (!newBuilding.isUnlocked)
            {
                Debug.LogWarning($"The building '{newBuilding.buildingType}' is locked. Cannot assign job.");
                job = string.Empty; // No asignar el trabajo
                npcJobText.text = "Job: Unassigned";
                return;
            }

            // Verificar si el edificio tiene capacidad disponible
            if (newBuilding.assignedNPCs.Count >= newBuilding.maxCapacity)
            {
                Debug.Log("Building capacity reached: " + newBuilding.buildingType);
                job = string.Empty; // No asignar el trabajo
                npcJobText.text = "Job: Unassigned";
                return;
            }

            // Asignar el trabajo al NPC y al edificio
            building = newBuilding;
            newBuilding.assignedNPCs.Add(this);
            building.UpdateBuildingUI(); // Actualiza la interfaz del edificio
            MoveToAssignedBuilding(newBuilding); // Mueve al NPC al edificio asignado

            npcHapiness.StartIncreasingHappiness(); // Inicia el aumento de felicidad
            StartEnergyGeneration(); // Inicia la generación de energía
        }
        else
        {
            Debug.LogWarning("Building not found for job: " + job);
            job = string.Empty; // No asignar el trabajo
            npcJobText.text = "Job: Unassigned";
        }
    }
}

private Coroutine energyGenerationCoroutine;

/// <summary>
/// Inicia el proceso de generación de energía del NPC.
/// </summary>
public void StartEnergyGeneration()
{
    if (!string.IsNullOrEmpty(job))
    {
        float happinessPercentage = npcHapiness.GetCurrentHappiness() / 100f;
        GameManager.instance.StartEnergyGeneration(npcID, happinessPercentage); // Inicia la generación de energía
    }
}

/// <summary>
/// Genera energía a lo largo del tiempo basado en la felicidad del NPC.
/// </summary>
   
/// <summary>
/// Mueve al NPC a la posición del edificio asignado.
/// </summary>
/// <param name="building">El edificio al que moverse.</param>
private void MoveToAssignedBuilding(Building building)
{
    if (building == null)
    {
        Debug.LogError("Building is null!"); // Verifica si el edificio es nulo
        return;
    }

    Vector3 buildingPosition = building.transform.position;
    SetJobPosition(buildingPosition); // Establece la posición del trabajo
}

/// <summary>
/// Busca un edificio en la escena por tipo de trabajo.
/// </summary>
/// <param name="job">El nombre del trabajo a buscar.</param>
/// <returns>El edificio que coincide con el nombre del trabajo, o null si no se encuentra.</returns>
private Building FindBuildingByJob(string job)
{
    Building[] allBuildings = FindObjectsOfType<Building>();

    foreach (Building b in allBuildings)
    {
        if (b.buildingType.ToString() == job)
        {
            return b; // Retorna el edificio encontrado
        }
    }

    return null; // Retorna null si no se encuentra el edificio
}
}
