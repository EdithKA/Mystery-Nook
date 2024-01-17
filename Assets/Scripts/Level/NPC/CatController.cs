using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public bool isWalking = false;
    public float changeTime = 3.0f;

    Rigidbody2D rb;
    float timer;
    int direction = 1;

    Animator animator;

    public GameObject textDialog;
  



    void Start()
    {
        

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textDialog.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
    }

    private void FixedUpdate()
    {
        setAnimation();
        if (isWalking == false)
        {
            StartCoroutine(Talk());
        }
        else
        {

            Move();
        }
       
    }


    void Move()
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

    private IEnumerator Talk()
    {
        textDialog.SetActive(true);
        direction = -direction;
        yield return new WaitForSeconds(5f);

        timer = changeTime;
        textDialog.SetActive(false);

    }

  

    void setAnimation()
    {
        animator.SetBool("Vertical", vertical);
        animator.SetBool("isWalking", isWalking);
    }

    
}
