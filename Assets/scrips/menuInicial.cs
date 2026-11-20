using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicial : MonoBehaviour
{
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