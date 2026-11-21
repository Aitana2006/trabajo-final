using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    public static Victoria Instance;

    [Header("UI de Victoria (Opcional)")]
    [Tooltip("Panel de victoria en el Canvas. Si no se asigna, se mostrara una ventana interactiva automaticamente.")]
    [SerializeField] private GameObject panelVictoria;

    public bool juegoGanado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        juegoGanado = false;
        Time.timeScale = 1f;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !juegoGanado)
        {
            Ganar();
        }
    }

    public void Ganar()
    {
        juegoGanado = true;
        Time.timeScale = 0f;

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        Debug.Log("¡VICTORIA! Nivel 2 completado.");
    }

    public void JugarDeNuevo()
    {
        Time.timeScale = 1f;
        juegoGanado = false;
        SceneManager.LoadScene("level1");
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        juegoGanado = false;

        if (trancicionScenasui.Instance != null)
        {
            trancicionScenasui.Instance.BloqueSalida(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    // Ventana visual automatica para que funcione sin configuracion adicional
    private void OnGUI()
    {
        if (juegoGanado && panelVictoria == null)
        {
            GUI.skin.box.fontSize = 20;
            GUI.skin.button.fontSize = 16;

            int ancho = 380;
            int alto = 250;
            int posX = (Screen.width - ancho) / 2;
            int posY = (Screen.height - alto) / 2;

            GUI.Box(new Rect(posX, posY, ancho, alto), "\n★ ¡VICTORIA! ★\n¡Completaste el juego con exito!");

            if (GUI.Button(new Rect(posX + 40, posY + 100, ancho - 80, 45), "Jugar de Nuevo (Nivel 1)"))
            {
                JugarDeNuevo();
            }

            if (GUI.Button(new Rect(posX + 40, posY + 160, ancho - 80, 45), "Menu Principal"))
            {
                SalirAlMenu();
            }
        }
    }
}
