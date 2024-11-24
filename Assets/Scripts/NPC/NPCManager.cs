using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private NPC[] allNPCs;

    private void Start()
    {
        // Find all NPCs in the scene
        allNPCs = FindObjectsOfType<NPC>();

        // Load all NPC states
        foreach (NPC npc in allNPCs)
        {
            npc.LoadNPCState(npc.npcID);
        }
    }

    private void OnApplicationQuit()
    {
        // Save all NPC states on application quit
        foreach (NPC npc in allNPCs)
        {
            npc.SaveNPCState(npc.npcID);
        }
    }
}
