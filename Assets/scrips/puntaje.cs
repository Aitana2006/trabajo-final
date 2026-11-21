using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class puntaje : MonoBehaviour
{
    public static puntaje Instance;

    [Header("Puntaje")]
    [SerializeField] private int puntosActuales = 0;

    [Header("UI (Opcional - asigna Text o TextMeshPro)")]
    [SerializeField] private Text textoPuntos;
    [SerializeField] private TextMeshProUGUI textoPuntosTMP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ActualizarTexto();
    }

    // Funcion para sumar puntos al recoger estrellas
    public void SumarPuntos(int puntos)
    {
        puntosActuales += puntos;
        ActualizarTexto();
        Debug.Log("Puntos actuales: " + puntosActuales);
    }

    public void AgregarPuntos(int puntos)
    {
        SumarPuntos(puntos);
    }

    public int ObtenerPuntos()
    {
        return puntosActuales;
    }

    private void ActualizarTexto()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + puntosActuales;
        }

        if (textoPuntosTMP != null)
        {
            textoPuntosTMP.text = "Puntos: " + puntosActuales;
        }
    }

    // Pasa al nivel 2 usando la transicion de pantalla
    public void PasarAlNivel2()
    {
        Debug.Log("¡Todas las estrellas recolectadas! Pasando al nivel 2...");

        if (trancicionScenasui.Instance != null)
        {
            // Carga level2 (Build Index 2) con la animacion de transicion
            trancicionScenasui.Instance.BloqueSalida(2);
        }
        else
        {
            SceneManager.LoadScene("level2");
        }
    }
}
