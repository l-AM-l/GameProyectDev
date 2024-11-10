using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingInteraction : MonoBehaviour
{
    public BuildingPanel buildingPanel;  // Referencia al panel de edificios
    public List<NPC> assignedNPCs;  // Lista de NPCs asignados a este edificio
    public Building buildingData;  // Datos del edificio

    // Método para mostrar la información del edificio cuando el jugador interactúa con él
    public void OnBuildingClicked()
    {
        buildingPanel.UpdateBuildingPanel(buildingData, assignedNPCs);
        Debug.Log("Edificio seleccionado: " + buildingData.buildingType.ToString());
    }

    // Método para asignar un NPC al edificio
    //public void AssignNPC(NPC npc)
    //{
    //    assignedNPCs.Add(npc);
    //    buildingPanel.UpdateBuildingPanel(buildingData, assignedNPCs);  // Actualizar el panel al asignar un NPC

    //}
}
