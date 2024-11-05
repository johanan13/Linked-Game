using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;
    public bool isPlayer1; // To differentiate between Player 1 and Player 2
    private float moveHorizontal;
    private float moveVertical;
    private Animator animator; // Reference to the animator component

    // Reference to the main camera
    private Camera mainCamera;

    // Margin for movement within camera view
    private float margin = 150.0f; // Adjust this value as needed

    void Start()
    {
        // Find the Animator component on the child character prefab
        animator = GetComponentInChildren<Animator>();

        // if (animator == null)
        // {
        //     Debug.LogError("Animator component not found in the character prefab!");
        // }

        // Get the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    void Update()
    {
        // Handle input based on player slot
        if (isPlayer1)
        {
            // Player 1 uses WASD keys
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        else
        {
            // Player 2 uses Arrow keys
            moveHorizontal = Input.GetAxis("Horizontal2");
            moveVertical = Input.GetAxis("Vertical2");
        }

        // Log inputs to debug
        // Debug.Log("moveHorizontal: " + moveHorizontal);
        // Debug.Log("moveVertical: " + moveVertical);

        // Calculate current speed
        float currentSpeed = Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical);

        // Update the animator's speed parameter
        // if (animator != null)
        // {
        //     animator.SetFloat("Speed", currentSpeed);
        // }

        // Only move if there is input
        if (currentSpeed > 0)
        {
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
            Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
            if (IsWithinCameraView(newPosition))
            {
                transform.position = newPosition;
            }

            // Flip the transform based on the movement direction
            FlipTransform(moveHorizontal);
        }

        // Debugging
        //Debug.Log("moveHorizontal (after movement): " + moveHorizontal);
        //Debug.Log("Current Speed: " + currentSpeed);

        GetComponentInChildren<PlayerAnimationController>().speed = currentSpeed;
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
    // Check if the player is moving to the right or left
    if (horizontalMovement > 0)
    {
        // Face right
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    else if (horizontalMovement < 0)
    {
        // Face left
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
}
