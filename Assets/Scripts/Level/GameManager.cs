using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text signText;
    public GameObject sign;

    
    public Text helpText;

    // Start is called before the first frame update
    void Start()
    {
        sign.SetActive(false);
    }

   
    public void ViewSign(bool visible)
    {
        sign.SetActive(visible);
    }

    public void changeScene(string scene)
    {
        SceneManager.LoadScene(sceneName: scene);
    }

    public void SetSignText(string text)
    {
        switch (text)
        {
            case "HouseSign":
                signText.text = "Shelter";
                break;
            case "Key1Sign":
                signText.text = "The first key is after passing the bridge.";
                break;
            case "Key2Sign":
                signText.text = "Key2";
                break;
            default:
                signText.text = "Unknown Text";
                break;
        }
    }

    public void setHelpText(string text)
    {
        switch (text)
        {
            case "NoCrossbow":
                helpText.text = "You don't have a crossbow";
                break;
            case "Post":
                helpText.text = "Press E to read";
                break;
            case "Key2Sign":
                helpText.text = "Key2";
                break;
            default:
                helpText.text = "";
                break;
        }
    }






}
