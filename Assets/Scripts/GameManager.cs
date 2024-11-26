
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// GameManager gestiona los estados globales del juego, incluyendo la gestión de energía
/// y asegurando que el objeto del juego persista a través de las escenas. 
/// Proporciona métodos útiles para interactuar con los niveles de energía y rastrea qué personaje activar en las escenas.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Instancia singleton del GameManager
    public static GameManager instance;

    // Propiedades del sistema de energía
    public int maxEnergy = 100; // Valor máximo de energía
    public Slider energySlider; // Control deslizante de UI para representar la energía actual
    public TextMeshProUGUI energyText; // Texto de UI para mostrar la energía actual como un valor

    /// <summary>
    /// Inicializa el control deslizante de energía y actualiza la UI de energía cuando comienza el juego.
    /// </summary>
    private Dictionary<int, Coroutine> npcEnergyCoroutines = new Dictionary<int, Coroutine>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartEnergyGeneration(int npcID, float happinessPercentage)
    {
        if (!npcEnergyCoroutines.ContainsKey(npcID))
        {
            // Inicia la generación de energía a lo largo del tiempo para el NPC especificado
            npcEnergyCoroutines[npcID] = StartCoroutine(GenerateEnergyOverTime(npcID, happinessPercentage));
        }
    }

    public void StopEnergyGeneration(int npcID)
    {
        if (npcEnergyCoroutines.ContainsKey(npcID))
        {
            // Detiene la generación de energía para el NPC especificado
            StopCoroutine(npcEnergyCoroutines[npcID]);
            npcEnergyCoroutines.Remove(npcID);
        }
    }

    private IEnumerator GenerateEnergyOverTime(int npcID, float happinessPercentage)
    {
        while (true)
        {
            int energyGenerated = Mathf.CeilToInt(happinessPercentage * 5);
            AddEnergy(energyGenerated);

            Debug.Log($"NPC {npcID} generó {energyGenerated} energía.");
            yield return new WaitForSeconds(5f); // Tiempo ajustable
        }
    }

    public void AddEnergy(int amount)
    {
        maxEnergy = Mathf.Min(maxEnergy + amount, 100);
        UpdateEnergyUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualiza referencias de UI
        energySlider = GameObject.FindWithTag("EnergySlider")?.GetComponent<Slider>();
        energyText = GameObject.FindWithTag("EnergyText")?.GetComponent<TextMeshProUGUI>();

        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
        }
        else
        {
            Debug.LogWarning("Control deslizante de energía no encontrado en la nueva escena.");
        }

        if (energyText != null)
        {
            energyText.text = "Energía: " + maxEnergy;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Actualiza el control deslizante de energía y el texto de la UI para reflejar el nivel actual de energía.
    /// </summary>
    public void UpdateEnergyUI()
    {
        if (energySlider != null)
        {
            energySlider.value = maxEnergy;
            Debug.Log($"Control deslizante actualizado: {energySlider.value}");
        }
        else
        {
            Debug.LogWarning("Control deslizante de energía no asignado o encontrado.");
        }

        if (energyText != null)
        {
            energyText.text = "Energía: " + maxEnergy;
        }
        else
        {
            Debug.LogWarning("Texto de energía no asignado.");
        }
    }

    /// <summary>
    /// Asegura que solo haya una instancia de GameManager y la hace persistente a través de las escenas.
    /// Destruye instancias duplicadas si existen.
    /// </summary>

    /// <summary>
    /// Verifica si el jugador tiene suficiente energía para un requisito especificado.
    /// </summary>
    /// <param name="requiredEnergy">La energía requerida para la acción.</param>
    /// <returns>Verdadero si el jugador tiene suficiente energía; de lo contrario, falso.</returns>
    public bool HasEnoughEnergy(int requiredEnergy)
    {
        return maxEnergy >= requiredEnergy;
    }

    /// <summary>
    /// Reduce la energía del jugador por la cantidad especificada, asegurando que no baje de 0.
    /// Actualiza la UI de energía después del consumo.
    /// </summary>
    /// <param name="amount">La cantidad de energía a usar.</param>
    public void UseEnergy(int amount)
    {
        if (maxEnergy <= 0)
        {
            maxEnergy = 0;
        }
        else
        {
            maxEnergy -= amount;
        }

        Debug.Log($"Energía usada: {amount}, restante: {maxEnergy}");
        UpdateEnergyUI();
    }

    /// <summary>
    /// Aumenta la energía del jugador por la cantidad especificada, asegurando que no exceda el límite máximo de energía.
    /// Actualiza la UI de energía después de la adición.
    /// </summary>
    /// <param name="amount">La cantidad de energía a agregar.</param>
}
