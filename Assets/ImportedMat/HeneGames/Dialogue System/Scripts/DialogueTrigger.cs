using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeneGames.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent startDialogueEvent;
        public UnityEvent nextSentenceDialogueEvent;
        public UnityEvent endDialogueEvent;
        
        
        public GameObject newCharacterPrefab;

    // Posición donde aparecerá el nuevo personaje
    public Transform spawnPoint;



    private IEnumerator DestroyAndSpawnAfterDelay()
    {
        // Espera 2 segundos antes de destruir el personaje
       
        // Espera un momento antes de instanciar el nuevo personaje (opcional)
        yield return new WaitForSeconds(0.5f);

        // Instancia el nuevo personaje en la posición especificada
        if (newCharacterPrefab != null && spawnPoint != null)
        {
            newCharacterPrefab.SetActive(true);
        }
        else
        {
            Debug.LogWarning("El prefab o el punto de spawn no están configurados.");
        }
    }
}
}