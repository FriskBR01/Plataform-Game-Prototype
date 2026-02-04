using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variáveis públicas / serializadas
    public float moveSpeed = 5f;
    public float jumpForce = 11f;

    // Variáveis privadas
    private Rigidbody2D rb;
    public bool isGrounded = false;
    private bool isFacingRight = true;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update é para ler Inputs
    void Update()
    {
        // 1. Captura o input
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Checa o pulo
        Jump();

        // 3. Lógica de virar o sprite
        if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();
    }

    // FixedUpdate é para aplicar a Física (Correção de tremor de câmera)
    void FixedUpdate()
    {
        MovePhysics();
    }

    void MovePhysics()
    {
        // Aplica a velocidade usando rb.velocity
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Aplica a força de pulo
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // NOVO: Lógica de Pulo na Cabeça (Stomp)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com o inimigo através do Collider Trigger nos pés
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                // Destrói o inimigo
                enemyScript.Die();

                // Adiciona um pequeno pulo extra (bounce) para feedback
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.75f);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}