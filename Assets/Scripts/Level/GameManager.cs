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

   
    public void ViewSign()
    {
        if(sign.activeSelf)
        {
            sign.SetActive(false);
        }
        else
        {
            sign.SetActive(true);
        }
        
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
                signText.text = "There's a key lost in the forest";
                break;
            default:
                signText.text = "";
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
            case "NoKnife":
                helpText.text = "You don't have a knife";
                break;
            case "Key":
                helpText.text = "Key collected";
                break;
            case "Knife":
                helpText.text = "Knife collected";
                break;
            case "Crossbow":
                helpText.text = "Crossbow collected";
                break;
            default:
                helpText.text = "";
                break;
        }
    }






}
