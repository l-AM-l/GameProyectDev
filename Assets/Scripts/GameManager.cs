using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int maxEnergy = 100;  // Valor máximo de energía
    public Slider energySlider; 
    public TextMeshProUGUI energyText;

    private void Start()
    {
        energySlider.maxValue = maxEnergy;  // Establecer el valor máximo del slider
        UpdateEnergyUI(); 
    }

    public void UpdateEnergyUI()
    {
        energySlider.value = maxEnergy;

        if (energyText != null)
        {
            energyText.text = "Energía: " + maxEnergy;
        }
    }

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

    public bool HasEnoughEnergy(int requiredEnergy)
    {
        return maxEnergy >= requiredEnergy;
    }

    public void UseEnergy(int amount)
    {if(maxEnergy<=0){
            maxEnergy=0;
        }
        else{
             maxEnergy -= amount;
        }
       
        
        

        Debug.Log("Energía restante: " + maxEnergy);
        UpdateEnergyUI();
    }
        
    public void AddEnergy(int amount)
{
        if(maxEnergy>=100){
            maxEnergy=100;
        }
        else{
             maxEnergy += amount;
        }
        Debug.Log("Energía actual: " + maxEnergy);
       
    UpdateEnergyUI();
}
}
