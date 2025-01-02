using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skill_Dash : MonoBehaviour
{
    // GameManager
    //GameManager gm;

    // Player
    GameObject player;
    PlayerController player_script;

    // Audio Manager
    GameObject audio_;
    AudioManager audioManager;

    Rigidbody2D rigid;
    SpriteRenderer render;
    Animator animator;

    private bool dashing = false;   // dash skill control
    private bool dashing_animation = false;   // animation control value

    public float dash_cooltime;
    // UI Controller
    CoolTimeUIController coolTimeController;

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // get player object and player script
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<PlayerController>();

        // get audio manager
        audio_ = transform.parent.Find("AudioManager_Player").gameObject;
        audioManager = audio_.GetComponent<AudioManager>();

        // get rigidbody2D
        rigid = player.GetComponent<Rigidbody2D>();
        // get spriterenderer
        render = player.GetComponent<SpriteRenderer>();
        // get animator
        animator = player.GetComponent<Animator>();

        coolTimeController = GameObject.FindWithTag("PlayerUI").transform.Find("CoolTimeUI_Dash").transform.Find("foreground_image").
            GetComponent<CoolTimeUIController>();
    }

    // Update is called once per frame
    void Update()
    {
        // dash animation
        animator.SetBool("Dashing", dashing_animation);

        if (!dashing) { Dash(); }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && player.layer != 7)
        {
            dashing = true;
            // Skill UI trigger
            StartCoroutine(coolTimeController.OnCoolTime(dash_cooltime));

            dashing_animation = true;
            animator.SetTrigger("Dash_trigger");

            player_script.canInput = false;
            audioManager.PlayOneShotAudio("Dash", 5f);

            // player is invicible
            player_script.Invincible();

            // dash some distance
            int direction; float dash_speed;
            if (render.flipX) { direction = -1; } else { direction = 1; }
            if (player_script.isGrounded) { dash_speed = player_script.dash_power; } else { dash_speed = player_script.dash_power * 2 / 3; }
            rigid.velocity = new Vector2(direction * dash_speed, rigid.velocity.y);

            // set on freeze position Y
            rigid.constraints =
                RigidbodyConstraints2D.FreezePositionY |
                RigidbodyConstraints2D.FreezeRotation;

            // finish dash
            StartCoroutine(OffDash());
        }
    }

    private IEnumerator OffDash()
    {
        float offDashTime = 0.5f;
        yield return new WaitForSeconds(offDashTime);
        dashing_animation = false; animator.Play("Idle", -1);

        rigid.velocity = new Vector2(0, 0); // initialize velocity
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;  // set off freeze position Y

        player_script.canInput = true;
        player_script.Vincible();

        StartCoroutine(OffSkillCoolTime(dash_cooltime-offDashTime));
    }
    private IEnumerator OffSkillCoolTime(float remain_cooltime)
    {
        yield return new WaitForSeconds(remain_cooltime);
        dashing = false;
    }
}
