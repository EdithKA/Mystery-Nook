using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    Animator animator;

    public Vector2 shootDirection = new Vector2(1, 0);

    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Shoot(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        
        SetAnimation();
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    void SetAnimation()
    {
        animator.SetFloat("Look X", shootDirection.x);
        animator.SetFloat("Look Y", shootDirection.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy (gameObject);
    }
}
