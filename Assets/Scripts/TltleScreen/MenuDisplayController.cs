using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplayController : MonoBehaviour
{
    public GameObject menu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOpenMenuClick()
    {
        menu.SetActive(true);
    }

    public void OnCloseMenuClick()
    {
        menu.SetActive(false);
    }
}
