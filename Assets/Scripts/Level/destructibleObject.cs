using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructibleObject : MonoBehaviour
{
    [SerializeField] int numSprites, index;

    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    AudioSource AudioSource;
    [SerializeField] AudioClip breakSound;
    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called before the first frame update


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AudioSource = GetComponent<AudioSource>();
        
        index = sprites.Count - 1;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "arrow")
        {
            damageObject();
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().isAttacking)
        {
            damageObject();
        }
    }

    void damageObject()
    {
        index -= 1;
        if(index >= 1)
        {
            spriteRenderer.sprite = sprites[index];
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }



}
