using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject
{
    [SerializeField] protected Transform floorCheckNode;
    public Transform FloorCheckNode => floorCheckNode;

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

    protected override void Update()
    {
        base.Update();
        
        // floor 체크
        var hitBoxs = Physics2D.OverlapBoxAll(FloorCheckNode.position, FloorCheckNode.localScale, 0f, LayerMask.GetMask(nameof(Floor)));
        if (hitBoxs.Length > 0)
        {
            // Debug.Log("Ignore Floor");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(nameof(Floor)), LayerMask.NameToLayer(nameof(Player)), true);
        }
        else
        {
            // Debug.Log("No Ignore Floor");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(nameof(Floor)), LayerMask.NameToLayer(nameof(Player)), false);
        }

        if (MotionHandler.IsHit || MotionHandler.IsDeath) return;
        
        if (Input.GetKeyDown(KeyCode.LeftControl))  // 공격
        {
            OnAttack();
        }
        else if (!MotionHandler.IsAttack && !MotionHandler.IsJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 점프
        {
            OnJump();
        }
        else if (!MotionHandler.IsAttack && Input.GetKey(KeyCode.RightArrow))   // 오른쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Right);
        }
        else if (!MotionHandler.IsAttack && Input.GetKey(KeyCode.LeftArrow))   // 왼쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Left);
        }        
        else if (!MotionHandler.IsJump && !MotionHandler.IsAttack)
        {
            OnIdle();
        }
    }
}
