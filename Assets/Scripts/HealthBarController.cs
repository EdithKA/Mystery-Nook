using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerController playerHealth;
    List<HealthController> hearts = new List<HealthController>();



    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthController> ();
    }
}
