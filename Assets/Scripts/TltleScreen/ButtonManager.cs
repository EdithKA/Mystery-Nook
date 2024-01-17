using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip buttonClick;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayButtonClick()
    {
        PlaySound(buttonClick); 
        SceneManager.LoadScene(sceneName: "Level");
    }
    public void OnExitButtonClick()
    {
        PlaySound(buttonClick); 
        Application.Quit();
    }

    public void onMainMenuClick()
    {
        PlaySound(buttonClick);
        SceneManager.LoadScene(sceneName: "MainMenu");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
