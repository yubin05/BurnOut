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
    }

    private void Update()
    {
        if (!IsJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 점프
        {
            IsJump = true;
            FSM.ChangeState(new JumpState(this));

            var player = data as Player;
            Rigidbody2D.AddForce(transform.up * player.BasicStat.JumpPower, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.RightArrow))   // 오른쪽으로 이동
        {
            SpriteRenderer.flipX = false;
            if (!IsJump) FSM.ChangeState(new WalkState(this));

            var player = data as Player;
            transform.Translate(new Vector3(+(player.BasicStat.MoveSpeed/60f), 0, 0));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))   // 왼쪽으로 이동
        {
            SpriteRenderer.flipX = true;
            if (!IsJump) FSM.ChangeState(new WalkState(this));

            var player = data as Player;
            transform.Translate(new Vector3(-(player.BasicStat.MoveSpeed/60f), 0, 0));
        }        
        else if (!IsJump)
        {
            FSM.ChangeState(new IdleState(this));
        }
    }
}
