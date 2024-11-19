using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
        public GameObject buildingPanel;  // Referencia al panel completo que se mostrará o ocultará

    public TextMeshProUGUI buildingNameText;  // Texto para el nombre del edificio
    public TextMeshProUGUI productionText;  // Texto para la producción del edificio
    public Transform npcListParent;  // Contenedor para la lista de NPCs asignados
    public GameObject npcListItemPrefab;  // Prefab para mostrar los NPCs asignados
    public Image buildingIcon;  // Icono del edificio

     private bool isPanelVisible = false; // Para controlar si el panel está visible o no

    private Building currentBuilding;  // Edificio actualmente seleccionado

    // Método para actualizar el panel cuando se selecciona un edificio
    public void UpdateBuildingPanel(Building building, List<NPC> assignedNPCs)
    {
        currentBuilding = building;
        buildingNameText.text = building.buildingType.ToString();
       // productionText.text = "Producción: " + building.cost.ToString();  // Cambiar esto por la lógica de producción

        // Establecer icono del edificio
        buildingIcon.sprite = building.buildingIcon;

        // Limpiar lista de NPCs actuales en la UI
        foreach (Transform child in npcListParent)
        {
            Destroy(child.gameObject);
        }

        // Agregar cada NPC asignado a la lista en la UI
        foreach (NPC npc in assignedNPCs)
        {
            GameObject npcItem = Instantiate(npcListItemPrefab, npcListParent);  // Instancia un prefab para cada NPC
            npcItem.GetComponentInChildren<TextMeshProUGUI>().text = npc.npcNameText.text;  // Establece el nombre del NPC en el prefab
        }
    }
}
