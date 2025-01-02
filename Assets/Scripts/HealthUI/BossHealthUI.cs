using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : HealthUIController
{
    Sprite bossHealthSprite;
    private readonly string bossHealthSpriteFileName = "barBlue_horizontalBlue";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // 보스 체력 바 이미지 설정
        bossHealthSprite = Resources.Load<Sprite>(healthSpriteFolderPath + bossHealthSpriteFileName);
        GetComponent<Image>().sprite = bossHealthSprite;

        health_amount = transform.root.GetComponent<Boss>().health;
        max_health_amount = transform.root.GetComponent<Boss>().max_health;

        healthUI.fillAmount = (health_amount / (float)max_health_amount);
    }
}
