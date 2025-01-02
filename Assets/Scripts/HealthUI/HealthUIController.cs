using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    protected Image healthUI;
    protected Sprite bgHealthBarSprite;
    protected Color bgHealthBarSpriteColor;
    protected float bgHealthBarSpriteColorAlphaValue;

    protected int health_amount;
    protected int max_health_amount;

    protected readonly string healthSpriteFolderPath = "HealthSprites/";
    protected readonly string bgHealthBarSpriteFileName = "barYellow_horizontalMid";

    protected virtual void Start()
    {
        healthUI = GetComponent<Image>();
        //healthUI.fillAmount = 1f;     // fill amount initalize lower class

        bgHealthBarSprite = Resources.Load<Sprite>(healthSpriteFolderPath + bgHealthBarSpriteFileName);
        Image bgHealthBarImg = transform.parent.GetComponent<Image>();
        bgHealthBarImg.sprite = bgHealthBarSprite;

        bgHealthBarSpriteColorAlphaValue = (150f / 255f);
        bgHealthBarSpriteColor = new Color(0, 0, 0, bgHealthBarSpriteColorAlphaValue);
        bgHealthBarImg.color = bgHealthBarSpriteColor;
    }

    // following method controled by other Script
    public virtual void setHealthUI(int newHealth)
    {
        health_amount = newHealth;
        healthUI.fillAmount = (health_amount / (float)max_health_amount);
    }
    public virtual void setMaxHealthUI(int newMaxHealth)
    {
        max_health_amount = newMaxHealth;
        healthUI.fillAmount = (health_amount / (float)max_health_amount);
    }
}
