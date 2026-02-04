using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // NOVO: Adicione isto para usar TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    // VARIÁVEL ALTERADA: De Slider para TextMeshProUGUI
    public TextMeshProUGUI healthDisplay;

    // Adicionado para integração com a tela de derrota (opcional)
    public GameObject defeatScreenUI;

    void Start()
    {
        currentHealth = maxHealth;
        // Chama o método para mostrar a vida inicial
        UpdateHealthDisplay();

        // Garante que o jogo despausa ao iniciar a cena
        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Atualiza o display após tomar dano
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // NOVO MÉTODO: Atualiza o texto na tela
    void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            // O texto será, por exemplo, "VIDA: 3"
            healthDisplay.text = "VIDA: " + currentHealth.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");

        // Congela o jogo
        Time.timeScale = 0f;

        // Mostra tela de Game Over/Derrota
        if (defeatScreenUI != null)
        {
            defeatScreenUI.SetActive(true);
        }

        // Desativa o player
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        // Congela a física
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Static;
    }
}