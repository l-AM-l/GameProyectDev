using System.Diagnostics;

using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

/// <summary>
/// Teleportation manages scene transitions between two predefined scenes.
/// It includes smooth transitions with animations and prevents multiple simultaneous transitions.
/// </summary>
public class Teleportation : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Name of the base scene to switch to.")]
    Building building;
    private string baseSceneName = "BaseScene"; // Name of the base scene
    [Tooltip("Name of the exploration scene to switch to.")]
    private string explorationSceneName = "PlatformerScene"; // Name of the exploration scene

    [Header("Input Settings")]
    [Tooltip("Key used to trigger scene transitions.")]
    public KeyCode transitionKey = KeyCode.T; // Key to switch between scenes

    [Header("Transition Settings")]
    [Tooltip("Reference to the Animator controlling transition animations.")]
    public Animator transitionAnim; // Reference to the Animator for transitions
    private bool isSwitching = false; // Prevents multiple transitions at the same time

    /// <summary>
    /// Detects input to switch between scenes and initiates the transition process.
    /// </summary>
    private void Update()
    {
        // Check if the transition key is pressed and no transition is currently active
        if (Input.GetKeyDown(transitionKey) && !isSwitching)
        {
            // Switch to the appropriate scene based on the current scene
            if (SceneManager.GetActiveScene().name == baseSceneName)
            {
                NPCManager npcManager = FindObjectOfType<NPCManager>();
        if (npcManager != null)
        {
            foreach (NPC npc in npcManager.GetComponentsInChildren<NPC>())
            {
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

    /// <summary>
    /// Initiates the scene switching process to the target scene.
    /// </summary>
    /// <param name="targetSceneName">The name of the scene to switch to.</param>
    private void SwitchScene(string targetSceneName)
{
    // Guarda los datos antes de cambiar de escena
    //SaveStorage.instance.GetSaveByFileName("SaveFileName").Save();

    // Realiza la transición de escena
    isSwitching = true;
    StartCoroutine(TransitionAndLoadScene(targetSceneName));
}


    /// <summary>
    /// Handles the transition animation and asynchronous loading of the target scene.
    /// </summary>
    /// <param name="targetSceneName">The name of the scene to switch to.</param>
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
    //SaveStorage.instance.GetSaveByFileName("SaveFileName").Load();

    if (transitionAnim != null)
    {
        transitionAnim.SetTrigger("Start");
    }

    isSwitching = false;
}

}
