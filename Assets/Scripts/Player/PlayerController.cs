using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody2D rigid;
    SpriteRenderer render;
    [HideInInspector] public Animator animator;
    BoxCollider2D colliderOfCollision;

    // GameObject crouch clone
    GameObject crouch_clone;
    BoxCollider2D crouch_collider;

    // Audio Manager
    GameObject audio_;
    AudioManager audioManager;

    private bool beingCombo;
    private float runSpeed;
    private float moveSpeed;
    private float rating;
    private bool isCrouch;
    private float collider_offset_x, collider_offset_y;
    private float collider_size_x, collider_size_y;
    private bool isAttacking = false;
    private float time_after_attack;
    private int combo_count = 0;
    private bool soundPlayed = false;
    public bool isDamaged = false;
    private bool playedDeadSound = false; // play dead audioclip only once
    private bool canFall_animation = true;

    public int health;
    public int max_health;  // auto setting
    public float walkSpeed;
    public float jumpPower;
    //public float floorColiderSubtract;
    public float combo_delay;
    public float playerDamagedTime;
    float min_velocity_y = -30f;            // 낙하속도 제한 등의 사용
    Vector2 playerVelocity;

    public int player_sword_attack_power;   // MouseButtonDown(0)
    public int player_throw_attack_power;   // MouseButtonDown(1)
    public float dash_power;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool bInputJumpKey = false;
    [HideInInspector] public bool canInput = true;
    [HideInInspector] public bool canThrow = true;

    // hitbox
    static GameObject hitbox;   // Player's attack collider
    float hitTimer; float hitDelay = 5f;     // isDamaged=false 안되는 버그 방지를 위한 타이머와 딜레이

    // Throw
    static GameObject throwObject;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // set player health and max_health
        //health = 50;
        //max_health = health;

        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // original collider size and offset save
        colliderOfCollision = GetComponent<BoxCollider2D>();
        collider_offset_x = colliderOfCollision.offset.x; collider_offset_y = colliderOfCollision.offset.y;
        collider_size_x = colliderOfCollision.size.x; collider_size_y = colliderOfCollision.size.y;

        animator.fireEvents = false;    // prevent animation no reciever exception
        isGrounded = false;
        isCrouch = false;
        beingCombo = false;
        runSpeed = walkSpeed * 2;

        // crouch clone object
        crouch_clone = GetComponentInChildren<CrouchCloneController>().gameObject;
        crouch_collider = crouch_clone.GetComponent<BoxCollider2D>();
        crouch_clone.SetActive(false);

        // hit box default value <- false(InActive)
        hitbox = GetComponentInChildren<HitBoxController>().gameObject;
        hitbox.SetActive(false);

        // Throw
        throwObject = GetComponentInChildren<ThrowObjectController>().gameObject;

        
    }

    private void OnEnable()
    {
        isDamaged = false;
    }

    public void InitOnStart()
    {
        // get audio manager
        audio_ = GameObject.Find("AudioManager_Player");
        audioManager = audio_.GetComponent<AudioManager_Player>();

        // player status initalization
        player_sword_attack_power = 10;
        player_throw_attack_power = 5;
        health = 50;
        if (max_health < health) { max_health = health; }   // health control

        hitTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Anim_Control();

        // 낙하 속도 제한
        if (rigid.velocity.y < min_velocity_y)
        {
            playerVelocity = rigid.velocity;
            playerVelocity.y = min_velocity_y;  // 제한치 이상 넘어가지 않도록 조절

            // 플레이어 속도 적용
            rigid.velocity = playerVelocity;
        }

        // player can input whether input state
        if (canInput)
        {
            SwordAttack();
            ThrowAttack();
            Crouch();
            Move();
            Jump();
        }

        // player dead
        if (health <= 0) { Dead(); }

        //float temp_offset_y = 1f;
        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - temp_offset_y), Vector2.down, Color.green);
        //RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - temp_offset_y), Vector2.down, LayerMask.GetMask("Platform"));
        //if (hit.collider != null) { Debug.Log("����"); }
    }

    private void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (isDamaged && hitTimer > hitDelay)
        {
            hitTimer = 0f;
            isDamaged = false;
        }
            
    }

    // update animator variable like executing animation every time
    public void Anim_Control()
    {
        // move animation
        animator.SetFloat("velocity_x_abs", Mathf.Abs(rigid.velocity.x));

        // while jumping animation
        animator.SetBool("jumping", !isGrounded);
        animator.SetFloat("velocity_y", rigid.velocity.y);

        // attack animation
        animator.SetBool("isAttacking", isAttacking);
        animator.SetInteger("attack_count", combo_count);

        // crouch animation
        animator.SetBool("isCrouch", isCrouch);

        // hit animation
        animator.SetBool("isDamaged", isDamaged);

        // throw animation
        animator.SetBool("isThrowing", !canThrow);

        // fall animation
        animator.SetBool("canFall", canFall_animation);
    }

    // MouseButtonDown(0)
    void SwordAttack()
    {
        // combo timer
        if (beingCombo)
        {
            // combo delay check timer
            time_after_attack += Time.deltaTime;
            
            if (time_after_attack > combo_delay)
            {
                IsNotCombo();
                time_after_attack = 0f;
                return;
            }
        }
        else
        {
            time_after_attack = 0f;
        }

        if (!isCrouch && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack_trigger");
            isAttacking = true;

            if (combo_count > 2) { return; }   // finish combo
            AttackAndCombo();
            time_after_attack = 0;
        }
    }

    void AttackAndCombo()
    {
        isCrouch = false;   // prevent when player attack during GetKey(KeyCode.S)
        beingCombo = true;
        combo_count += 1;

        // On hitBox and Play Audio
        OnHitBox2D();
        audioManager.PlayOneShotAudio("Attack", 0.85f);
    }

    // when player's attack combo stop
    void IsNotCombo()
    {
        animator.SetTrigger("stopCombo");

        isAttacking = false;
        beingCombo = false;
        combo_count = 0;
    }

    void OnHitBox2D()
    {
        hitbox.SetActive(true);

        // delay
        StartCoroutine(OffHitBox2D());
    }

    IEnumerator OffHitBox2D()
    {
        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
    }

    // MouseButtonDown(1)
    private void ThrowAttack()
    {
        if (canThrow && !isCrouch && Input.GetMouseButtonDown(1))
        {
            canFall_animation = false;
            animator.SetTrigger("throw_trigger");
            throwObject.GetComponent<ThrowObjectController>().InstantiateClone(render.flipX);

            // throw delay
            canThrow = false;
            StartCoroutine(ThrowCoolingTime());
        }

        // can fall animation
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            canFall_animation = true;
        }
    }

    IEnumerator ThrowCoolingTime()
    {
        yield return new WaitForSeconds(
            GetComponentInChildren<ThrowObjectController>().throw_delay
        );
        canThrow = true;
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Running
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
        // left, right move
        if (!isAttacking && Input.GetKey(KeyCode.A))
        {
            isCrouch = false;
            rigid.velocity = new Vector2(-1 * moveSpeed, rigid.velocity.y);
            PlayMoveSound();
        }
        if (!isAttacking && Input.GetKey(KeyCode.D))
        {
            isCrouch = false;
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
            PlayMoveSound();
        }

        // FilpX
        if (rigid.velocity.x <= -walkSpeed)
        {
            render.flipX = true;
        }
        else if (rigid.velocity.x >= walkSpeed)
        {
            render.flipX = false;
        }
    }

    // control audio while only moving
    void PlayMoveSound()
    {
        if (!soundPlayed)
        {
            if (moveSpeed > walkSpeed)
            {
                audioManager.PlayAudio("Run", 0.5f);
            }
            else
            {
                audioManager.PlayAudio("Walk", 0.5f);
            }
            soundPlayed = true;
            StartCoroutine(offSoundPlayed());
        }
    }

    IEnumerator offSoundPlayed()
    {
        yield return new WaitForSeconds(0.5f);
        soundPlayed = false;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            // downJump
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit2D hit_down = Physics2D.Raycast(transform.position, Vector3.down, 2, LayerMask.GetMask("Floor"));
                
                if (hit_down.collider != null)   // prevent null exception
                {
                    hit_down.collider.isTrigger = true;
                    isGrounded = false;
                    audioManager.PlayAudio("Jump");
                }
            }
            // jump
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                CancelInvoke("AfterJump");
                bInputJumpKey = true;
                Invoke("AfterJump", 0.5f);

                // rating
                if (moveSpeed > walkSpeed) { rating = 1.2f; }
                else { rating = 1f; }
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * rating);

                isGrounded = false;
                animator.SetTrigger("jump_trigger");    // jump trigger animation
                animator.Play("Jump"); audioManager.PlayAudio("Jump");
            }
        }
    }
    void AfterJump()
    {
        bInputJumpKey = false;
    }

    public void Crouch()
    {
        // check isCrouch variable
        if (isCrouch)
        {
            // reduce colider size
            colliderOfCollision.size = new Vector2(crouch_collider.size.x, crouch_collider.size.y);
            colliderOfCollision.offset = new Vector2(crouch_collider.offset.x, crouch_collider.offset.y);
        }
        else
        {
            // revert collider size
            colliderOfCollision.offset = new Vector2(collider_offset_x, collider_offset_y);
            colliderOfCollision.size = new Vector2(collider_size_x, collider_size_y);
        }

        if (Input.GetKey(KeyCode.S) && isGrounded) { isCrouch = true; }
        else { isCrouch = false; }
    }

    // when player dead
    // you can use this Dead() method directly by others
    public virtual void Dead()
    {
        gameManager.isGameOver = true;

        canInput = false;   // player can not move while dying
        isDamaged = true;

        // when other script execute this method directly
        health = 0;
        gameObject.GetComponentInChildren<PlayerHealthUI>().setHealthUI(0); // set health bar UI

        animator.SetTrigger("OnDead");

        Invincible();

        if (!playedDeadSound)
        {
            audioManager.PlayOneShotAudio("Death", 1f, 1f);    // Enemy Dead Sound
            playedDeadSound = true;
        }

        // if animation finish, this object inactive
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            playedDeadSound = false;
            gameManager.GameOver();
        }
    }

    // following method controled by other script
    public void Hit(Transform otherTransform, int hitDamage)
    {
        // Player hit by enemy
        {
            hitTimer = 0f;
            Hit(hitDamage);

            int reDircX = (transform.position.x - otherTransform.position.x > 0) ? 1 : -1;
            int reDircY = (transform.position.y - otherTransform.position.y > 1) ? 1 : 2;
            Vector2 reActDirc = new Vector2(reDircX, reDircY).normalized;
            rigid.AddForce(reActDirc * 10, ForceMode2D.Impulse);
        }
    }

    // not physical reaction version
    public void Hit(int hitDamage)
    {
        canInput = false;    // player can't input key
        health -= hitDamage;    // health decreased

        // player's hit effect
        OnDamaged();
    }

    // player's hit effect
    void OnDamaged()
    {
        isDamaged = true;
        // start hurt animation
        animator.SetTrigger("OnDamaged");

        gameObject.GetComponentInChildren<PlayerHealthUI>().setHealthUI(health); // set health bar UI

        Invincible();

        // Player's OnDamage Sound
        audioManager.PlayOneShotAudio("Hurt");

        StartCoroutine(OffHurtAnimation());           // set off animtaion time
        StartCoroutine(OffDamaged());    // set off playerdamaged time 
    }

    IEnumerator OffHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        // player can input key
        canInput = true;
        // finish hurt animation
        isDamaged = false;
    }

    // finish player's no hit time
    IEnumerator OffDamaged()
    {
        yield return new WaitForSeconds(playerDamagedTime);

        Vincible();
    }

    // isGrounded check true and animation excute
    // this method called by other scripts
    public void Landing()
    {
        animator.SetTrigger("landing_trigger");
        isGrounded = true;
        audioManager.PlayOneShotAudio("Landing");
    }

    public void Invincible()
    {
        gameObject.layer = 7;   // PlayerDamaged
        render.color = new Color(1, 1, 1, 0.4f);
    }

    public void Vincible()
    {
        gameObject.layer = 6;   // Playerd  
        render.color = new Color(1, 1, 1, 1);
    }
}