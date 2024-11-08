using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUnlockSystem : MonoBehaviour
{
    public GameObject lockUI;
    public TextMeshProUGUI costText;
    public Button unlockButton;

    private Building building;

    public void InitializeUnlockSystem(Building building)
    {
        this.building = building;
        lockUI.SetActive(false); // Ocultar UI al inicio
        unlockButton.onClick.AddListener(AttemptUnlockBuilding); // Asignar evento de desbloqueo
    }

    public void ShowUnlockPanel()
    {
        lockUI.SetActive(true);
        costText.text = "Cost: " + building.requiredEnergyToUnlock + " energy";
    }

    public void HideUnlockPanel()
    {
        lockUI.SetActive(false);
    }

    private void AttemptUnlockBuilding()
    {
        if (GameManager.instance.HasEnoughEnergy(building.requiredEnergyToUnlock))
        {
            building.UnlockBuilding();
            GameManager.instance.UseEnergy(building.requiredEnergyToUnlock);
            HideUnlockPanel();
        }
        else
        {
            Debug.Log("No tienes suficiente energía para desbloquear " + building.buildingType.ToString());
        }
    }
}
