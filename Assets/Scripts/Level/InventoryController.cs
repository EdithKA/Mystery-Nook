using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{

    public GameObject Key1, Key2, crossbow, knife;
    // Start is called before the first frame update
    void Start()
    {
        Key1.SetActive(false);
        Key2.SetActive(false);
        crossbow.SetActive(false);
        knife.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectCollected(string objectName)
    {
        if(objectName == "Key1")
        {
            Key1.SetActive(true);
            
        }
        if (objectName == "Key2")
        {
            Key2.SetActive(true);
        }
        if (objectName == "Crossbow")
        {
            crossbow.SetActive(true);

        }
        if (objectName == "Knife")
        {
            knife.SetActive(true);

        }
    }
}
