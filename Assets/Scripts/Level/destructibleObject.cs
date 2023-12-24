using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructibleObject : MonoBehaviour
{
    [SerializeField] int health, numSprites;

    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    AudioSource AudioSource;
    [SerializeField] AudioClip breakSound;
    // Start is called before the first frame update


    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        health = numSprites;

    }

    private void Update()
    {
       
    }

    void damageObject(int amount)
    {
        health += amount;
    }



}
