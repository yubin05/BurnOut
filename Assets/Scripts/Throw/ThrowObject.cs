using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    // this ThrowObject
    Rigidbody2D rigid;

    // Player
    GameObject player;
    SpriteRenderer player_render;
    PlayerController playerController;

    // Animator
    Animator animator;

    // Coliision Enemy
    Enemy enemy;
    Boss boss;

    private int directionX;
    private float moveSpeed;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player");
        player_render = player.GetComponent<SpriteRenderer>();
        playerController = player.GetComponent<PlayerController>();

        moveSpeed = player.GetComponentInChildren<ThrowObjectController>().moveSpeed;
    }

    private void Start()
    {
        if (player_render.flipX) { directionX = -1; }
        else { directionX = 1; }
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = new Vector2(directionX*moveSpeed, rigid.velocity.y);
        DestroyClone();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            StopMove();

            // Enemy Health Decreased
            enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hit(playerController.player_throw_attack_power);
        }
        else if (collision.transform.tag == "Boss")
        {
            StopMove();

            // Boss Health Decreased
            boss = collision.gameObject.GetComponent<Boss>();
            boss.Hit(playerController.player_throw_attack_power);
        }
        else if (collision.transform.tag == "Wall") 
        {
            StopMove();
        }
    }

    void StopMove()
    {      
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX;  // stop
        rigid.gravityScale = 0f;

        // ThrowObject hit animation
        animator.SetTrigger("hit_trigger");
    }

    void DestroyClone()
    {
        // Destroy Object
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            Destroy(gameObject);
        }
    }
}