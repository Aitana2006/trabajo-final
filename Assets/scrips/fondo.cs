using UnityEngine;

public class fondo: MonoBehaviour
{
    [SerializeField] private Vector2 velocidadDeMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
       // jugadorRB = GameObject.FindGameObjectsWithTag().GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        offset = (jugadorRB.linearVelocity.x * 0.1f) * velocidadDeMovimiento * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
