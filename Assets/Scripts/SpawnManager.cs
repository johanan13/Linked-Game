using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] powerups;
    // private GameOverManager gameOverManager;

    public TMP_Text roundText;
    private CanvasGroup roundTextCanvasGroup;
    private int currentRound = 1;

    private float powerupSpawnTime = 12.0f;
    private float powerupLifetime = 15.0f;
    private float enemySpawnTime = 4.0f;
    private float startDelay = 3.0f;

    private int maxEnemies;
    private int enemyCount = 0;
    private float enemySpeedMultiplier = 1.0f;

    private Camera mainCamera;
    private Transform[] players;

    private float margin = 150.0f;
    private float minimumDistanceFromPlayers = 60.0f;

    private bool allEnemiesSpawned = false;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private TMP_Text powerupSubs;
    // [SerializeField] private CharacterLoader characterLoader;

    void Start()
    {
        mainCamera = Camera.main;

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playerObjects.Length];
        for (int i = 0; i < playerObjects.Length; i++)
        {
            players[i] = playerObjects[i].transform;
        }

        roundTextCanvasGroup = roundText.GetComponent<CanvasGroup>();
        StartRound();
    }

    void StartRound()
    {
        switch (currentRound)
        {
            case 1:
                maxEnemies = 7;
                enemySpeedMultiplier = 1.0f;
                break;
            case 2:
                maxEnemies = 10;
                enemySpeedMultiplier = 1.1f;
                break;
            case 3:
                maxEnemies = 12;
                enemySpeedMultiplier = 1.2f;
                break;
        }

        Debug.Log("Starting Round " + currentRound);
        roundText.text = "Round " + currentRound;
        roundText.gameObject.SetActive(true);
        roundTextCanvasGroup.alpha = 1.0f;
        StartCoroutine(FadeOutRoundTextAfterDelay());

        enemyCount = 0;
        allEnemiesSpawned = false;
        InvokeRepeating("SpawnRandomEnemy", startDelay, enemySpawnTime / enemySpeedMultiplier);
        InvokeRepeating("SpawnPowerup", startDelay, powerupSpawnTime);
    }

    void SpawnRandomEnemy()
    {
        if (enemyCount >= maxEnemies)
        {
            CancelInvoke("SpawnRandomEnemy");
            allEnemiesSpawned = true;
            OnEnemyDestroyed();
            Debug.Log("All enemies for the round have been spawned.");
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPositionWithinCameraView();
        int randomIndex = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[randomIndex], spawnPos, enemies[randomIndex].transform.rotation);
        enemy.tag = "Enemy"; // Ensure the enemy is tagged correctly

        enemyCount++;
        Debug.Log("Enemy spawned. Current count: " + enemyCount);
    }

    void SpawnPowerup()
    {
        Vector3 spawnPos = GetRandomSpawnPositionWithinCameraView();
        int randomIndex = Random.Range(0, powerups.Length);
        GameObject powerup = Instantiate(powerups[randomIndex], spawnPos, powerups[randomIndex].transform.rotation);
        StartCoroutine(DestroyPowerupAfterDelay(powerup, powerupLifetime));
    }

    public IEnumerator DestroyPowerupAfterDelay(GameObject powerup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (powerup != null)
        {
            Destroy(powerup);
        }
    }

    Vector3 GetRandomSpawnPositionWithinCameraView()
    {
        Vector3 spawnPos;
        bool positionIsSafe;

        do
        {
            Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            float randomX;
            float randomY;

            randomizeSpawn:
            randomX = Random.Range(minBounds.x + margin, maxBounds.x - margin);
            randomY = Random.Range(minBounds.y + margin, maxBounds.y - margin);
            Collider2D[] colls = Physics2D.OverlapCircleAll(new Vector2(randomX, randomY), 50f);
            foreach (Collider2D coll in colls) {
                if (coll.GetComponent<PlayerMovement>() != null) {
                    goto randomizeSpawn;
                }
            }

            spawnPos = new Vector3(randomX, randomY, 0);

            positionIsSafe = true;
            foreach (Transform player in players)
            {
                if (Vector3.Distance(spawnPos, player.position) < minimumDistanceFromPlayers)
                {
                    positionIsSafe = false;
                    break;
                }
            }
        } while (!positionIsSafe);

        return spawnPos;
    }

public void OnEnemyDestroyed()
{
    // Call the coroutine to check for the next round whenever an enemy is destroyed
    StartCoroutine(CheckForNextRound());
    Debug.Log("CheckForNextRound");
    Debug.Log("CheckForNextRound");
}

private IEnumerator CheckForNextRound()
{
    yield return new WaitForSeconds(0.5f); // Small delay to ensure proper timing

    // Check how many active enemies remain in the scene
    int remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

    // Check if all enemies have been spawned and no enemies remain in the scene
    if (remainingEnemies == 0 && allEnemiesSpawned == true)
    {
        currentRound++;
        if (currentRound <= 3) // Continue to the next round if within the round limit
        {
            StartRound();
        }
 else
{
    string elapsedTime = gameTimer.FormatTime(gameTimer.GetElapsedTime());

    // Set the static variable directly before loading the scene
    GameOverManager.ElapsedTime = elapsedTime;
    // Load the GameOver scene
    SceneManager.LoadScene("GameOver");
}


    }
}


    private IEnumerator FadeOutRoundTextAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);

        float fadeDuration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            roundTextCanvasGroup.alpha = alpha;
            yield return null;
        }

        roundTextCanvasGroup.alpha = 0;
        roundText.gameObject.SetActive(false);
    }
    public void UpdatePowerupText(string message, float duration)
    {
        powerupSubs.text = message; // Update the power-up text
        StartCoroutine(HidePowerupTextAfterDelay(duration)); // Start the coroutine to hide it
    }

    private IEnumerator HidePowerupTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified duration
        powerupSubs.text = ""; // Clear the text
    }
}
