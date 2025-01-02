using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player's left and right hitbox
public class HitBoxController : HitBoxControllor_All
{
    // Enemy & Boss
    Enemy enemy; Boss boss;

    protected override void Start()
    {
        isRightDefaultValue = true;
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)    // Enemy(Layer)
            // hit enemy
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy?.Hit(player_script.player_sword_attack_power);
        }
        if (collision.gameObject.layer == 11)    // Boss(Layer)
            // hit boss
        {
            boss = collision.gameObject.GetComponent<Boss>();
            boss?.Hit(player_script.player_sword_attack_power);
        }
    }
}
