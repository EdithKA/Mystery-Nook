using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    


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
    private GameManager gameManager;
    private InventoryController inventoryController;

    bool hasCrossbow = false;
    int numKeys = 0;

    public ParticleSystem HealthEffect;
    public ParticleSystem CollectEffect;


    private void Awake()
    {
        currentHealth = maxHealth;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        heartsBarController = FindObjectOfType<HeartsBarController>();

        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        inventoryController = FindObjectOfType<InventoryController>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            gameManager.changeScene("GameOver");
        }
     

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

        if (Input.GetKeyDown(KeyCode.G) && hasCrossbow)
        {
            Shoot();
        }

        OnPlayerDamaged?.Invoke();
        

        
        
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
        if(collision.gameObject.name == "House")
        {
            if(numKeys == 2)
            {
                gameManager.changeScene("End");
            }
        }

        if (collision.gameObject.tag == "HealthCollectible")
        {
            ChangeHealth(1);
            HealthEffect.Play();
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectEffect.Play();

        if (collision.gameObject.name == "Crossbow")
        {
            hasCrossbow = true;
        }
        if(collision.gameObject.tag  == "Key")
        {
            numKeys += 1;
        }
       

        inventoryController.ObjectCollected(collision.gameObject.name);
       Destroy(collision.gameObject);
        
    }
}