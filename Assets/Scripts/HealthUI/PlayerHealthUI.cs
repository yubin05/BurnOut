using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : HealthUIController
{
    Sprite playerHealthSprite;
    private readonly string playerHealthSpriteFileName = "barGreen_horizontalMid";

    // player health amount text
    [SerializeField] Text playerHealthText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // 플레이어 체력 바 이미지 설정
        playerHealthSprite = Resources.Load<Sprite>(healthSpriteFolderPath + playerHealthSpriteFileName);
        GetComponent<Image>().sprite = playerHealthSprite;

        health_amount = transform.root.GetComponentInChildren<PlayerController>().health;
        max_health_amount = transform.root.GetComponentInChildren<PlayerController>().max_health;

        healthUI.fillAmount = (health_amount / (float)max_health_amount);
        playerHealthText.text = health_amount.ToString();
    }

    public override void setHealthUI(int newHealth)
    {
        base.setHealthUI(newHealth);

        playerHealthText.text = newHealth.ToString();
    }
}
