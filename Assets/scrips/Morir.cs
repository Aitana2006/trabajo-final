using UnityEngine;
using UnityEngine.SceneManagement;

// Adjuntar al jugador (carbon)
// Muere al tocar cualquier objeto con tag "Enemigo"
public class Morir : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemigo"))
            Caer();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemigo"))
            Caer();
    }

    private void Caer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
