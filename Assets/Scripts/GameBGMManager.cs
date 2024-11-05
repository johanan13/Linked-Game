using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static GameBGMManager instance;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartBGM();
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
}
