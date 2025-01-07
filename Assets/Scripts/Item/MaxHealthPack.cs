using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPack : ItemPack
{
    protected override void TriggeredItem(GameObject player, PlayerController_Legacy player_script)
    {
        player_script.max_health += addValue;
        player.GetComponentInChildren<PlayerHealthUI>().setMaxHealthUI(player_script.max_health);    // player health UI control

        // ü�� �ִ�ġ �߰��и�ŭ ü�� ȸ��
        // ü�� �ִ�ġ �߰� ���� �̾����� �۾��̹Ƿ� ���� ó�� �ʿ����
        player_script.health += addValue;
        player.GetComponentInChildren<PlayerHealthUI>().setHealthUI(player_script.health);    // player health UI control
    }
}
