using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rino : Boss
{
    //private int defaultMoveSpeed = 2;
    [SerializeField] public int walkSpeed;
    [SerializeField] public int runSpeed;

    private int defaultContactPower;

    protected override void Awake()
    {
        base.Awake();
        moveSpeed = walkSpeed;

        // attack damage control
        defaultContactPower = contactPower;
    }

    protected override void Start()
    {
        base.Start();

        // get audio manager
        audioManager = gameObject.GetComponentInChildren<AudioManager_Rino>();

        // deacive self
        gameObject.SetActive(false);    // onactive when entry bossroom by EntryBossRoom.cs
    }

    protected override void Anim_Control()
    {
        // move speed variable
        animator.SetInteger("moveSpeed", moveSpeed);
    }

    public override void Move()
    {
        base.Move();

        if (render.flipX) { moveDirection = 1; }
        else { moveDirection = -1; }
        rigid.velocity = new Vector2(moveDirection * moveSpeed, rigid.velocity.y);
    }

    // following two methods called by Boss_Rino_DetectPlayerCollider.cs
    public void Run()
    {
        moveSpeed = runSpeed;
        contactPower = attackPower;     // rino attack while running
        audioManager.PlayOneShotAudio("Run", 3f);
    }
    public void NotRun()
    {
        moveSpeed = walkSpeed;
        contactPower = defaultContactPower;
    }

    public override void Hit(int player_attack_power)
    {
        base.Hit(player_attack_power);

        // play hurt sound
        audioManager.PlayOneShotAudio("Hurt", 0.5f, 0.8f);
    }
    public override void Dead()
    {
        if (!playedDeadSound)
        {
            audioManager.PlayOneShotAudio("Death", 1f, 0.6f);    // Enemy Dead Sound
            playedDeadSound = true;
        }
        base.Dead();
    }
    protected override void DeadAnimationTrigger()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            StartCoroutine(DestroyBoss(0.9f));
        }
    }
}
