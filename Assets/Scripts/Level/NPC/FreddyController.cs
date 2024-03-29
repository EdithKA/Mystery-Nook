using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreddyController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField]  float maxHealth;
    [SerializeField] HealthBarController healthBar;


    public float changeTime = 3.0f;
    float currentHealth;
    private Rigidbody2D rb;
    private float timer;
    private int currentDirection = 0; // 0: Derecha, 1: Abajo, 2: Izquierda, 3: Arriba
    private bool changingDirection = false;

    private Animator animator;

    //Sound
    AudioSource audioSource;
    [SerializeField] AudioClip moveSound;

    bool attacking;

    private void Awake()
    {
        attacking = false;
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBarController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentHealth = maxHealth;

    }

    void Update()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        animator.SetBool("Vertical", currentDirection == 1 || currentDirection == 3);
    }

    void FixedUpdate()
    {
        if(!attacking)
        {
            if (!changingDirection)
            {
                Move();
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    ChangeDirection();
                }
            }
        }
    }

    private void Move()
    {
        Vector2 position = rb.position;

        switch (currentDirection)
        {
            case 0: // Derecha
                position.x += Time.deltaTime * speed;
                break;
            case 1: // Abajo
                position.y -= Time.deltaTime * speed;
                break;
            case 2: // Izquierda
                position.x -= Time.deltaTime * speed;
                break;
            case 3: // Arriba
                position.y += Time.deltaTime * speed;
                break;
        }

        rb.MovePosition(position);
    }

    private void ChangeDirection()
    {
        StartCoroutine(moveEffect());
        currentDirection = (currentDirection + 1) % 4; // Cambia la direcci�n en el patr�n derecha, abajo, izquierda, arriba
        timer = changeTime;

        StartCoroutine(ChangeDirectionCooldown());
    }

    private IEnumerator ChangeDirectionCooldown()
    {
        changingDirection = true;
        yield return new WaitForSeconds(0.5f); // Tiempo de espera antes de poder cambiar de direcci�n nuevamente
        changingDirection = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ChangeDirection();
        }
        if(collision.gameObject.tag == "arrow")
        {
            ChangeHealth(-1);
        }
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Attack());
        }
    }

    public void ChangeHealth(int amount)
    {
        
        currentHealth += amount;
        
        Debug.Log(currentHealth);
    }

    IEnumerator moveEffect()
    {
        PlaySound(moveSound);
        yield return new WaitForSeconds(2);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    IEnumerator Attack()
    {
        attacking = true;
        float prevSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        attacking = false;
        speed = prevSpeed;
    }
}
