using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;
    private Transform target;
    public int maxHealth = 2;
    private int health;

    // NOVO (Opcional, mas útil): Variável para o Animator se você tiver animações
    private Animator anim;

    void Start()
    {
        // Define o alvo inicial
        target = pointB != null ? pointB : pointA;
        health = maxHealth;
        // Pega o componente Animator
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Patrol();
        // Você pode adicionar chamadas de animação aqui se tiver um Animator
        // UpdateAnimation(); 
    }

    void Patrol()
    {
        // Garante que os pontos de patrulha existem
        if (target == null || pointA == null || pointB == null) return;

        // Move o inimigo em direção ao alvo
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Se chegou perto o suficiente do alvo, troca de ponto e vira
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            // Troca o alvo
            target = target == pointA ? pointB : pointA;

            // Inverte o sprite para encarar o novo alvo
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        // Inverte a escala X
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Método de dano (não usado pelo pulo na cabeça, mas útil para outros ataques)
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    // A função Die() é chamada pelo PlayerController (pulo) ou por TakeDamage
    public void Die() // Mantenha como PUBLIC para ser acessível
    {
        // 1. NOTIFICA O GAMEMANAGER (Pontuação)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyKilled();
        }

        // 2. Destrói o Game Object
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // O corpo do inimigo causa dano ao Player (se não for o ataque de pulo)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Tenta pegar o script de vida do jogador
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                // Verifica a direção da colisão para evitar dano no stomp.
                // Se o player está acima do inimigo, o dano pode ser ignorado aqui,
                // pois o PlayerController cuida do "stomp".

                // Assumindo que você quer dano apenas em colisões laterais:
                // Se a colisão ocorrer abaixo do jogador, o PlayerController cuidou da morte.
                // Se a colisão é lateral (e o inimigo está vivo), cause dano.

                // Para simplificar, mantemos a lógica original de causar dano por toque:
                ph.TakeDamage(1);
            }
        }
    }
}