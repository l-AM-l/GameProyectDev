using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEncounter : MonoBehaviour
{
    private HashSet<string> encounteredTags = new HashSet<string>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;
        if (tag == "Tutorial" && !encounteredTags.Contains(tag))
        {
            EventManager.TriggerEncounter(tag);
            encounteredTags.Add(tag);  // Agregar la etiqueta a la lista de encuentros
        }
    }
}
