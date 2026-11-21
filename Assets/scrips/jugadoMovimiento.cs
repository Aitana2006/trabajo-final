using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class jugadoMovimiento : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento = 400f;
    [SerializeField] private float suavizadoDeMovimiento = 0.05f;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto = 12f;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionescaja = new Vector3(1f, 0.4f, 0f);
    [SerializeField] private bool enSuelo;
    private float tiempoBufferSalto = 0f;
    [SerializeField] private float duracionBufferSalto = 0.15f;
    private float velocidadXRef = 0f;

    [Header("Animaciones con Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] spritesIdle;
    [SerializeField] private Sprite[] spritesCaminando;
    [SerializeField] private Sprite[] spritesSalto;
    [SerializeField] private float fpsCaminar = 8f;
    [SerializeField] private float fpsIdle = 4f;
    [SerializeField] private float fpsSalto = 10f;
    [Header("Animacion")]
    private Animator animator; 

    private float timerAnimacion = 0f;
    private int frameAnimacion = 0;
    private EstadoAnimacion estadoAnim = EstadoAnimacion.Idle;

    private enum EstadoAnimacion
    {
        Idle,
        Caminando,
        Saltando
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (rb2D != null)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        float horizontal = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        }
        if (Gamepad.current != null)
        {
            float stick = Gamepad.current.leftStick.x.ReadValue();
            if (Mathf.Abs(stick) > 0.1f) horizontal = stick;
        }
        movimientoHorizontal = horizontal * velocidadDeMovimiento;

        if ((Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            tiempoBufferSalto = duracionBufferSalto;
        }


                movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            tiempoBufferSalto = duracionBufferSalto;
        }

        GestionarAnimaciones();
    }

    private void FixedUpdate()
    {
        // Contador del buffer de salto
        if (tiempoBufferSalto > 0f)
        {
            tiempoBufferSalto -= Time.fixedDeltaTime;
        }

        // Detección de suelo
        Vector2 puntoSuelo = (controladorSuelo != null)
            ? (Vector2)controladorSuelo.position
            : new Vector2(transform.position.x, GetComponent<Collider2D>() != null ? GetComponent<Collider2D>().bounds.min.y : transform.position.y - 0.5f);

        Collider2D[] colisiones = (queEsSuelo.value != 0)
            ? Physics2D.OverlapBoxAll(puntoSuelo, dimensionescaja, 0f, queEsSuelo)
            : Physics2D.OverlapBoxAll(puntoSuelo, dimensionescaja, 0f);

        enSuelo = false;
        foreach (var col in colisiones)
        {
            if (col.gameObject != gameObject && !col.transform.IsChildOf(transform) && !col.isTrigger)
            {
                enSuelo = true;
                break;
            }
        }

        // Si la velocidad es pequeña (< 50), se escala para que responda con buen dinamismo
        float factorVelocidad = (velocidadDeMovimiento < 50f) ? 15f : 1f;
        Mover(movimientoHorizontal * Time.fixedDeltaTime * factorVelocidad, tiempoBufferSalto > 0f);
    }

    private void Mover(float mover, bool saltar)
    {
        // Solo suavizamos el eje horizontal X para no alterar la física del salto o la gravedad en Y
        float velocidadX = Mathf.SmoothDamp(rb2D.linearVelocity.x, mover, ref velocidadXRef, suavizadoDeMovimiento);
        rb2D.linearVelocity = new Vector2(velocidadX, rb2D.linearVelocity.y);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && saltar)
        {
            enSuelo = false;
            tiempoBufferSalto = 0f;
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, fuerzaDeSalto);
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void GestionarAnimaciones()
    {
        if (spriteRenderer == null) return;

        EstadoAnimacion nuevoEstado;

        if (!enSuelo)
        {
            nuevoEstado = EstadoAnimacion.Saltando;
        }
        else if (Mathf.Abs(movimientoHorizontal) > 0.05f)
        {
            nuevoEstado = EstadoAnimacion.Caminando;
        }
        else
        {
            nuevoEstado = EstadoAnimacion.Idle;
        }

        if (nuevoEstado != estadoAnim)
        {
            estadoAnim = nuevoEstado;
            frameAnimacion = 0;
            timerAnimacion = 0f;
        }

        Sprite[] listaSprites;
        float fps;

        switch (estadoAnim)
        {
            case EstadoAnimacion.Caminando:
                listaSprites = spritesCaminando;
                fps = fpsCaminar;
                break;
            case EstadoAnimacion.Saltando:
                listaSprites = spritesSalto;
                fps = fpsSalto;
                break;
            case EstadoAnimacion.Idle:
            default:
                listaSprites = spritesIdle;
                fps = fpsIdle;
                break;
        }

        if (listaSprites != null && listaSprites.Length > 0)
        {
            timerAnimacion += Time.deltaTime;
            float duracionFrame = 1f / Mathf.Max(fps, 0.01f);

            if (timerAnimacion >= duracionFrame)
            {
                timerAnimacion -= duracionFrame;

                if (estadoAnim == EstadoAnimacion.Saltando)
                {
                    // Al saltar, avanza hasta el último frame mientras esté en el aire
                    if (frameAnimacion < listaSprites.Length - 1)
                    {
                        frameAnimacion++;
                    }
                }
                else
                {
                    frameAnimacion = (frameAnimacion + 1) % listaSprites.Length;
                }
            }

            frameAnimacion = Mathf.Clamp(frameAnimacion, 0, listaSprites.Length - 1);
            if (listaSprites[frameAnimacion] != null)
            {
                spriteRenderer.sprite = listaSprites[frameAnimacion];
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 punto = (controladorSuelo != null) ? controladorSuelo.position : transform.position;
        Gizmos.DrawWireCube(punto, dimensionescaja);
    }
}
