using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Building : MonoBehaviour
{
    public bool isPlayerNearby = false;
    public int maxCapacity;
    public List<NPC> assignedNPCs;
    public GameObject buildingInfoUI; 
    public TextMeshProUGUI buildingNameText; 
    public TextMeshProUGUI npcInfoText; 
    public BuildingType buildingType; 
    public Sprite buildingIcon;
    public int requiredEnergyToUnlock;
    private bool isUnlocked = false;

    // Panel para desbloquear el edificio
    public GameObject lockUI; 
    public TextMeshProUGUI costText; 
    public Button unlockButton; // Botón para desbloquear el edificio

    private void Start()
    {
        buildingInfoUI.SetActive(false);
        lockUI.SetActive(false); // Asegúrate de que el panel de desbloqueo esté oculto al inicio

        // Comprobamos si el edificio está desbloqueado desde el principio
       
        // Asignar la función al botón
        unlockButton.onClick.AddListener(AttemptUnlockBuilding);
    }

    // Método para desbloquear el edificio
public void UnlockBuilding()
{
    if (!isUnlocked)
    {
        isUnlocked = true; // Desbloquea este edificio específico
        GameManager.instance.UseEnergy(requiredEnergyToUnlock); // Reduce la energía necesaria
        Debug.Log("Edificio desbloqueado: " + buildingType.ToString());
        lockUI.SetActive(false); // Oculta el panel de desbloqueo de este edificio
    }
}

    // Intentar desbloquear el edificio cuando se presiona el botón
    public void AttemptUnlockBuilding()
    {
        if (GameManager.instance.HasEnoughEnergy(requiredEnergyToUnlock))
        {
            UnlockBuilding();
        }
        else
        {
            Debug.Log("No tienes suficiente energía para desbloquear " + buildingType.ToString());
        }
    }

    // Mostrar el panel de desbloqueo cuando el edificio esté bloqueado
    private void ShowUnlockPanel()
    {
        lockUI.SetActive(true);
        costText.text = "Costo: " + requiredEnergyToUnlock + " energía";
    }

    // Ocultar el panel de desbloqueo
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

            if (!isUnlocked) // Si el edificio está bloqueado, muestra el panel de desbloqueo
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
            buildingInfoUI.SetActive(false);  // Cierra la UI si el jugador se aleja
            HideUnlockPanel();  // Cierra el panel de desbloqueo
        }
    }


    public void UpdateBuildingUI()
    {
        buildingNameText.text = "Nombre del Edificio: " + this.name;
        npcInfoText.text = "NPCs Asignados: " + assignedNPCs.Count + "/" + maxCapacity;

        if (assignedNPCs.Count >= maxCapacity)
    {
        Debug.Log("El edificio ha alcanzado su capacidad máxima.");
    }
    else{
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
            npcInfoText.text += "\nNo hay NPCs asignados.";
        }
    }
        // Si hay NPCs asignados, muestra sus nombres
       

    }

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
                Debug.Log("El edificio no está desbloqueado.");
            }
        }
    }

    private void ShowBuildingInfo()
    {
        if (!isPlayerNearby)
        {
            Debug.Log("mostrar datos de edificio");
        }

        buildingNameText.text = "Nombre del Edificio: " + buildingType.ToString();
        npcInfoText.text = "NPCs Asignados: " + assignedNPCs.Count + "/" + maxCapacity;


        // Si hay NPCs asignados, muestra sus nombres
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
