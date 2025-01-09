using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CharacterObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        var enemy = data as Enemy;
        enemy.Init(this);

        enemy.MoveDirectionX = Character.MoveDirectionXs.Left;

        transform.ChangeLayerRecursively(nameof(Enemy));
    }

    protected virtual void Update()
    {
        if (MotionHandler.IsDeath) return;
        
        var enemy = data as Enemy;
        if (enemy.MoveType == Enemy.MoveTypes.Continuous)
        {
            // 피격 중일땐 피격 모션이 나오면서 정지해야 함
            if (!MotionHandler.IsHit)
            {
                var hitBoxs = Physics2D.OverlapBoxAll(AttackNode.position, AttackNode.localScale, 0f, enemy.AttackTargets);
                if (hitBoxs.Length > 0 && !MotionHandler.IsAttack)
                {
                    OnAttack();
                    foreach (var hitBox in hitBoxs)
                    {
                        var playerObj = hitBox.GetComponent<PlayerObject>();
                        if (playerObj != null && !playerObj.ImmunitySystem.IsImmunity)
                        {
                            playerObj.OnHit(enemy.BasicStat.AttackDamage);
                        }
                    }
                }
                else
                {
                    OnMove(enemy.MoveDirectionX);
                }
            }
        }        
    }
}
