using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start() {
        BGMusicManager.instance.StartBGM();
        PlayerData.instance?.playerName.Clear();
        PlayerData.instance?.playerSpriteIndex.Clear();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame()
    {
         #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
            }
    }

