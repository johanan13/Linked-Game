using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardMenu : MonoBehaviour
{
    public void ExitToMainMenu()
    { 
        Time.timeScale = 1; // Ensure time scale is normal
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
