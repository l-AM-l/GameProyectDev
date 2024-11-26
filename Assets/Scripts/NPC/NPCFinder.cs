using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script gestiona la detección y el seguimiento de los NPCs en la escena.
/// Marca los NPCs encontrados y oculta aquellos que ya fueron destruidos previamente,
/// asegurando que su estado persista entre sesiones.
/// </summary>
public class NPCFinder : MonoBehaviour
{   
    private void Start()
    {
        // Encuentra todos los objetos en la escena
        foreach (GameObject npc in FindObjectsOfType<GameObject>())
        {
            // Si el NPC está marcado como destruido, desactívalo
            if (PlayerPrefs.GetInt(npc.name + "_Destroyed", 0) == 1)
            {
                npc.SetActive(false);
                Debug.Log(npc.name + " está marcado como destruido y no aparecerá.");
            }
        }
    }

    /// <summary>
    /// Detecta colisiones con objetos y marca el NPC como encontrado en PlayerPrefs.
    /// </summary>
    /// <param name="collision">El objeto con el que se ha colisionado.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Obtiene el tag del objeto colisionado
        string npcTag = collision.tag;

        // Verifica que el tag no esté vacío o nulo antes de guardarlo
        if (!string.IsNullOrEmpty(npcTag))
        {
            // Marca este NPC como encontrado en PlayerPrefs
            PlayerPrefs.SetInt(npcTag + "Found", 1);
            Debug.Log(npcTag + " encontrado");
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Limpia todos los datos de PlayerPrefs al cerrar la aplicación.
    /// </summary>
    private void OnApplicationQuit()
    {
        // Elimina todos los datos guardados en PlayerPrefs
        PlayerPrefs.DeleteAll();

        // Asegura que los cambios se guarden inmediatamente
        PlayerPrefs.Save();

        Debug.Log("Todos los datos guardados se eliminaron al salir de la aplicación.");
    }
}
