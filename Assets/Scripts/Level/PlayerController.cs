using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDamaged;

    // Movement
    public float speed = 3.0f;
    private Rigidbody2D rigidbody2d;
    private float horizontal;
    private float vertical;

    // Health
    public int maxHealth;
    private int currentHealth;
    public int health { get { return currentHealth; } }
    private HeartsBarController heartsBarController;
    public ParticleSystem HealthEffect;

    // Inventory + Shoot
    public GameObject arrowPrefab;
    private GameObject arrowObject;
    private GameManager gameManager;
    private InventoryController inventoryController;
    public ParticleSystem CollectEffect;

    List<string> Inventory = new List<string>();

    public int numKeys = 0;

    bool canAttack = true;
    public float attackCooldown = 1.0f; // Tiempo entre ataques
    public float attackDuration = 0.01f; // Duración del ataque
    public bool isAttacking = false;
    bool isWalking = false;
    string weapon;

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    // Animations
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    public AudioClip damageSound, shootSound, stabSound, walkSound, collectSound, healEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        heartsBarController = FindObjectOfType<HeartsBarController>();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        inventoryController = FindObjectOfType<InventoryController>();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }

        Vector2 move = Vector2.zero;

        if (!isAttacking)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                if (!audioSource.isPlaying)
                {
                    PlaySound(walkSound);
                }
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();

                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        animator.SetBool("IsWalking", isWalking);
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        OnPlayerDamaged?.Invoke();

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Inventory.Contains("Crossbow"))
            {
                if (canAttack)
                {
                    weapon = "Crossbow";
                    StartCoroutine(Attack());
                }
                
            }
            else
            {
                StartCoroutine(ViewHelpText("NoCrossbow"));
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Inventory.Contains("Knife"))
            {
                if(canAttack)
                {
                    weapon = "Knife";
                    StartCoroutine(Attack());
                }
          
            }
            else
            {
                StartCoroutine(ViewHelpText("NoKnife"));
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        if (isInvincible)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            PlaySound(damageSound);
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        heartsBarController.DrawHearts();
    }

    private IEnumerator Attack()
    {
        isWalking = false;
        isAttacking = true;
        speed = 0;

        if (weapon == "Crossbow")
        {
            Vector2 arrowSpawnPosition = rigidbody2d.position + Vector2.up * 0.5f;
            arrowObject = Instantiate(arrowPrefab, arrowSpawnPosition, Quaternion.identity);
            Arrow arrow = arrowObject.GetComponent<Arrow>();
            arrow.shootDirection = lookDirection;
            arrow.Shoot(lookDirection, 300);

            animator.SetTrigger("Shoot");
            PlaySound(shootSound);

            yield return new WaitForSeconds(attackDuration); // Espera a que termine el ataque

            isAttacking = false;
            speed = 3.0f;

            StartCoroutine(AttackCooldown());
        }
        else if (weapon == "Knife")
        {
            animator.SetTrigger("Stab");
            PlaySound(stabSound);
            yield return new WaitForFixedUpdate();

            isAttacking = false;
            speed = 3.0f;

            StartCoroutine(AttackCooldown());
        }
    }

   

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!isAttacking) //No lo coloco en el stay porque si no el audio de recibir daño se reproduce múltiples veces seguidas
        {
            switch (collisionObject.tag)
            {
                case "Snake":
                    ChangeHealth(-1);
                    break;
                case "Bear":
                    ChangeHealth(-2);
                    break;
            }
        }
        switch (collisionObject.tag)
        {

            case "House":
                if (numKeys == 2)
                {
                    gameManager.changeScene("End");
                }
                break;
            case "HealthCollectible":
                if (currentHealth < maxHealth)
                {
                    ChangeHealth(1);
                    HealthEffect.Play();
                    PlaySound(healEffect);
                    Destroy(collision.gameObject);
                }
                break;
            case "post":
                gameManager.SetSignText(collision.gameObject.name);
                gameManager.ViewSign();
                break;
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (isAttacking && weapon == "Knife")
        {
            switch (collisionObject.tag)
            {
           
                case "Snake":
                    collisionObject.GetComponent<SnakeController>().ChangeHealth(-3);
                    break;
               
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "post")
        {
            gameManager.ViewSign();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectCollected = collision.gameObject.name;

        if (collision.gameObject.tag == "Weapon")
        {
            Inventory.Add(objectCollected);
        }

        StartCoroutine(ViewHelpText(objectCollected));
        inventoryController.ObjectCollected(objectCollected);

        if (collision.gameObject.tag == "Object" || collision.gameObject.tag == "HealthCollectible" || collision.gameObject.tag == "Key" || collision.gameObject.tag == "Weapon")
        {
            if (collision.gameObject.tag == "Key") { numKeys += 1; }
            Destroy(collision.gameObject);
            CollectEffect.Play();
            PlaySound(collectSound);
        }
    }

    private IEnumerator ViewHelpText(string helpCase)
    {
        gameManager.setHelpText(helpCase);
        yield return new WaitForSeconds(1f);
        gameManager.setHelpText("Default");
    }

    private IEnumerator Death()
    {
        rigidbody2d.simulated = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2.5f);
        gameManager.changeScene("GameOver");
    }
}
