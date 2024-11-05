using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public float speed;

    void Update()
    {
        // Set the speed parameter for animations
        animator.SetFloat("speed", speed);
       // Debug.Log("Current Speed: " + speed);
    }

}
