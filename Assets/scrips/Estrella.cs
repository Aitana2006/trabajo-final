using UnityEngine;
using UnityEngine.SceneManagement;

// Adjuntar este script a las estrellas
// Al tocarlas, suma puntos en 'puntaje' y al recoger todas pasa a 'level2' con transicion
public class Estrella : MonoBehaviour
{
    [SerializeField] private int puntos = 10;
    private bool recogida = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (recogida) return;

        if (col.gameObject.CompareTag("Player"))
        {
            recogida = true;

            // 1. Sumar puntos usando el script puntaje
            if (puntaje.Instance != null)
            {
                puntaje.Instance.SumarPuntos(puntos);
            }
            if (controladorPuntos.Instance != null)
            {
                controladorPuntos.Instance.SumarPuntos(puntos);
            }
            if (GestorPuntos.Instance != null)
            {
                GestorPuntos.Instance.AgregarPuntos(puntos);
            }

            // Ocultar y desactivar la estrella de inmediato
            Collider2D c = GetComponent<Collider2D>();
            if (c != null) c.enabled = false;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            // Destruir el GameObject de la estrella
            Destroy(gameObject);

            // 2. Verificar cuantas estrellas quedan activas en el mapa
            Estrella[] todas = Object.FindObjectsByType<Estrella>(FindObjectsSortMode.None);
            int restantes = 0;
            foreach (Estrella est in todas)
            {
                if (est != this && est.gameObject.activeInHierarchy && !est.recogida)
                {
                    restantes++;
                }
            }

            // 3. Si era la ultima estrella, hacer la transicion hacia el nivel 2
            if (restantes == 0)
            {
                PasarAlNivel2();
            }
        }
    }

    private void PasarAlNivel2()
    {
        Debug.Log("¡Todas las estrellas recogidas! Ejecutando transicion hacia level2...");

        // Si el script puntaje tiene la logica o si llamamos a la transicion directamente
        if (trancicionScenasui.Instance != null)
        {
            // Carga level2 (BuildIndex 2 en Scenes) con la animacion del bloque
            trancicionScenasui.Instance.BloqueSalida(2);
        }
        else
        {
            SceneManager.LoadScene("level2");
        }
    }
}
