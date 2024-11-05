using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollider : MonoBehaviour
{
    private HeartManager heartManager;
    private PlayerMovement playerMovement; // Reference to PlayerMovement component
    private SpawnManager spawnManager; 
    private int playerIndex; // Index to differentiate players (0 for Player 1, 1 for Player 2)
    public float speedMultiplier = 2f; // Multiplier for speed boost
    public float speedBoostDuration = 5f; // Duration of speed boost
    public float enemySlowDuration = 5f; // Duration for enemies to move slower

    private void Start()
    {
        // Initialize HeartManager
        heartManager = FindObjectOfType<HeartManager>();
        if (heartManager == null)
        {
            Debug.LogError("HeartManager is not found in the scene!");
        }

        // Initialize PlayerMovement
        playerMovement = GetComponentInParent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component is not found on this GameObject!");
        }

        // Initialize SpawnManager
        spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager is not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        else if (other.CompareTag("Boost"))
        {
            HandleSpeedBoost(other);
        }
        else if (other.CompareTag("Enemy Slow"))
        {
            HandleEnemySlow(other);
        }
        else if (other.CompareTag("Heart"))
        {
            HandleHeartPickup(other);
        }
    }

    private void HandleEnemyCollision(Collider2D other)
    {
        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        if (playerRenderer != null)
        {
            playerRenderer.color = Color.red; // Change player's color to red
            StartCoroutine(ResetColorAfterDelay(playerRenderer, 0.5f));
            
            // Change the other player's color to red
            PlayerCollider otherPlayer = FindOtherPlayer();
            if (otherPlayer != null)
            {
                SpriteRenderer otherRenderer = otherPlayer.GetComponent<SpriteRenderer>();
                if (otherRenderer != null)
                {
                    otherRenderer.color = Color.red; // Change other player's color to red
                    StartCoroutine(otherPlayer.ResetColorAfterDelay(otherRenderer, 0.5f));
                }
            }
        }

        // Reduce shared player's lives and update hearts
        heartManager.UpdateHearts(heartManager.currentLives - 1);
        
        // Check for game over
        if (heartManager.currentLives <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Debug.Log("You Lose, Try Again!");
        }
    }

    private void HandleSpeedBoost(Collider2D other)
    {
        PlayerMovement[] allPlayers = FindObjectsOfType<PlayerMovement>(); // Find all player movement components

        foreach (PlayerMovement player in allPlayers)
        {
            player.speed *= speedMultiplier; // Apply the speed boost to each player
            StartCoroutine(ResetSpeedAfterDelay(player, speedBoostDuration)); // Start a coroutine to reset speed for each player
        }

        Destroy(other.gameObject); // Destroy the power-up after collision
        spawnManager.UpdatePowerupText("Boost - Player Movements X2", 5f);
    }

    private void HandleEnemySlow(Collider2D other)
    {
        StartCoroutine(SlowDownEnemies(enemySlowDuration)); // Slow down enemies for a duration
        Destroy(other.gameObject); 
        spawnManager.UpdatePowerupText("Enemy Slow - Enemy Movements Are Slowed", 5f);
    }

    private void HandleHeartPickup(Collider2D other)
    {
        if (heartManager.currentLives < heartManager.maxLives)
        {
            heartManager.UpdateHearts(heartManager.currentLives + 1); // Add a life
        }
        Destroy(other.gameObject); // Destroy the heart power-up after collision
        spawnManager.UpdatePowerupText("Heart - Players lives +1", 5f);
    }

    private PlayerCollider FindOtherPlayer()
    {
        // Find the other player based on playerIndex
        PlayerCollider[] players = FindObjectsOfType<PlayerCollider>();
        foreach (PlayerCollider player in players)
        {
            if (player != this)
            {
                return player; // Return the other player
            }
        }
        return null; // Return null if not found
    }

    private IEnumerator ResetSpeedAfterDelay(PlayerMovement player, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the duration of the boost
        if (player != null)
        {
            player.speed /= speedMultiplier; // Reset speed to original for each player
        }
    }

    private IEnumerator SlowDownEnemies(float duration)
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.speed *= 0.5f; // Reduce enemy speed by half
        }

        yield return new WaitForSeconds(duration);

        foreach (EnemyMovement enemy in enemies)
        {
            enemy.speed /= 0.5f; // Reset to original speed
        }
    }

    private IEnumerator ResetColorAfterDelay(SpriteRenderer renderer, float delay)
    {
        yield return new WaitForSeconds(delay);
        renderer.color = Color.white; // Reset to original color (white)
    }
}
