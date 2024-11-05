using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameTimer gameTimer; // Reference to the GameTimer script


    public void ResumeGame()
    {
        gameTimer.TogglePause(); // Resume the game by calling TogglePause
      
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time scale is normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
