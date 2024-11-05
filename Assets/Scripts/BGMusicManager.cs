using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BGMusicManager : MonoBehaviour
{
    public static BGMusicManager instance;
    private AudioSource audioSource;
    private string gameSceneName = "GameScene";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
    }

    public void StartBGM() {
        audioSource.Play();
    }
    public void PauseBGM() {
        audioSource.Pause();
    }
    public void UnPauseBGM() {
        audioSource.UnPause();
    }
    public void StopBGM() {
        audioSource.Stop();
    }

    private void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the game scene
        if (scene.name == gameSceneName)
        {
            audioSource.Pause();  // Pause the persistent background music
        }
        else
        {
            audioSource.UnPause(); // Resume the persistent background music
        }
    }
}
