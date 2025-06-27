using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : ActionButton
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

                // 공격 모션 중이거나 점프 모션 중이 아닐 때만 점프
                if (!playerObj.MotionHandler.IsAttack && !playerObj.MotionHandler.IsJump)
                {
                    playerObj.OnJump();
                }
                // 점프 중일 때 한번 더 누르면 더블 점프
                else if (!playerObj.MotionHandler.IsAttack && playerObj.MotionHandler.IsJump && !player.IsDoubleJump)  // 더블 점프
                {
                    //player.Skills.Use(GameApplication.Instance.GameModel.ClientData.PlayerKeyCodes.playerKeyCodes.Find(x => x.KeyCodeId == (int)KeyCode.LeftAlt).Id);
                    player.Skills.Use<DoubleJump>();
                }
            }
        });
    }
}
