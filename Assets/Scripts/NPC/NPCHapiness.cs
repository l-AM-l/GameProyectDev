using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCHapiness : MonoBehaviour
{
    private NPC npc;
    private float happiness = 10f; 
    private float maxHappiness = 100f; 
    private float happinessIncreaseRate = 10f; 
    private float happinessOverTimeRate = 5f; 
    private float timeInterval = 5f; 

    public Slider happinessSlider; 
    public TextMeshProUGUI happinessText;

    private Coroutine happinessCoroutine;

    public float GetCurrentHappiness()
{
    return happiness;
}
public void SetHappiness(float value)
{
    happiness = Mathf.Clamp(value, 0, maxHappiness);
    UpdateHappinessUI();
}
    private void Start()
    {
        npc = GetComponent<NPC>();
        if(happinessSlider == null)
        {
            Debug.Log("Happiness slider is not assigned");
        }
        if(happinessText == null)
        {
            Debug.Log("Happiness text is not assigned");
        }
        happinessSlider.maxValue = maxHappiness;
        happinessSlider.value = happiness;
        UpdateHappinessUI();
    }

    public void AssignJob()
    {
        // Incrementa la felicidad en 10% al asignar trabajo
        happiness = Mathf.Min(happiness + happinessIncreaseRate, maxHappiness); // No pasa del 100%

        // Actualizar la barra del slider y el texto
        UpdateHappinessUI();

        Debug.Log("Felicidad del NPC aumentada al asignar trabajo: " + happiness);
    }

    public void IncreaseHappiness(float amount)
    {
        happiness = Mathf.Min(happiness + amount, maxHappiness);
        UpdateHappinessUI();
    }

    public void StartIncreasingHappiness()
    {
        if(happinessCoroutine == null)
        {
            Debug.Log("Hapiness increase started");
            happinessCoroutine = StartCoroutine(IncreaseHappinessOverTime());
        }
    }

    public void StopIncreasingHappiness()
    {
        if(happinessCoroutine != null)
        {
            Debug.Log("Stopping hapiness increase");
            StopCoroutine(happinessCoroutine);
            happinessCoroutine = null;
        }
    }

    private IEnumerator IncreaseHappinessOverTime()
    {
        while(happiness < maxHappiness)
        {
            if(npc.job == "Unemployed")
            {
                Debug.Log("NPC became unemployed, stopping happiness coroutine");
                StopIncreasingHappiness();
                yield break;
            }
            yield return new WaitForSeconds(timeInterval);
            IncreaseHappiness(happinessOverTimeRate);
            Debug.Log("Happiness increased to: " + happiness);
        }
    }

    private void UpdateHappinessUI()
    {
        // Actualizar el valor del slider
        happinessSlider.value = happiness;

        // Actualizar el texto del porcentaje
        happinessText.text = Mathf.RoundToInt(happiness).ToString() + "%";
        Debug.Log("Hapiness UI updated: " + happiness + "%");
    }
}
