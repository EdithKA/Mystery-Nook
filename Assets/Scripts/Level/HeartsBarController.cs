using System.Collections.Generic;
using UnityEngine;

public class HeartsBarController : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerController playerController;

    private List<HealthController> hearts = new List<HealthController>();


    private void OnEnable()
    {
       
        PlayerController.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDamaged -= DrawHearts;
    }
    private void Start()
    {
        DrawHearts();
    }
    public void DrawHearts()
    {
        ClearHearts();


        float maxHealthRemainder = playerController.maxHealth % 2;
        int heartsToMake = (int)((int)(playerController.maxHealth / 2) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = Mathf.Clamp(playerController.health - (i * 2), 0, 2);
            hearts[i].SetHeartSprite((HeartSprite) heartStatusRemainder);
        }
    }
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthController heartComponent = newHeart.GetComponent<HealthController>();
        heartComponent.SetHeartSprite(HeartSprite.Empty);
        hearts.Add(heartComponent);
    }
    
    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthController>();
    }

    
}
