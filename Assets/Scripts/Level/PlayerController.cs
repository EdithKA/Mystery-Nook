using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    List<String> Inventory = new List<String>();

    bool hasCrossbow = false;
    int numKeys = 0;

    bool isAttacking = false;
    bool isWalking = false;
    string weapon;

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    // Animations
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    public AudioClip damageSound, shootSound, stabSound, walkSound, collectSound, healEffet;
    private AudioSource audioSource;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(hasCrossbow);
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
               if(!audioSource.isPlaying)
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
                weapon = "Crossbow";
                StartCoroutine(Attack());
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
                weapon = "Knife";
                StartCoroutine(Attack());
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
            // Manejar el parpadeo durante el per�odo de invulnerabilidad
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true; // Asegurar que el sprite sea visible cuando no es invulnerable
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
            Vector2 arrowSpawnPosition = rigidbody2d.position + Vector2.up * 0.5f; // Ajusta la posici�n en el eje Y

            arrowObject = Instantiate(arrowPrefab, arrowSpawnPosition, Quaternion.identity);
            Arrow arrow = arrowObject.GetComponent<Arrow>();
            arrow.shootDirection = lookDirection;
            arrow.Shoot(lookDirection, 300);

            animator.SetTrigger("Shoot");
            PlaySound(shootSound);
        }
        else if (weapon == "Knife")
        {
            animator.SetTrigger("Stab");
            PlaySound(stabSound);
        }

        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        speed = 3.0f;
    }



    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (isAttacking && weapon == "Knife")
        {
            switch(collisionObject.tag)
            {
                case "Bear":
                    
                    break;
                case "Snake":
                    collisionObject.GetComponent<SnakeController>().ChangeHealth(-3);
                    break;

            }
        }
        else
        {

            switch(collisionObject.tag)
            {
                case "Snake":
                    ChangeHealth(-1);
                    break;
                case "Bear":
                    ChangeHealth(-2);
                    break;
                case "House":
                    if (numKeys == 2)
                    {
                        gameManager.changeScene("End");
                    }
                    break;
                case "HealthCollectible":
                    if(currentHealth < maxHealth)
                    {
                        ChangeHealth(1);
                        HealthEffect.Play();
                        PlaySound(healEffet);
                        Destroy(collision.gameObject);
                    }
                    break;
                case "post":
                    gameManager.SetSignText(collision.gameObject.name);
                    gameManager.ViewSign();
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

        switch (objectCollected)
        {
            case "Crossbow":
                Inventory.Add("Crossbow");
                break;
            case "Knife":
                Inventory.Add("Knife");
                break;
            case "Key":
                numKeys += 1;
                break;
            default:
                break;
        }

        StartCoroutine(ViewHelpText(objectCollected));
        inventoryController.ObjectCollected(objectCollected);
        if (collision.gameObject.tag == "Object" || collision.gameObject.tag == "HealthCollectible" || collision.gameObject.tag == "Key") { 
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
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(2.5f);

        gameManager.changeScene("GameOver");
    }
}
