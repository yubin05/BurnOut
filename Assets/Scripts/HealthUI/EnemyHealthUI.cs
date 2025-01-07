using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : HealthUIController
{
    Enemy_Legacy enemy;

    Sprite enemyHealthSprite;
    private readonly string enemyHealthSpriteFileName = "barRed_horizontalMid";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // �� ü�� �� �̹��� ����
        enemyHealthSprite = Resources.Load<Sprite>(healthSpriteFolderPath + enemyHealthSpriteFileName);
        GetComponent<Image>().sprite = enemyHealthSprite;

        enemy = transform.parent.parent.parent.GetComponent<Enemy_Legacy>();
        health_amount = enemy.health;
        max_health_amount = enemy.max_health;

        healthUI.fillAmount = (health_amount / (float)max_health_amount);
    }
}
