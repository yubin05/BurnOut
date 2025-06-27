using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : ActionButton
{
    protected override void Init()
    {
        m_button.onClick.RemoveAllListeners();
        m_button.onClick.AddListener(() =>
        {
            var players = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
            if (players != null)
            {
                var player = players[0];
                var playerObj = player.MyObject as PlayerObject;
                if (playerObj.MotionHandler.IsHit || playerObj.MotionHandler.IsDeath) return;

                playerObj.OnAttack();
            }
        });
    }
}
