using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;


    public float speed = 3.0f;
    public int maxHealth;

    public GameObject arrowPrefab;

    private int currentHealth;
    

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    private Rigidbody2D rigidbody2d;
    private float horizontal;
    private float vertical;

    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    public AudioClip throwSound;
    public AudioClip hitSound;

    private AudioSource audioSource;
    private GameObject arrowObject;

    public int health { get { return currentHealth; } }
    private HeartsBarController heartsBarController;


    private void Awake()
    {
        currentHealth = maxHealth;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        heartsBarController = FindObjectOfType<HeartsBarController>();

        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Shoot();
        }

        OnPlayerDamaged?.Invoke();
        

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
        
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Llama a DrawHearts cada vez que cambia la salud
        heartsBarController.DrawHearts();

        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }


    void Shoot()
    {
        arrowObject = Instantiate(arrowPrefab, rigidbody2d.position + Vector2.up * 0.01f, Quaternion.identity);
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.shootDirection = lookDirection;
        arrow.Shoot(lookDirection, 300);

        animator.SetTrigger("Shoot");
        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ChangeHealth(-1);
        }
    }
}