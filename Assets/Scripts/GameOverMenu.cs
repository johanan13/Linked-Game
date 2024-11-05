using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{


    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time scale is normal
        LoadPlayerData();
        SceneManager.LoadScene("GameScene"); // Reload the current scene
    }

    public void ExitToMainMenu()
    { 
        Time.timeScale = 1; // Ensure time scale is normal
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
    private void LoadPlayerData()
    {
        // Assuming PlayerData has a method to retrieve the current player name and sprite index
        string playerName = PlayerData.instance.playerName[0]; // Example for the first player
        int playerSpriteIndex = PlayerData.instance.playerSpriteIndex[0];

    }
}
