using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    [SerializeField] float maxHealth, speed, changeTime;
    [SerializeField] HealthBarController healthBar;
    

    public bool vertical;
    float currentHealth;

    Rigidbody2D rb;
    float timer;
    int direction = 1;

    Animator animator;

    //Sound
    AudioSource audioSource;
    [SerializeField] AudioClip moveSound;


    private void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if ((currentHealth <= 0))
        {
            StartCoroutine(Death());
        }
        timer -= Time.deltaTime;
        
        if(timer < 0)
        {
            StartCoroutine(moveEffect());
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        rb.MovePosition(position);
    }


    private IEnumerator Death()
    {
        speed = 0;
        animator.SetTrigger("Death"); 

        yield return new WaitForSeconds(1f);  

        
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "arrow")
        {
            ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount)
    {

        currentHealth += amount;

        
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

}
