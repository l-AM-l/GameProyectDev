using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// NPCHapiness gestiona la felicidad de un NPC, incluidas las actualizaciones de su interfaz de usuario y
/// la mecánica de la felicidad aumenta con el tiempo o en función de eventos como la asignación de trabajo.
/// </summary>
public class NPCHapiness : MonoBehaviour
{
    // Referencias
    private NPC npc; // Referencia al componente NPC asociado

    // Propiedades de felicidad
    public float happiness = 10f; // Valor actual de felicidad
    private float maxHappiness = 100f; // Valor máximo de felicidad
    private float happinessIncreaseRate = 10f; // Incremento de felicidad al asignar un trabajo
    private float happinessOverTimeRate = 5f; // Incremento de felicidad con el tiempo
    private float timeInterval = 5f; // Intervalo de tiempo para el aumento periódico de felicidad

    // Elementos de UI
    public Slider happinessSlider; // Control deslizante para mostrar la felicidad actual
    public TextMeshProUGUI happinessText; // Texto para mostrar la felicidad como un porcentaje

    // Coroutine para manejar el aumento de felicidad con el tiempo
    private Coroutine happinessCoroutine;

    /// <summary>
    /// Recupera el valor actual de felicidad.
    /// </summary>
    /// <returns>El valor actual de felicidad como un float.</returns>
    public float GetCurrentHappiness()
    {
        return happiness;
    }

    /// <summary>
    /// Establece el valor de felicidad, limitándolo entre 0 y maxHappiness.
    /// Actualiza la UI de felicidad en consecuencia.
    /// </summary>
    /// <param name="value">El nuevo valor de felicidad a establecer.</param>
    public void SetHappiness(float value)
    {
        happiness = Mathf.Clamp(value, 0, maxHappiness); // Asegura que la felicidad se mantenga dentro de los límites
        UpdateHappinessUI();
    }

    /// <summary>
    /// Inicializa el sistema de felicidad y asegura que los elementos de UI estén configurados correctamente.
    /// </summary>
    public int npcID;

    private void Start()
    {
        npc = GetComponent<NPC>(); // Obtener el componente NPC

        // Validar referencias de UI
        if (happinessSlider == null)
        {
            Debug.Log("Happiness slider is not assigned");
        }
        if (happinessText == null)
        {
            Debug.Log("Happiness text is not assigned");
        }

        // Establecer propiedades del control deslizante
        happinessSlider.maxValue = maxHappiness;

        // Cargar datos de felicidad guardados
        LoadHappiness();

        // Actualizar UI
        UpdateHappinessUI();
    }
    
    /// <summary>
    /// Guarda el porcentaje de felicidad de este NPC en PlayerPrefs.
    /// </summary>
    public void SaveHappiness()
    {
        PlayerPrefs.SetFloat($"NPC_{npcID}_Happiness", happiness);
        PlayerPrefs.Save();
        Debug.Log($"NPC {npcID} happiness saved: {happiness}%");
    }

    /// <summary>
    /// Carga el porcentaje de felicidad de este NPC desde PlayerPrefs.
    /// Por defecto, se establece en 10% si no existe un valor guardado.
    /// </summary>
    public void LoadHappiness()
    {
        happiness = PlayerPrefs.GetFloat($"NPC_{npcID}_Happiness", 10f); // Por defecto a 10%
        Debug.Log($"NPC {npcID} happiness loaded: {happiness}%");
    }

    /// <summary>
    /// Borra los datos de felicidad guardados para este NPC.
    /// </summary>
    public void ClearHappinessData()
    {
        PlayerPrefs.DeleteKey($"NPC_{npcID}_Happiness");
        PlayerPrefs.Save();
        Debug.Log($"NPC {npcID} happiness data cleared.");
    }

    private void OnApplicationQuit()
    {
        SaveHappiness(); // Guardar felicidad al salir del juego
    }
    
    /// <summary>
    /// Aumenta la felicidad por una cantidad fija cuando se asigna un trabajo al NPC.
    /// </summary>
    public void AssignJob()
    {
        happiness = Mathf.Min(happiness + happinessIncreaseRate, maxHappiness); // Limitar la felicidad al máximo
        UpdateHappinessUI(); // Actualizar la UI
        Debug.Log("NPC happiness increased after assigning a job: " + happiness);
    }

    /// <summary>
    /// Aumenta directamente la felicidad por una cantidad especificada.
    /// </summary>
    /// <param name="amount">La cantidad de felicidad a añadir.</param>
    public void IncreaseHappiness(float amount)
    {
        happiness = Mathf.Min(happiness + amount, maxHappiness); // Limitar la felicidad al máximo
        UpdateHappinessUI(); // Actualizar la UI
    }

    /// <summary>
    /// Inicia una coroutine para aumentar la felicidad periódicamente con el tiempo.
    /// </summary>
    public void StartIncreasingHappiness()
    {
        if (happinessCoroutine == null)
        {
            Debug.Log("Happiness increase started");
            happinessCoroutine = StartCoroutine(IncreaseHappinessOverTime());
        }
    }

    /// <summary>
    /// Detiene la coroutine que aumenta la felicidad con el tiempo.
    /// </summary>
    public void StopIncreasingHappiness()
    {
        if (happinessCoroutine != null)
        {
            Debug.Log("Stopping happiness increase");
            StopCoroutine(happinessCoroutine);
            happinessCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine que aumenta periódicamente la felicidad mientras el NPC está empleado.
    /// Se detiene si el NPC queda desempleado o alcanza la felicidad máxima.
    /// </summary>
    private IEnumerator IncreaseHappinessOverTime()
    {
        while (happiness < maxHappiness)
        {
            // Verificar si el NPC está desempleado
            if (npc.job == "Unemployed")
            {
                Debug.Log("NPC became unemployed, stopping happiness coroutine");
                StopIncreasingHappiness();
                yield break; // Salir de la coroutine
            }

            // Esperar el intervalo especificado antes de aumentar la felicidad
            yield return new WaitForSeconds(timeInterval);

            // Aumentar la felicidad y registrar el valor actualizado
            IncreaseHappiness(happinessOverTimeRate);
            Debug.Log("Happiness increased to: " + happiness);
        }
    }

    /// <summary>
    /// Actualiza los elementos de UI (control deslizante y texto) para reflejar la felicidad actual.
    /// </summary>
    private void UpdateHappinessUI()
    {
        happinessSlider.value = happiness; // Actualizar el valor del control deslizante
        happinessText.text = Mathf.RoundToInt(happiness).ToString() + "%"; // Actualizar el texto
        Debug.Log("Happiness UI updated: " + happiness + "%");
        SaveHappiness();
    }
}
