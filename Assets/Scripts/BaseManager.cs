using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public GameObject[] npcs;     public NPCtype nPCtype;

   void Start()
{
    string rescuedNPCName = PlayerPrefs.GetString("rescuedNPC", string.Empty);
    Debug.Log("NPC rescatado: " + rescuedNPCName); // Verifica el nombre recuperado

    foreach (GameObject npcObject in npcs)
    {
        Debug.Log("Verificando objeto: " + npcObject.name); // Verifica el nombre del objeto
        NPC npc = npcObject.GetComponent<NPC>();
        if (npc != null)
        {
            if (npc.nPCtype.ToString() == rescuedNPCName)
            {
                Debug.Log("NPC activado: " + npc.nPCtype);
                npcObject.SetActive(true);
                break; // Salir después de activar el NPC correcto
            }
            else
            {
                Debug.Log("No coincide: " + npc.nPCtype);
            }
        }
        else
        {
            Debug.Log("Es nulo: " + npcObject.name);
        }
    }
}


}
