using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{

    public void onPlayButtonClick()
    {
        SceneManager.LoadScene(sceneName: "Level");
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }

}
