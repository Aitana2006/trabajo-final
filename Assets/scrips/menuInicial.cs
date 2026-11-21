using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicial : MonoBehaviour
{
    private void Awake()
    {
        Transform opciones = transform.Find("menuOpciones");
        if (opciones != null)
        {
            opciones.gameObject.SetActive(false);
        }

        Transform inicial = transform.Find("menuInicial");
        if (inicial != null)
        {
            inicial.gameObject.SetActive(true);
        }
    }

    public void Jugar()
    {
        trancicionScenasui.Instance.BloqueSalida(SceneManager.GetActiveScene().buildIndex + 1);
    }
        public void Salir()
    {
        Debug.Log("salir...");
        Application.Quit();
    }
}