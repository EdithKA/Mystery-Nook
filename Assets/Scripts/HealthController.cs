using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Sprite fullHeart, damageHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartSprite(HeartSprite status)
    {
        switch (status)
        {
            case HeartSprite.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartSprite.Damage:
                heartImage.sprite = damageHeart;
                break;
            case HeartSprite.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartSprite
{
    Empty = 0,
    Damage = 1,
    Full = 2
}
