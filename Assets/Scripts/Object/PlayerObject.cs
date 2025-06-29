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

            // 공격 사운드
            GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10004, Vector3.zero, Quaternion.identity);
        };

        MotionHandler.HitEvent += () => 
        {
            GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10005, Vector3.zero, Quaternion.identity);
        };

        player.MoveDirectionX = Character.MoveDirectionXs.Right;

        var skillButton1 = SkillButton1.Instance; skillButton1?.Init(80002); // DarkMagic

        transform.ChangeLayerRecursively(nameof(Player));
    }

    protected virtual void FixedUpdate()
    {
        // floor 체크
        var hitBoxs = Physics2D.OverlapBoxAll(FloorCheckNode.position, FloorCheckNode.localScale, 0f, LayerMask.GetMask(nameof(Floor)));
        if (hitBoxs.Length > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(nameof(Floor)), LayerMask.NameToLayer(nameof(Player)), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(nameof(Floor)), LayerMask.NameToLayer(nameof(Player)), false);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (MotionHandler.IsHit || MotionHandler.IsDeath) return;

        var player = data as Player;
        var joystick = JoyStick.Instance;

        // AttackButton.cs로 이동
        /*if (Input.GetKeyDown(KeyCode.LeftControl))  // 공격
        {
            OnAttack();
        }*/
        // SkillButton1.cs로 이동
        /*elseif (Input.GetKeyDown(KeyCode.Q))
        {
            // player.Skills.Use(80002);
            // player.Skills.Use<DarkMagic>();
            player.Skills.Use(GameApplication.Instance.GameModel.ClientData.PlayerKeyCodes.playerKeyCodes.Find(x => x.KeyCodeId == (int)KeyCode.Q).Id);
        }*/
        // JumpButton.cs로 이동
        /*else if (!MotionHandler.IsAttack && !MotionHandler.IsJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 점프
        {
            OnJump();
        }*/
        /*else if (!MotionHandler.IsAttack && MotionHandler.IsJump && !player.IsDoubleJump && Input.GetKeyDown(KeyCode.LeftAlt))  // 더블 점프
        {
            // player.Skills.Use(80001);
            // player.Skills.Use<DoubleJump>();
            player.Skills.Use(GameApplication.Instance.GameModel.ClientData.PlayerKeyCodes.playerKeyCodes.Find(x => x.KeyCodeId == (int)KeyCode.LeftAlt).Id);
        }*/
        /*else */if (!MotionHandler.IsAttack && !player.IsDoubleJump && joystick && joystick.JoystickAnchorPos.x > joystick.JoyStickMinRange/*Input.GetKey(KeyCode.RightArrow)*/)   // 오른쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Right);
        }
        else if (!MotionHandler.IsAttack && !player.IsDoubleJump && joystick && joystick.JoystickAnchorPos.x < -joystick.JoyStickMinRange/*Input.GetKey(KeyCode.LeftArrow)*/)   // 왼쪽으로 이동
        {
            OnMove(Character.MoveDirectionXs.Left);
        }
        else if (!MotionHandler.IsJump && !MotionHandler.IsAttack)
        {
            OnIdle();
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10006, Vector3.zero, Quaternion.identity);
    }
}
