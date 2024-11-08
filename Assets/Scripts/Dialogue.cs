using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialoguePanelWitch;  // Panel del diálogo
    public float textSpeed;

    public string[] lines;
    private int index;
    private bool isPlayerNearby = false;

    private void Start()
    {
        textComponent.text = string.Empty;
       dialoguePanelWitch.SetActive(false);  // Asegúrate de que el panel esté oculto al inicio
    }

    private void Update()
    {
        // Solo permitir la interacción si el jugador está cerca
        if (isPlayerNearby && Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        dialoguePanelWitch.SetActive(true);  // Mostrar el panel
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void EndDialogue()
    {
    dialoguePanelWitch.SetActive(false);  // Ocultar el panel al finalizar
        textComponent.text = string.Empty;
    }

    // Método público para iniciar el diálogo con diferentes líneas
    public void TriggerDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        StartDialogue();
    }

    // Detectar cuando el jugador entra en la zona de interacción
   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Jugador cerca, iniciando diálogo...");
        isPlayerNearby = true;
        TriggerDialogue(lines);  // Iniciar el diálogo inmediatamente
    }
}

    // Detectar cuando el jugador sale de la zona de interacción
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                        Debug.Log("salio");

            isPlayerNearby = false;  // El jugador ya no está cerca
            EndDialogue();  // Ocultar el diálogo si se aleja
        }
    }
}
