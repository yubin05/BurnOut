using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Bandit : Boss
{
    // hitbox
    static GameObject hitbox_bandit_left_right;   // Bandit's left and right attack collider
    static GameObject hitbox_bandit_down;   // Bandit's down attack collider

    bool attacking_animation = false;      // attack motion
    bool attack_triggered = false;      // variable for prevent keeping hitbox

    bool dash_attack_triggered = false; // variable for tie dash_attack_percent and prevent keeping dash_attack() method

    bool jump_animation = false;
    bool jump_attack_triggered = false; // prevent keeping jump_attack() method

    private int dash_attack_percentage = 15;
    private int jump_attack_percentage = 100;

    private int jump_power = 500;

    private bool down_attacking = false;

    private bool dead_sound_triggered = false;
    private bool walk_sound_triggered = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2;

        // get audio manager
        audio_ = GameObject.Find("AudioManager_Bandit");
        audioManager = audio_.GetComponent<AudioManager_Bandit>();

        // hit box default value <- false(InActive)
        hitbox_bandit_left_right = GetComponentInChildren<HitBoxControllor_Bandit>().gameObject;
        hitbox_bandit_left_right.SetActive(false);

        hitbox_bandit_down = GetComponentInChildren<HitBoxControllor_Bandit_DownAttack>().gameObject;
        hitbox_bandit_down.SetActive(false);

        // deacive self
        gameObject.SetActive(false);    // onactive when entry bossroom by EntryBossRoom.cs
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Debug.Log($"attack_triggered: {attack_triggered}, dash_attack_triggered: {dash_attack_triggered}, jump_attack_triggered: {jump_attack_triggered}");
        //Debug.Log($"jump_attack_triggered: {jump_attack_triggered}, walk_sound_triggered: {walk_sound_triggered}");

        // Player detecting
        if (rightThanPlayerX)
        {
            render.flipX = false;
        }
        else
        {
            render.flipX = true;
        }

        if (!isDead) { AttackPattern(); }
    }

    public override void Move()
    {
        base.Move();

        if (rightThanPlayerX) { moveDirection = -1; }   // left move because player exist left than bandit
        else { moveDirection = 1; }                     // right move because player exist light than bandit

        if (moveSwitch)
        {
            // when player close bandit, bandit move to player
            if (distanceAbsXDifferenceOfPlayer <= 8f && !isStaggered_velocity)
            {
                rigid.velocity = new Vector2(moveDirection * moveSpeed, rigid.velocity.y);
            }
        }
    }

    protected override void Anim_Control()
    {
        base.Anim_Control();

        animator.SetBool("isAttacking", attacking_animation);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            // attack sound time control
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.60f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.61f)
            {
                audioManager.PlayOneShotAudio("Attack", 0.8f, 0.7f);
            }
            OnHitBox2D_left_right();
        }

        animator.SetBool("isJumpAttacking", jump_animation);
        // when bandit jump high, trigger down attack
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            distanceAbsXDifferenceOfPlayer < 2f)
        {
            down_attacking = true;
            animator.SetTrigger("down_attack_trigger"); isStaggered_position = true;
        }
        animator.SetBool("down_attacking", down_attacking);
        // when bandit down attack, positon constraints set off
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DownAttack"))
        {
            StartCoroutine(DownAttack());

            // down attack sound time control
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.40f &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.41f)
            {
                audioManager.PlayOneShotAudio("Attack", 0.8f, 0.7f);
            }
        }

        // walk sound control
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            if (!walk_sound_triggered)
            {
                walk_sound_triggered = true;
                audioManager.PlayAudio("Walk", 1f, 1f);
                StartCoroutine(WalkSoundOff());
            }
        }
    }

    private void AttackPattern()
    {
        int temp = Random.Range(1, 10001);   // all attack trigger percentage

        // when boss close player, trigger attacked
        if (distanceAbsXDifferenceOfPlayer <= 2f)
        {
            if (!attack_triggered)
            {
                SetOnTriggers();
                Attack();
            }
        }
        // when player out hitbox range, set off attacking
        else if (distanceAbsXDifferenceOfPlayer > 2f)
        {
            isStaggered_velocity = false;
            attacking_animation = false;
            SetOffTriggers();

            // bandit sometimes trigger dash_attack
            if (distanceAbsXDifferenceOfPlayer > 8f)
            {
                if (!dash_attack_triggered && (temp) <= (dash_attack_percentage))
                {
                    SetOnTriggers();
                    Dash_Attack();
                }
            }
            // bandit sometimes trigger jump_attack
            else if (distanceAbsXDifferenceOfPlayer > 4f)
            {
                if (!jump_attack_triggered && (temp) <= (jump_attack_percentage))
                {
                    SetOnTriggers();
                    Jump();
                }
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        isStaggered_velocity = true; // stop during attacking player
        attacking_animation = true; animator.SetTrigger("attack_trigger");
    }

    void OnHitBox2D_left_right()
    {
        hitbox_bandit_left_right.SetActive(true);
        StartCoroutine(OffHitBox2D_left_right());
    }

    public IEnumerator OffHitBox2D_left_right()
    {
        yield return new WaitForSeconds(0.1f);
        hitbox_bandit_left_right.SetActive(false);

        attacking_animation = false;
        StartCoroutine(AfterAttackDelay());
    }

    IEnumerator AfterAttackDelay()
    {
        yield return new WaitForSeconds(1f);
        SetOffTriggers(); isStaggered_velocity = false;
        walk_sound_triggered = false;
    }

    private void Dash_Attack()
    {
        // prevent attack pattern && move
        moveSwitch = false;

        // teleport
        render.color = new Color(1, 1, 1, 0.5f);    // effect on
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);

        float position_offset_x = 1.5f;
        if (render.flipX) { position_offset_x *= -1f; }
        transform.position = new Vector2(player.transform.position.x + position_offset_x, transform.position.y);

        render.color = new Color(1, 1, 1, 1);   // effect off

        // attacking
        attack_triggered = false;

        // after a while other constraints set off
        StartCoroutine(AfterTeleportSetOffConstrarints());
    }

    IEnumerator AfterTeleportSetOffConstrarints()
    {
        yield return new WaitForSeconds(1f);

        dash_attack_triggered = false; jump_attack_triggered = false; moveSwitch = true;
    }

    private void Jump()
    {
        walk_sound_triggered = true;
        jump_animation = true; animator.SetTrigger("jump_trigger"); moveSwitch = false;
        
        rigid.AddForce(new Vector2(moveDirection * (distanceAbsXDifferenceOfPlayer) * 100, jump_power));
        audioManager.PlayOneShotAudio("Jump", 0.3f);
        StartCoroutine(AfterJumpSetOffConstrarints());
    }

    // this method execute continous by anim_control method not once
    IEnumerator DownAttack()
    {
        yield return new WaitForSeconds(0.3f);
        isStaggered_position = false;

        //rigid.velocity = Vector2.down * 100f;
        rigid.AddForce(Vector2.down * 100f);

        OnHitBox2D_down();
        StartCoroutine(AfterDownAttackSetOffConstrarints());
    }

    void OnHitBox2D_down()
    {
        hitbox_bandit_down.SetActive(true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DownAttack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            StartCoroutine(OffHitBox2D_down());
        }
    }

    public IEnumerator OffHitBox2D_down()
    {
        yield return new WaitForSeconds(0.5f);
        hitbox_bandit_down.SetActive(false);

        attacking_animation = false;
        StartCoroutine(AfterAttackDelay());
    }

    IEnumerator AfterJumpSetOffConstrarints()
    {
        yield return new WaitForSeconds(1f);        
        jump_animation = false; moveSwitch = true; 
    }

    IEnumerator AfterDownAttackSetOffConstrarints()
    {
        yield return new WaitForSeconds(0.5f);
        SetOffTriggers();
    }

    // setting all trigger related attack because prevent other attack pattern while one attack pattern
    void SetOnTriggers()
    {
        attack_triggered = true; dash_attack_triggered = true; jump_attack_triggered = true;
    }
    void SetOffTriggers()
    {
        attack_triggered = false; dash_attack_triggered = false; jump_attack_triggered = false;
    }

    // only sound implement
    public override void Hit(int player_attack_power)
    {
        audioManager.PlayOneShotAudio("Hurt", 0.8f);
        base.Hit(player_attack_power);
    }
    public override void Dead()
    {
        if (!dead_sound_triggered)
        {
            audioManager.PlayAudio("Death", 1f, 0.5f);
            dead_sound_triggered = true;
        }
        base.Dead();
    }
    protected override void DeadAnimationTrigger()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            animator.Play("Death", -1, 0.99999f);
            StartCoroutine(DestroyBoss(2f));
        }
    }

    IEnumerator WalkSoundOff()
    {
        yield return new WaitForSeconds(0.5f);
        walk_sound_triggered = false;
    }
}
