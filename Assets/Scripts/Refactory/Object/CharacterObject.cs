using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : EntityObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    [SerializeField] private Transform attackNode;
    public Transform AttackNode => attackNode;

    public MotionHandler MotionHandler { get; protected set; }
    
    public Rigidbody2D Rigidbody2D { get; protected set; }

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
    }

    public FSM FSM { get; protected set; }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init(this);

        FSM = new FSM(this);
        MotionHandler.Init();
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        FSM.ChangeState(new IdleState(this));
    }

    // 이동합니다.
    public virtual void OnMove(Character.MoveDirectionXs moveDirectionX)
    {
        if (!MotionHandler.IsJump)
            FSM.ChangeState(new MoveState(this));

        var character = data as Character;
        character.MoveDirectionX = moveDirectionX;
        transform.Translate(new Vector3((int)moveDirectionX*character.BasicStat.MoveSpeed*Time.deltaTime, 0, 0));
    }

    // 점프합니다.
    public virtual void OnJump()
    {
        FSM.ChangeState(new JumpState(this));

        var character = data as Character;
        Rigidbody2D.AddForce(transform.up * character.BasicStat.JumpPower, ForceMode2D.Impulse);
    }

    // 공격합니다.
    public virtual void OnAttack()
    {
        FSM.ChangeState(new AttackState(this));
    }

    // 피격 당합니다.
    public virtual void OnHit(int attackPower)
    {
        FSM.ChangeState(new HitState(this));

        var character = data as Character;
        character.CurrentHp = Mathf.Max(0, character.CurrentHp-attackPower);
    }
}
