using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 20f; // Movement speed of the enemy
    public float chaseDistance = 50f; // Distance within which the enemy starts chasing the player
    public float changeDirectionTime = 5f; // Time interval for changing direction for random movement
    private SpawnManager spawnManager;

    private Transform[] players; // Array to store player transforms
    private Transform targetPlayer; // Reference to the target player transform (the closer one)
    private Vector3 randomDirection; // Direction for random movement
    private float timer; // Timer to keep track of direction change

    // Animator reference (optional)
    private Animator animator;

    // Reference to the main camera
    private Camera mainCamera;

    // Margin for movement within camera view
    private float margin = 150.0f; // Adjust this value as needed

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        // Find all players by tag
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playerObjects.Length];
        for (int i = 0; i < playerObjects.Length; i++)
        {
            players[i] = playerObjects[i].transform;
        }

        if (players.Length == 0)
        {
            Debug.LogWarning("Players not found. Make sure the players have the 'Player' tag.");
        }

        // Initialize random direction
        SetRandomDirection();

        // Get the animator component (optional)
        animator = GetComponentInChildren<Animator>();

        // Get the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    void Update()
    {
        // Determine the closer player
        float closestDistance = chaseDistance;
        targetPlayer = null;

        foreach (Transform player in players)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    targetPlayer = player;
                }
            }
        }

        // Chase the target player if within chase distance
        if (targetPlayer != null)
        {
            ChasePlayer();
        }
        else
        {
            RandomMovement();
        }

        // Update animator speed if it exists
        // if (animator != null)
        // {
        //     animator.SetFloat("Speed", speed);
        // }
    }

    void RandomMovement()
    {
        // Move in the random direction
        Vector3 newPosition = transform.position + randomDirection * speed * Time.deltaTime;
        if (IsWithinCameraView(newPosition))
        {
            transform.position = newPosition;
            FlipTransform(randomDirection.x);
        }
        else
        {
            // Change direction if the new position is outside the camera view
            SetRandomDirection();
        }

        // Update timer and change direction at specified intervals
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            SetRandomDirection();
            timer = 0f; // Reset timer
        }
    }

    void ChasePlayer()
    {
        // Move towards the target player
        Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
        Vector3 newPosition = transform.position + directionToPlayer * speed * Time.deltaTime;
        if (IsWithinCameraView(newPosition))
        {
            transform.position = newPosition;
            FlipTransform(directionToPlayer.x);
        }
    }

    void SetRandomDirection()
    {
        // Generate a random direction
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        FlipTransform(randomDirection.x);
    }

    bool IsWithinCameraView(Vector3 position)
    {
        if (mainCamera == null)
        {
            return false;
        }

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        return viewportPoint.x > 0 + margin / mainCamera.pixelWidth && viewportPoint.x < 1 - margin / mainCamera.pixelWidth &&
               viewportPoint.y > 0 + margin / mainCamera.pixelHeight && viewportPoint.y < 1 - margin / mainCamera.pixelHeight;
    }

    void FlipTransform(float horizontalMovement)
    {
        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Face right
        }
        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Face left
        }
    }
private void OnTriggerEnter2D(Collider2D other)
{
    // Check if the enemy collides with a player or a magical link
    if (other.CompareTag("Player") || other.CompareTag("Link"))
    {
        StartCoroutine(DestroyEnemyAfterDelay(0.4f)); // Start coroutine with a 0.5 second delay
        //Debug.Log("Enemy triggered by: " + other.name); // Log trigger event
    }
}

private IEnumerator DestroyEnemyAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay); // Wait for the specified delay
   // Debug.Log("Destroying enemy after delay.");
    spawnManager.OnEnemyDestroyed();  // Log before destroying enemy
    Destroy(gameObject); // Destroy the enemy
}



}
