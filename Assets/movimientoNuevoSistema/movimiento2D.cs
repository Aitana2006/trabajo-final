using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Controles controles;
    public Vector2 direccion;
    public Rigidbody2D rb2D;
    public float velocidadMovimiento;
    public bool mirandoDerecha = true;
    public float fuerzaSalto;
    public LayerMask queEsSuelo;
    public Transform controladorSuelo;
    public Vector3 dimensionesCaja;
    public bool enSuelo;
    private Animator animator;

    private void Awake()
    {
        controles = new();
        if (rb2D == null)
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controles.Enable();
        controles.movimiento.saltar.started += _ => Saltar();
    }

    private void OnDisable()
    {
        controles.Disable();
        controles.movimiento.saltar.started -= _ => Saltar();
    }

    private void Update()
    {
        if (controles != null)
        {
            direccion = controles.movimiento.mover.ReadValue<Vector2>();
        }
        AjustarRotacion(direccion.x);

        if (controladorSuelo != null)
        {
            enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        }

        if (animator != null)
        {
            animator.SetFloat("horizontal", Mathf.Abs(direccion.x));
            animator.SetBool("estaEnSuelo", enSuelo);
        }
    }

    private void FixedUpdate()
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, rb2D.linearVelocity.y);

            // Freno de caida: gravedad extra cuando baja
            if (rb2D.linearVelocity.y < 0)
            {
                rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.fixedDeltaTime;
            }
        }
    }

    private void AjustarRotacion(float direccionX) 
    {
        if(direccionX > 0 && !mirandoDerecha) 
        {
            Girar();
        }
        else if (direccionX < 0 && mirandoDerecha) 
        {
            Girar();
        }
    }

    private void Girar() 
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void Saltar() 
    {
        if (enSuelo)
        {
            rb2D.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
        }
    }  
}
