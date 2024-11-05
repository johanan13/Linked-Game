using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalLink : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
       // Debug.Log("Trigger entered by: " + other.name); // Log when trigger is entered

        if (other.CompareTag("Enemy"))
        {
        
            // Change the enemy color to red
            SpriteRenderer enemyRenderer = other.GetComponent<SpriteRenderer>();
            if (enemyRenderer != null)
            {
               // Debug.Log("Enemy detected: " + other.name); // Log enemy detection
                enemyRenderer.color = Color.red;
            }
            
            other.GetComponent<Collider2D>().enabled = false;
            // Make the enemy disappear after a short delay
             // 1 second delay
        }
    }

}
