using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 1;
        isRightDefaultValue = true;
    }

    protected override void Start()
    {
        base.Start();

        // get audio manager
        audioManager = gameObject.GetComponentInChildren<AudioManager_Slime>();
    }

    public override void Hit(int player_attack_power)
    {
        base.Hit(player_attack_power);

        // play hurt sound
        audioManager.PlayOneShotAudio("Hurt", 0.5f, 0.7f);
    }

    public override void Dead()
    {
        if (!playedDeadSound)
        {
            audioManager.PlayOneShotAudio("Death", 1f, 0.7f);    // Enemy Dead Sound
            playedDeadSound = true;
        }
        base.Dead();
    }
}
