using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angrybird : MonoBehaviour
{
    [SerializeField] private AudioClip hitClip;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool hasBeenLauched;
    private bool shouldFaceVelocityDir;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        rb.isKinematic = true;
        circleCollider.isTrigger = true;        
    }

    private void FixedUpdate()
    {
        if (hasBeenLauched && shouldFaceVelocityDir) 
        {
            //* The bird looks in the direction of his velocity
            transform.right = rb.velocity;
        }
    }

    public void LauchBird(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        circleCollider.isTrigger = false;

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLauched = true;
        shouldFaceVelocityDir = true;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        shouldFaceVelocityDir = false;
        SoundManager.instance.PlaySound(hitClip, audioSource);
        Destroy(this);
    }
}
