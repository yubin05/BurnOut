using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        var player = data as Player;
        player.Init();

        MotionHandler.JumpEvent += () => 
        {
            Rigidbody2D.AddForce(transform.up * player.BasicStat.JumpPower, ForceMode2D.Impulse);
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))  // 공격
        {
            FSM.ChangeState(new AttackState(this));
        }
        else if (!MotionHandler.IsJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 점프
        {
            FSM.ChangeState(new JumpState(this));            
        }
        else if (Input.GetKey(KeyCode.RightArrow))   // 오른쪽으로 이동
        {
            SpriteRenderer.flipX = false;
            if (!MotionHandler.IsJump) FSM.ChangeState(new WalkState(this));

            var player = data as Player;
            transform.Translate(new Vector3(+(player.BasicStat.MoveSpeed*Time.deltaTime), 0, 0));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))   // 왼쪽으로 이동
        {
            SpriteRenderer.flipX = true;
            if (!MotionHandler.IsJump) FSM.ChangeState(new WalkState(this));

            var player = data as Player;
            transform.Translate(new Vector3(-(player.BasicStat.MoveSpeed*Time.deltaTime), 0, 0));
        }        
        else if (!MotionHandler.IsJump && !MotionHandler.IsAttack)
        {
            FSM.ChangeState(new IdleState(this));
        }
    }
}
