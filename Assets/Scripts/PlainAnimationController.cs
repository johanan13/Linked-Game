using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlainAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the animator component
    public string animationParameter = "isAnimating"; // The parameter name to control the animation

    void Start()
    {
        // Find the Animator component on the GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    void Update()
    {
        // This example assumes the animation is controlled by a boolean parameter
        // Here, we simply set it to true to start the animation
        if (animator != null)
        {
            animator.SetBool(animationParameter, true);
        }
    }
}
