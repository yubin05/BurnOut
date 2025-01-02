using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : ItemPack
{    
    protected override void TriggeredItem(GameObject player, PlayerController player_script)
    {
        player_script.health += addValue;
        player.GetComponentInChildren<PlayerHealthUI>().setHealthUI(player_script.health);    // player health UI control

        // check health value overflow
        if (player_script.health > player_script.max_health)
        {
            player_script.health = player_script.max_health;
            player.GetComponentInChildren<PlayerHealthUI>().setHealthUI(player_script.max_health);
        }
    }
}
