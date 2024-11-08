using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tabScript : MonoBehaviour
{
    // Referencia a los tabs y el panel de UI
    public GameObject[] tabs; 
    public TextMeshProUGUI titleText; 
    public GameObject UIPanel;
    
    private bool isPanelVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPanelVisible = !isPanelVisible;

            Debug.Log("UI activada/desactivada");

            UIPanel.SetActive(isPanelVisible);

            if (isPanelVisible)
            {
                turnOnTabs(1); 
            }
        }
    }

    public void turnOnTabs(int tab)
    {
        // Desactivar todos los tabs
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }

        // Activar el tab especificado
        tabs[tab - 1].SetActive(true);
    }

    public void changeTitleText(string text){
        titleText.text = text;

    }
}
