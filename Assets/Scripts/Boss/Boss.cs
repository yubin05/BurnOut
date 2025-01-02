using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    GameManager gameManager;

    protected Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer render;
    protected Animator animator;

    // Player Object
    protected GameObject player;

    // AudioManager
    protected GameObject audio_;
    protected AudioManager audioManager;
    protected bool playedDeadSound = false; // play dead audioclip only once

    // move related variable
    protected float velocity_x_abs;

    public int health;
    public int max_health;  // auto setting

    public int attackPower;
    public int contactPower;

    // boss position.x compare player position.x
    protected bool rightThanPlayerX;    // initalize in sub class
    protected float distanceAbsXDifferenceOfPlayer;

    protected int moveSpeed;
    protected int moveDirection;
    protected bool moveSwitch = true;    //move when this switch on
    protected bool moveAnimationTriggered = true;

    // when hit by player, boss is stagger for a while
    protected bool isStaggered_velocity = false;    // boss velocity stop
    protected bool isStaggered_position = false;    // boss position constraints all stop

    // this boss whether die or not listen other script
    [HideInInspector] public bool isDead = false;

    protected virtual void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player");
        if (max_health < health) { max_health = health; }
    }

    protected virtual void Start()
    {
        render.flipX = false;
    }

    protected virtual void Update()
    {
        Anim_Control();

        // prevent collider bug with littttle move
        if (isStaggered_velocity) { rigid.velocity = new Vector2(moveDirection * moveSpeed * 0.01f, rigid.velocity.y); }

        if (isStaggered_position) {
            rigid.constraints =
                RigidbodyConstraints2D.FreezePositionX |
                RigidbodyConstraints2D.FreezePositionY |
                RigidbodyConstraints2D.FreezeRotation;
        }
        else { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }

        // Player detecting
        if (transform.position.x - player.transform.position.x >= 0)
        {
            rightThanPlayerX = true;
        }
        else
        {
            rightThanPlayerX = false;
        }
        distanceAbsXDifferenceOfPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
        
        // Boss move but dead
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) { Move(); }

        // Boss is dead
        if (health <= 0) { Dead(); }
    }

    // update animator variable like executing animation every time
    protected virtual void Anim_Control()
    {
        // move animation
        velocity_x_abs = Mathf.Abs(rigid.velocity.x);
        animator.SetFloat("velocity_x_abs", velocity_x_abs);
        animator.SetFloat("velocity_y", rigid.velocity.y);

        //// when hit by player, boss stop
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        //{
        //    isStaggered = true;
        //    StartCoroutine(AfterHitDelay());
        //}
    }

    //IEnumerator AfterHitDelay()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    isStaggered = false;
    //}

    // move route
    public virtual void Move()
    {
        /*// if hole exist front, boss turn
        RaycastHit2D hole_check = Physics2D.Raycast(new Vector2(rigid.position.x + (moveDirection / 2.0f), rigid.position.y),
                                                Vector3.down, 1f, LayerMask.GetMask("Floor", "Platform"));
        if (hole_check.collider == null)
        {
            render.flipX = !render.flipX;   // turn
        }*/
    }

    // when boss detect player and boss attack player
    public virtual void Attack()
    {
        // this method implement in subclass
    }

    // when boss contact player
    // following method controled by OnCollisionEnter2D of this script
    protected virtual void Contact(Collision2D collision, int contactPower)
    {
        collision.gameObject.GetComponent<PlayerController>().Hit(transform, contactPower);
    }

    // when boss dead
    // you can use this Dead() method directly by others
    public virtual void Dead()
    {
        isDead = true;

        animator.SetTrigger("OnDead");

        gameObject.layer = 12;   // Boss Dead

        // when other script execute this method directly
        health = 0;
        //gameObject.GetComponentInChildren<EnemyHealthUI>().setHealthUI(0); // set health bar UI

        rigid.velocity = Vector2.zero;  // stop

        // if animation finish, death animation is keeping
        DeadAnimationTrigger();
    }
    protected virtual void DeadAnimationTrigger()
    {
        // this method implement in subclass
    }

    protected virtual IEnumerator DestroyBoss(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameManager.ActiveStageClearPanelController(GameObject.FindWithTag("StageManager"));
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "EntryBossRoomDoor")
        // when boss contact wall or door
        {
            render.flipX = !render.flipX;   // turn
        }
        else if (collision.transform.tag == "Player" && !player.GetComponent<PlayerController>().isDamaged)
        // when boss contact player
        {
            Contact(collision, contactPower);
        }
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Platform")
        {
            // set off freeze position Y
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public virtual void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.tag == "Floor")
        {
            // set on freeze position Y
            rigid.constraints =
                RigidbodyConstraints2D.FreezePositionY |
                RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // when boss hit
    // following method controled by HitBoxController.cs
    public virtual void Hit(int player_attack_power)
    {
        animator.SetBool("isDamaged", true);
        animator.SetTrigger("OnDamaged");

        isStaggered_velocity = true;

        // delay animation
        StartCoroutine(OffHitAnimation());

        // health decreased
        health -= player_attack_power;
        gameObject.GetComponentInChildren<BossHealthUI>().setHealthUI(health); // set health bar UI
    }

    public virtual IEnumerator OffHitAnimation()
    {
        yield return new WaitForSeconds(1f);
        
        animator.SetBool("isDamaged", false);
        isStaggered_velocity = false;
    }
}
