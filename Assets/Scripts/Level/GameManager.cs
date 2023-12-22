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

    // Start is called before the first frame update
    void Start()
    {
        sign.SetActive(false);
    }

    // Update is called once per frame
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
                signText.text = "Key1";
                break;
            case "Key2Sign":
                signText.text = "Key2";
                break;
            default:
                signText.text = "Unknown Text";
                break;
        }
    }



    


}
