using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private NPC[] allNPCs;

    private void Start()
    {
        // Encontrar todos los NPCs en la escena
        allNPCs = FindObjectsOfType<NPC>();

        // Cargar todos los estados de los NPCs
        foreach (NPC npc in allNPCs)
        {
            npc.LoadNPCState(npc.npcID); // Cargar el estado del NPC usando su ID
        }
    }

    private void OnApplicationQuit()
    {
        // Guardar todos los estados de los NPCs al cerrar la aplicación
        foreach (NPC npc in allNPCs)
        {
            npc.SaveNPCState(npc.npcID); // Guardar el estado del NPC usando su ID
        }
    }
}
