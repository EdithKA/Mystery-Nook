using System.Collections;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public float speed;
    public float waitTime = 3.0f;

    float timer;
    bool isWalking;
    int direction = 1;

    // Start is called before the first frame update


    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // No necesitas FixedUpdate para este caso
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            
            isWalking = false;
            timer = waitTime;

            StartCoroutine(WaitAndStartWalking());
        }

        setAnimation();
    }

    private void StartWalking()
    {
        isWalking = true;
        timer = waitTime;
        StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        while (isWalking)
        {
            Move();
            yield return null;
        }
    }

    private void Move()
    {
        //Debug.Log(direction);
        Vector2 position = rb.position;
        position.x = position.x + Time.deltaTime * speed * direction;
        animator.SetFloat("moveX", direction); // 1 para moverse hacia la derecha


        rb.MovePosition(position);

        
        
    }

    private IEnumerator WaitAndStartWalking()
    {
        
        yield return new WaitForSeconds(2f);
        
        StartWalking(); // Iniciar el movimiento después de esperar
    }

    void setAnimation()
    {
        animator.SetBool("isWalking", isWalking);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = -direction;
    }
}
