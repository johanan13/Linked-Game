using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<string> playerName = new List<string>();
    public List<int> playerSpriteIndex = new List<int>();

    public static PlayerData instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}