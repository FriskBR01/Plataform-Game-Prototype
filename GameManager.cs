using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Não esqueça deste using se estiver usando TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // --- VARIÁVEIS DE UI DE FIM DE JOGO ---
    public GameObject winScreenUI;
    public TextMeshProUGUI finalScoreText; // Renomeado para maior clareza

    // --- NOVO: VARIÁVEL DE UI EM TEMPO REAL ---
    public TextMeshProUGUI realTimeScoreText;

    // --- VARIÁVEIS DE JOGO ---
    private int totalEnemies;
    private int enemiesKilled = 0;
    private float startTime;
    private bool gameFinished = false;

    // --- VARIÁVEIS DE CÁLCULO DE PONTUAÇÃO ---
    private int maxScore = 1000;
    private int penaltyPerSecond = 50;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Garante que não é destruído entre cenas, se você tiver mais de uma
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Encontra o número de inimigos na cena no início
        totalEnemies = FindObjectsOfType<Enemy>().Length;
        startTime = Time.time;
        Time.timeScale = 1f;
        gameFinished = false;

        // Limpa a tela de vitória ao iniciar
        if (winScreenUI != null) winScreenUI.SetActive(false);
    }

    // NOVO: Atualiza a HUD constantemente
    void Update()
    {
        if (gameFinished || realTimeScoreText == null) return;

        float timeTaken = Time.time - startTime;

        // Calcula a pontuação atual em tempo real
        int currentScore = maxScore - Mathf.FloorToInt(timeTaken * penaltyPerSecond);
        if (currentScore < 0) currentScore = 0;

        // Mostra o tempo decorrido e a pontuação atual
        realTimeScoreText.text = $"Tempo: {timeTaken:F1}s\nPontuação: {currentScore}";
    }

    public void EnemyKilled()
    {
        if (gameFinished) return;

        enemiesKilled++;

        if (enemiesKilled >= totalEnemies)
        {
            EndGame(true); // Vitória
        }
    }

    public void EndGame(bool victory)
    {
        gameFinished = true;
        Time.timeScale = 0f; // Pausa o jogo

        if (victory)
        {
            float timeTaken = Time.time - startTime;

            // Recalcula a pontuação final (apenas por segurança)
            int finalScore = maxScore - Mathf.FloorToInt(timeTaken * penaltyPerSecond);
            if (finalScore < 0) finalScore = 0;

            // 1. Exibir a Tela de Vitória
            if (winScreenUI != null) winScreenUI.SetActive(true);

            // 2. Mostrar a Pontuação Final
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Fase Concluída!\n\nTempo: {timeTaken:F2} segundos\n\nPontuação Final: {finalScore} pontos!";
            }
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Despausa o jogo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Destruir esta instância para que a nova cena crie uma limpa (IMPORTANTE)
        Destroy(gameObject);
    }
}