using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxControllor_Bandit : HitBoxControllor_All
{
    // Boss_Bandit
    Boss_Bandit bandit_script;

    protected override void Start()
    {
        isRightDefaultValue = false;
        base.Start();

        bandit_script = thisGameobject.GetComponent<Boss_Bandit>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)    // Player(Layer)
                                                // hit Player
        {
            player_script = collision.gameObject.GetComponent<PlayerController>();
            if (collision.gameObject != null) { player_script.Hit(transform, bandit_script.attackPower); }
            else { bandit_script.OffHitBox2D_left_right(); }
        }
    }

}
