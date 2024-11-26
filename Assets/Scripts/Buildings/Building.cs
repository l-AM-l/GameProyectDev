using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Este script gestiona el comportamiento y propiedades de los edificios en el juego.
/// Incluye funcionalidades para desbloquear edificios, asignar NPCs, interactuar con el jugador
/// y actualizar la interfaz de usuario (UI).
/// </summary>
public class Building : MonoBehaviour
{
    // Indica si el jugador está cerca del edificio
    public bool isPlayerNearby = false;

    // Capacidad máxima de NPCs en este edificio
    public int maxCapacity;

    // Lista de NPCs actualmente asignados al edificio
    public List<NPC> assignedNPCs;

    // Elementos UI para información del edificio
    public GameObject buildingInfoUI; // Panel para mostrar detalles del edificio
    public TextMeshProUGUI buildingNameText; // Nombre del edificio
    public TextMeshProUGUI npcInfoText; // Información de los NPCs asignados

    // Propiedades específicas del edificio
    public BuildingType buildingType; // Tipo de edificio (e.g., recurso, creación)
    public int requiredEnergyToUnlock; // Energía requerida para desbloquear el edificio
    public bool isUnlocked = false; // Estado de desbloqueo del edificio

    // UI para desbloquear el edificio
    public GameObject lockUI; // Panel de desbloqueo
    public TextMeshProUGUI costText; // Costo de energía para desbloqueo
    public Button unlockButton; // Botón para desbloquear el edificio

    private void Start()
    {
        // Inicializa el estado del edificio y asigna funcionalidad al botón de desbloqueo
        buildingInfoUI.SetActive(false);
        LoadBuildingState();
        unlockButton.onClick.AddListener(AttemptUnlockBuilding);
    }

    /// <summary>
    /// Desbloquea el edificio si el jugador tiene suficiente energía.
    /// </summary>
    public void UnlockBuilding()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            GameManager.instance.UseEnergy(requiredEnergyToUnlock);
            SaveBuildingState();
            Debug.Log("Building unlocked: " + buildingType.ToString());
            lockUI.SetActive(false);
            SaveBuildingState();
        }
    }

    /// <summary>
    /// Guarda el estado del edificio en PlayerPrefs.
    /// </summary>
    private void SaveBuildingState()
    {
        PlayerPrefs.SetInt($"{buildingType}_isUnlocked", isUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        // Limpia los datos guardados al salir del juego
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All saved data cleared on application quit.");
    }

    /// <summary>
    /// Carga el estado del edificio desde PlayerPrefs.
    /// </summary>
    private void LoadBuildingState()
    {
        buildingInfoUI.SetActive(false);
        isUnlocked = PlayerPrefs.GetInt($"{buildingType}_isUnlocked", 0) == 1;
    }

    /// <summary>
    /// Intenta desbloquear el edificio verificando la energía del jugador.
    /// </summary>
    public void AttemptUnlockBuilding()
    {
        if (GameManager.instance.HasEnoughEnergy(requiredEnergyToUnlock))
        {
            UnlockBuilding();
            SaveBuildingState();
        }
        else
        {
            Debug.Log("Not enough energy to unlock " + buildingType.ToString());
        }
    }

    /// <summary>
    /// Muestra el panel de desbloqueo con el costo necesario.
    /// </summary>
    private void ShowUnlockPanel()
    {
        lockUI.SetActive(true);
        costText.text = "Cost: " + requiredEnergyToUnlock + " energy";
    }

    /// <summary>
    /// Oculta el panel de desbloqueo.
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
                ShowUnlockPanel();
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
    /// Actualiza la UI del edificio con su estado actual y detalles de los NPCs asignados.
    /// </summary>
    public void UpdateBuildingUI()
    {
        buildingNameText.text = "Building Name: " + this.name;
        npcInfoText.text = "Assigned NPCs: " + assignedNPCs.Count + "/" + maxCapacity;

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
    /// Alterna entre mostrar información del edificio o el panel de desbloqueo según su estado.
    /// </summary>
    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isUnlocked)
            {
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
    /// Muestra información detallada del edificio, como su nombre y NPCs asignados.
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
