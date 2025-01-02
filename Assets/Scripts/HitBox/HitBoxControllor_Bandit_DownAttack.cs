using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxControllor_Bandit_DownAttack : HitBoxControllor_All
{
    // Boss_Bandit
    Boss_Bandit bandit_script;

    // Start is called before the first frame update
    protected override void Start()
    {
        isRightDefaultValue = false;    // this variable isn't required because don't use offset
        base.Start();

        bandit_script = thisGameobject.GetComponent<Boss_Bandit>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // this hitbox isn't required render left right switch
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
