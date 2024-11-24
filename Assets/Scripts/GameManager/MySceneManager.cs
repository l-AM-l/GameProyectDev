using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    // Instancia única de la clase
    public static MySceneManager instance;
    public Animator transitionAnim;


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

    // Método para cargar la siguiente escena
    public void NextScene()
    {
        StartCoroutine(LoadLevel());
    }

    // Detectar colisiones con el jugador
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            NextScene();
        }
    }
  public IEnumerator LoadLevel()
{
    // Guarda el estado de energía
    
    transitionAnim.SetTrigger("End");
    yield return new WaitForSeconds(1);
    
    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    transitionAnim.SetTrigger("Start");
}

}
