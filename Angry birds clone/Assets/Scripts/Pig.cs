using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float damageThreshold = 0.2f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DamageBaddie(float damageAmouth)
    {
        currentHealth -= damageAmouth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.RemovePig(this);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //* Damages the pig with the velocity of the other object colliding with him
        float impactVelocity = other.relativeVelocity.magnitude;

        if (impactVelocity > damageThreshold)
        {
            DamageBaddie(impactVelocity);
        }
    }
}
