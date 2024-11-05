using UnityEngine;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    public GameObject fullHeart;
    public GameObject emptyHeart;
    public int maxLives = 6;
    public int currentLives = 6; // Added to track current lives

    private List<GameObject> hearts = new List<GameObject>();

    private void Start()
    {
        // Initialize the heart display with full hearts
        UpdateHearts(currentLives);
    }

    public void UpdateHearts(int lives)
    {
        currentLives = lives; // Update current lives

        // Clear previous heart displays
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // Populate hearts based on current lives
        for (int i = 0; i < maxLives; i++)
        {
            GameObject heartPrefab = i < currentLives ? fullHeart : emptyHeart;
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);
        }
    }
}
