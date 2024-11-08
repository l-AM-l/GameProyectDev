using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCHapiness : MonoBehaviour
{
    private NPC npc;
    private float happiness; 
    private float maxHappiness = 100f; 
    private float happinessIncreaseRate = 10f; 
    private float happinessOverTimeRate = 5f; 
    private float timeInterval = 10f; 

    public Slider happinessSlider; 
    public TextMeshProUGUI happinessText; 

    private void Start()
    {
        // Iniciar todos los NPCs con un 50% de felicidad
        happiness = 50f;

        happinessSlider.maxValue = maxHappiness;
        happinessSlider.value = happiness;

        // Actualizar el texto del porcentaje
        UpdateHappinessUI();

        // Comienza a incrementar la felicidad con el tiempo
        StartCoroutine(IncreaseHappinessOverTime());
    }

    public void AssignJob()
    {
        // Incrementa la felicidad en 10% al asignar trabajo
        happiness = Mathf.Min(happiness + happinessIncreaseRate, maxHappiness); // No pasa del 100%

        // Actualizar la barra del slider y el texto
        UpdateHappinessUI();

        Debug.Log("Felicidad del NPC aumentada al asignar trabajo: " + happiness);
    }

    private IEnumerator IncreaseHappinessOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeInterval);

            if (!string.IsNullOrEmpty(npc.job))
            {
                happiness = Mathf.Min(happiness + happinessOverTimeRate, maxHappiness); // No pasa del 100%
                UpdateHappinessUI(); // Actualizar la barra del slider y el texto
                Debug.Log("Felicidad del NPC aumentada con el tiempo: " + happiness);
            }
        }
    }

    private void UpdateHappinessUI()
    {
        // Actualizar el valor del slider
        happinessSlider.value = happiness;

        // Actualizar el texto del porcentaje
        happinessText.text = Mathf.RoundToInt(happiness).ToString() + "%";
    }

    // Método para obtener el nivel actual de felicidad del NPC
    public float GetHappiness()
    {
        return happiness;
    }

    public void DecreaseHappiness(float amount)
    {
        happiness = Mathf.Max(happiness - amount, 0f); // No cae por debajo de 0
        UpdateHappinessUI();
        Debug.Log("Felicidad del NPC reducida: " + happiness);
    }
}
