using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialoguePanel;  
    public TextMeshProUGUI dialogueText; 
    public string[] lines; 
    public float textSpeed = 0.05f;  
    private int index; 
    public TextMeshProUGUI npcNameText;     
    private bool isPlayerNearby = false;  

    public NPCtype nPCtype;  

    public Button takeToBaseButton;  
    public Button cancelButton;  

    void Start()
    {
        dialoguePanel.SetActive(false);  
        dialogueText.text = string.Empty;  
        takeToBaseButton.gameObject.SetActive(false);  
        cancelButton.gameObject.SetActive(false);  
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeSelf)
            {
                StartDialogue();  
            }
            else if (dialogueText.text == lines[index])
            {
                NextLine();  
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;  
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;  
            dialoguePanel.SetActive(false); 
            dialogueText.text = string.Empty;  
            takeToBaseButton.gameObject.SetActive(false); 
            cancelButton.gameObject.SetActive(false); 
        }
    }
    void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);  
        npcNameText.text = nPCtype.ToString(); 
        StartCoroutine(TypeLine());  
    }

    IEnumerator TypeLine()
    {
        dialogueText.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            takeToBaseButton.gameObject.SetActive(true);  
            cancelButton.gameObject.SetActive(true); 
        }
    }

  public void OnTakeToBaseButtonClick()
{
    // Guardar el tipo del NPC rescatado
    PlayerPrefs.SetString("rescuedNPC", nPCtype.ToString());
    PlayerPrefs.Save(); // Asegúrate de guardar los cambios

    SceneManager.LoadScene("BaseScene");  // Cargar la escena de la base
}
    public void OnCancelButtonClick()
    {
        dialoguePanel.SetActive(false);  
        takeToBaseButton.gameObject.SetActive(false); 
        cancelButton.gameObject.SetActive(false);  
    }
}
