using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpPack : ItemPack
{
    protected override void TriggeredItem(GameObject player, PlayerController player_script)
    {
        player_script.player_sword_attack_power += addValue;
    }
}
