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
                    var hits = Physics2D.RaycastAll(transform.position, new Vector2((int)enemy.MoveDirectionX, 0), 1f, LayerMask.GetMask(nameof(Floor)));
                    // 앞에 floor가 있으면 점프
                    if (hits.Length > 0 && !MotionHandler.IsJump)
                    {
                        OnJump();
                    }
                    // 앞에 floor가 없으면 이동
                    else
                    {
                        OnMove(enemy.MoveDirectionX);
                    }
                }
            }
        }
    }

    public override void OnHit(int attackPower)
    {
        base.OnHit(attackPower);

        // 헤드바가 존재하지 않는 경우 오브젝트 생성
        if (HeadBarObject == null)
        {
            HeadBarObject = GameApplication.Instance.GameController.HeadBarController.Spawn<HeadBar, HeadBarObject>(this, 50001);
        }
        // 헤드바가 존재하는 경우, 해당 헤드바 생명 주기 초기화하여 계속 유지
        else
        {
            var headBar = HeadBarObject.data as HeadBar;
            headBar.StartLifeTime(headBar.LifeTime);
        }
        HeadBarObject.UpdateHeadBar();  // 헤드바 정보 업데이트
    }
}
