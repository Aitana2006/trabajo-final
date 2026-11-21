using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class MenuPausa : MonoBehaviour
{
    [Header("Paneles de la UI (Opcional)")]
    [Tooltip("Panel principal del menu de pausa (contiene Continuar, Opciones, Salir). Si no se asigna, se mostrara una ventana visual automatica.")]
    [SerializeField] private GameObject menuPausa;

    [Tooltip("Panel del menu de opciones (pantalla completa, volumen)")]
    [SerializeField] private GameObject menuOpciones;

    public static bool juegoPausado = false;

    private void Start()
    {
        // Al iniciar la partida, aseguramos que el juego no comience pausado
        Continuar();
    }

    private void Update()
    {
        bool presionoPausa = false;

        // Compatible con New Input System (tecla P o Escape)
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                presionoPausa = true;
            }
        }
        #endif

        // Compatible con Legacy Input Manager (tecla P o Escape)
        try
        {
            if (!presionoPausa && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
            {
                presionoPausa = true;
            }
        }
        catch
        {
            // Proteccion por si solo esta activo el New Input System
        }

        if (presionoPausa)
        {
            if (juegoPausado)
            {
                // Si esta viendo las opciones, volver al menu de pausa
                if (menuOpciones != null && menuOpciones.activeSelf)
                {
                    CerrarOpciones();
                }
                else
                {
                    Continuar();
                }
            }
            else
            {
                Pausar();
            }
        }
    }

    // Boton CONTINUAR / REANUDAR
    public void Continuar()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);

        if (menuOpciones != null)
            menuOpciones.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;
    }

    // Activa la pausa
    public void Pausar()
    {
        if (menuPausa != null)
            menuPausa.SetActive(true);

        if (menuOpciones != null)
            menuOpciones.SetActive(false);

        Time.timeScale = 0f;
        juegoPausado = true;
    }

    // Boton OPCIONES
    public void AbrirOpciones()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);

        if (menuOpciones != null)
            menuOpciones.SetActive(true);
    }

    // Boton VOLVER (desde Opciones al Menu de Pausa)
    public void CerrarOpciones()
    {
        if (menuOpciones != null)
            menuOpciones.SetActive(false);

        if (menuPausa != null)
            menuPausa.SetActive(true);
    }

    // Boton SALIR (regresa al Menu Principal en BuildIndex 0)
    public void Salir()
    {
        Time.timeScale = 1f;
        juegoPausado = false;

        if (trancicionScenasui.Instance != null)
        {
            trancicionScenasui.Instance.BloqueSalida(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    // Boton REINICIAR (reinicia el nivel actual)
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Boton SALIR DEL JUEGO
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando aplicacion...");
        Application.Quit();
    }

    // Ventana visual interactiva automatica si no se asigno un panel de Canvas
    private void OnGUI()
    {
        if (juegoPausado && menuPausa == null)
        {
            GUI.skin.box.fontSize = 20;
            GUI.skin.button.fontSize = 15;

            int ancho = 320;
            int alto = 260;
            int posX = (Screen.width - ancho) / 2;
            int posY = (Screen.height - alto) / 2;

            GUI.Box(new Rect(posX, posY, ancho, alto), "\n⏸ JUEGO PAUSADO (P)");

            if (GUI.Button(new Rect(posX + 40, posY + 70, ancho - 80, 40), "Continuar"))
            {
                Continuar();
            }

            if (GUI.Button(new Rect(posX + 40, posY + 125, ancho - 80, 40), "Reiniciar Nivel"))
            {
                Reiniciar();
            }

            if (GUI.Button(new Rect(posX + 40, posY + 180, ancho - 80, 40), "Menu Principal"))
            {
                Salir();
            }
        }
    }
}
