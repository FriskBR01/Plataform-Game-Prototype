using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // NOVO: Variável para o nome da cena da primeira fase. 
    // Coloque o nome exato da sua primeira fase (ex: "Level_01", "SampleScene")
    public string firstLevelSceneName = "Level_01";

    // Esta função será chamada pelo botão "Começar Jogo" na UI
    public void StartGame()
    {
        // Se a variável estiver preenchida no Inspector, carrega a cena.
        if (!string.IsNullOrEmpty(firstLevelSceneName))
        {
            // O Build Index 1 será a primeira fase se a MainMenuScene for 0.
            // Para maior segurança, usamos o nome da cena.
            SceneManager.LoadScene(firstLevelSceneName);
        }
        else
        {
            Debug.LogError("O nome da cena da primeira fase não foi definido no MenuManager!");
        }
    }

    // Função opcional para o botão "Sair"
    public void QuitGame()
    {
        Application.Quit();
        // A linha abaixo só aparece no Editor, não em builds
        Debug.Log("Saindo do Jogo...");
    }
}   