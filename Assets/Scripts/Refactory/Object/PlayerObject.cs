using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        var player = data as Player;
        player.Init(this);

        MotionHandler.AttackEvent += () => 
        {
            var hitBoxs = Physics2D.OverlapBoxAll(AttackNode.position, AttackNode.localScale, 0f, player.AttackTargets);
            foreach (var hitBox in hitBoxs)
            {
                var enemyObj = hitBox.GetComponent<EnemyObject>();
                if (enemyObj != null && !enemyObj.ImmunitySystem.IsImmunity && !enemyObj.MotionHandler.IsDeath)
                {
                    enemyObj.OnHit(player.BasicStat.AttackDamage);
                }
            }
        };

        player.MoveDirectionX = Character.MoveDirectionXs.Right;

        transform.ChangeLayerRecursively(nameof(Player));
    }

    private void Update()
    {
        if (MotionHandler.IsHit || MotionHandler.IsDeath) return;
        
        if (Input.GetKeyDown(KeyCode.LeftControl))  // 공격
        {
            OnAttack();
        }
        else if (!MotionHandler.IsJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 점프
        {
            OnJump();
        }
        else if (Input.GetKey(KeyCode.RightArrow))   // 오른쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))   // 왼쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Left);
        }        
        else if (!MotionHandler.IsJump && !MotionHandler.IsAttack)
        {
            OnIdle();
        }
    }
}
