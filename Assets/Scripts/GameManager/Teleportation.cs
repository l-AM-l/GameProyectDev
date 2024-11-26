using System.Diagnostics;

using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

/// <summary>
/// La teletransportación gestiona las transiciones de escenas entre dos escenas predefinidas.
/// Incluye transiciones suaves con animaciones y evita múltiples transiciones simultáneas.
/// </summary>
public class Teleportation : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Name of the base scene to switch to.")]
    Building building;
    private string baseSceneName = "BaseScene"; 
    [Tooltip("Name of the exploration scene to switch to.")]
    private string explorationSceneName = "PlatformerScene"; 

    [Header("Input Settings")]
    [Tooltip("Key used to trigger scene transitions.")]
    public KeyCode transitionKey = KeyCode.T; 

    [Header("Transition Settings")]
    [Tooltip("Reference to the Animator controlling transition animations.")]
    public Animator transitionAnim; 
    GameManager gameManager;
    NPCHapiness hapiness;
    private bool isSwitching = false;     NPC npc;
    
    private void Update()
    {
        if (Input.GetKeyDown(transitionKey) && !isSwitching)
        {
            if (SceneManager.GetActiveScene().name == baseSceneName)
            {
                NPCManager npcManager = FindObjectOfType<NPCManager>();
        if (npcManager != null)
        {
            foreach (NPC npc in npcManager.GetComponentsInChildren<NPC>())
            {   
                gameManager.StartEnergyGeneration(npc.npcID, hapiness.happiness);
                npc.SaveNPCState(npc.npcID);
            }
        }      
                SwitchScene(explorationSceneName);
                
            }
            else if (SceneManager.GetActiveScene().name == explorationSceneName)
            {
               
                SwitchScene(baseSceneName);
                
                 
            }
        }
    }

   
    private void SwitchScene(string targetSceneName)
{
  

    // Realiza la transición de escena
    isSwitching = true;
    StartCoroutine(TransitionAndLoadScene(targetSceneName));
}


    private System.Collections.IEnumerator TransitionAndLoadScene(string targetSceneName)
{
    if (transitionAnim != null)
    {
        transitionAnim.SetTrigger("End");
    }

    yield return new WaitForSeconds(1f);

    var operation = SceneManager.LoadSceneAsync(targetSceneName);

    while (!operation.isDone)
    {
        yield return null;
    }

    // Cargar los datos después de la transición
   

    if (transitionAnim != null)
    {
        transitionAnim.SetTrigger("Start");
    }

    isSwitching = false;
}

}
