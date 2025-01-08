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

        transform.ChangeLayerRecursively("Enemy");
    }

    protected virtual void Update()
    {
        // 피격 중일땐 피격 모션이 나오면서 정지해야 함
        if (!MotionHandler.IsHit)
        {
            var enemy = data as Enemy;
            OnMove(enemy.MoveDirectionX);
        }
    }
}
