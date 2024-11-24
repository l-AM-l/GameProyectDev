using System.Collections;
using System.Collections.Generic;
using HeneGames.DialogueSystem;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
  void Start()
{
    // Verifica si hay un personaje que debe activarse
    string characterToActivate = GameManager.instance.characterToActivate;

    if (!string.IsNullOrEmpty(characterToActivate))
    {
        // Busca el personaje por su tag en la escena base
        GameObject character = GameObject.FindGameObjectWithTag(characterToActivate);

        if (character != null)
        {
            character.SetActive(true); // Activa el personaje en la escena base
        }

        // Opcional: Limpia el estado después de activar el personaje
        GameManager.instance.characterToActivate = null;
    }
}



}
