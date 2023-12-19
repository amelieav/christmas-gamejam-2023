using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public float maxDamage = 100f; // Maximum damage the block can take before being destroyed
    private float currentDamage = 0f; // Current damage level of the block

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate damage based on collision force
        float damage = collision.relativeVelocity.magnitude;

        // Add an additional damage condition for falling from height
        if (collision.gameObject.CompareTag("Ground") && transform.position.y >= 3 * Mathf.Max(transform.localScale.x, transform.localScale.y))
        {
            damage *= 2; // Increase damage if falling from a height
        }

        // Apply damage if colliding with a projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(damage);
        }
    }

    void TakeDamage(float damage)
    {
        currentDamage += damage;
        if (currentDamage >= maxDamage)
        {
            Destroy(gameObject); // Destroy the block if it exceeds the max damage
        }
    }
}
