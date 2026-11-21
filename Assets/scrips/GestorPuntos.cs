using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Maneja la puntuacion, temporizador y derrota/victoria
public class GestorPuntos : MonoBehaviour
{
    public static GestorPuntos Instance;

    [Header("Puntos")]
    public int puntosActuales = 0;
    public int puntosParaGanar = 100;

    [Header("Temporizador")]
    public float tiempoLimite = 60f;
    private float tiempoRestante;
    private bool juegoActivo = true;

    [Header("UI")]
    [SerializeField] private Text textoPuntos;
    [SerializeField] private Text textoTiempo;
    [SerializeField] private GameObject pantallaDerrota;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        if (pantallaDerrota != null)
            pantallaDerrota.SetActive(false);
        ActualizarUI();
    }

    private void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante < 0f) tiempoRestante = 0f;

        ActualizarUI();

        if (tiempoRestante <= 0f)
        {
            MostrarDerrota();
        }
    }

    public void AgregarPuntos(int cantidad)
    {
        if (!juegoActivo) return;
        puntosActuales += cantidad;
        ActualizarUI();

        // Si se alcanzan los puntos requeridos, avanzar
        if (puntosActuales >= puntosParaGanar)
        {
            PasarNivel();
        }
    }

    private void ActualizarUI()
    {
        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntosActuales;

        if (textoTiempo != null)
        {
            int seg = Mathf.CeilToInt(tiempoRestante);
            textoTiempo.text = "Tiempo: " + seg + "s";
        }
    }

    public void PasarNivel()
    {
        if (!juegoActivo) return;
        juegoActivo = false;

        int siguiente = SceneManager.GetActiveScene().buildIndex + 1;
        if (trancicionScenasui.Instance != null)
            trancicionScenasui.Instance.BloqueSalida(siguiente);
        else
            SceneManager.LoadScene(siguiente);
    }

    private void MostrarDerrota()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        if (pantallaDerrota != null)
            pantallaDerrota.SetActive(true);
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
