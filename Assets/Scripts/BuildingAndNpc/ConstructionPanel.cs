using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{
     public GameObject  constructionPanel;
    public GameObject buildingButtonPrefab;  // Prefab del botón de construcción
    public Transform buttonParent;  // Contenedor donde se colocarán los botones de edificios
    public List<Building> availableBuildings;  // Lista de edificios disponibles para construir
         private bool isPanelVisible = false; // Para controlar si el panel está visible o no

    private void Start()
    {
        PopulateConstructionPanel();
    }

     void Update()
    {
        // Detectar si la tecla Tab ha sido presionada
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Alternar el estado del panel
            isPanelVisible = !isPanelVisible;

            // Activar o desactivar el panel en función de su estado actual
            Debug.Log("ui building actuvado");
            constructionPanel.SetActive(isPanelVisible);
        }
    }

    // Método para llenar el panel de construcción con los edificios disponibles
    void PopulateConstructionPanel()
{
    // Limpiar los botones existentes en el panel antes de instanciar nuevos
    foreach (Transform child in buttonParent)
    {
        Destroy(child.gameObject);  // Eliminar todos los botones antiguos
    }

    // Instanciar nuevos botones para cada edificio disponible
    foreach (Building building in availableBuildings)
    {
        GameObject newButton = Instantiate(buildingButtonPrefab, buttonParent);  // Instancia un nuevo botón
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = building.buildingType.ToString();  // Establece el nombre del edificio en el botón
        newButton.GetComponent<Button>().onClick.AddListener(() => OnBuildingSelected(building));  // Añade el listener para seleccionar el edificio
    }
}

    // Método llamado al seleccionar un edificio para construir
    public void OnBuildingSelected(Building building)
    {
        Debug.Log("Edificio seleccionado para construir: " + building.buildingType.ToString());
        // Aquí podrías implementar la lógica de construcción
    }
}

