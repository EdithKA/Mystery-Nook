using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplayController : MonoBehaviour
{
    public GameObject menu;
    public AudioSource audioSource;
    [SerializeField] private AudioClip buttonClick;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnOpenMenuClick()
    {
        PlaySound(buttonClick); 
        menu.SetActive(true);
    }

    public void OnCloseMenuClick()
    {
        PlaySound(buttonClick); 
        menu.SetActive(false);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
