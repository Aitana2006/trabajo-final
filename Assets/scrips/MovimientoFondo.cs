using UnityEngine;

public class MovimientoFondo : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB;

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            material = sr.material;
        }

        // Buscar jugador por tag
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugadorRB = jugadorObj.GetComponent<Rigidbody2D>();
        }

        // Fallback: buscar cualquier Rigidbody2D que no sea el propio
        if (jugadorRB == null)
        {
            Rigidbody2D[] todos = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);
            foreach (var rb in todos)
            {
                if (rb.gameObject != gameObject)
                {
                    jugadorRB = rb;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (jugadorRB == null || material == null) return;

        offset = (jugadorRB.linearVelocity.x * 0.1f) * velocidadMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }

}
