using UnityEngine;
using UnityEngine.Events;

namespace HeneGames.DialogueSystem
{
    public class DialogueOption : MonoBehaviour
    {
        // Estos eventos aparecerán en el Inspector
        [Header("Events when player makes a choice")]
        public UnityEvent onAccept;
        public UnityEvent onDecline;

        // Llama a este método para mostrar la opción al jugador cuando termine el diálogo
        public void ShowOption()
        {
            // Aquí puedes implementar la UI que muestra la opción al jugador
            DialogueUI.instance.ShowOptionUI("¿Quieres llevar al NPC a la base?", OnAccept, OnDecline);
        }

        private void OnAccept()
        {
            // Ejecuta cualquier lógica adicional al aceptar, si es necesario
            onAccept.Invoke();

            // Opcional: desactivar el NPC o hacerlo desaparecer
            gameObject.SetActive(false);
        }

        private void OnDecline()
        {
            // Ejecuta cualquier lógica adicional al rechazar, si es necesario
            onDecline.Invoke();
        }
    }
}
