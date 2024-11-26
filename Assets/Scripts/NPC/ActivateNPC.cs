using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNPC : MonoBehaviour
{
    [SerializeField] private GameObject[] npcObjects; 

    private void Start()
    {
        // Itera sobre cada objeto NPC en la lista
        foreach (GameObject npc in npcObjects)
        {
            // Verifica si el NPC no es nulo
            if (npc != null)
            {
                // Obtiene la etiqueta del NPC
                string npcTag = npc.tag; 
                // Comprueba si el NPC ha sido encontrado por el jugador
                if (PlayerPrefs.GetInt(npcTag + "Found", 0) == 1)
                {
                    // Activa el NPC si ha sido encontrado
                    npc.SetActive(true);
                    Debug.Log(npcTag + " activado");
                }
                else
                {
                    // Desactiva el NPC si no ha sido encontrado
                    npc.SetActive(false);
                    Debug.Log(npcTag + " desactivado");
                }
            }
            else
            {
                // Muestra un error si falta un NPC en la lista
                Debug.LogError("Falta asignar un NPC en la lista.");
            }
        }
    }
}
