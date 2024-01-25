using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angrybird : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        rb.isKinematic = true;
        circleCollider.isTrigger = true;        
    }

    public void LauchBird(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        circleCollider.isTrigger = false;

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
