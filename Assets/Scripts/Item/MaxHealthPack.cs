using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPack : ItemPack
{
    protected override void TriggeredItem(GameObject player, PlayerController player_script)
    {
        player_script.max_health += addValue;
        player.GetComponentInChildren<PlayerHealthUI>().setMaxHealthUI(player_script.max_health);    // player health UI control

        // 체력 최대치 추가분만큼 체력 회복
        // 체력 최대치 추가 이후 이어지는 작업이므로 예외 처리 필요없음
        player_script.health += addValue;
        player.GetComponentInChildren<PlayerHealthUI>().setHealthUI(player_script.health);    // player health UI control
    }
}
