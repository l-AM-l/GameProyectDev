using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public GameObject buildingInfoUI;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI npcInfoText;

    public void HideBuildingInfo()
    {
        buildingInfoUI.SetActive(false);
    }

    public void ToggleBuildingInfo(List<NPC> assignedNPCs, int maxCapacity, BuildingType buildingType)
    {
        if (buildingInfoUI.activeSelf)
        {
            HideBuildingInfo();
        }
        else
        {
            ShowBuildingInfo(assignedNPCs, maxCapacity, buildingType);
        }
    }
    public void UpdateBuildingUI(List<NPC> assignedNPCs, int maxCapacity, BuildingType buildingType)
    {
        buildingNameText.text =  name;
        npcInfoText.text = "Assigned NPCs: " + assignedNPCs.Count + "/" + maxCapacity;

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
       

    }

    private void ShowBuildingInfo(List<NPC> assignedNPCs, int maxCapacity, BuildingType buildingType)
    {
        buildingNameText.text = buildingType.ToString();
        npcInfoText.text = "Assigned NPC" + assignedNPCs.Count + "/" + maxCapacity;

        if (assignedNPCs.Count > 0)
        {
            npcInfoText.text += "\n";
            foreach (NPC npc in assignedNPCs)
            {
                npcInfoText.text += npc.name + "\n";
            }
        }
        else
        {
            npcInfoText.text += "\nNo hay NPCs asignados.";
        }

        buildingInfoUI.SetActive(true);
    }
}
