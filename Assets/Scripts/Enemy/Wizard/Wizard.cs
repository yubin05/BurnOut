using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    SpriteRenderer[] renders;

    private bool canTeleport = true;
    public float teleportCoolTime;

    private float scaleX;
    private bool isDead = false;

    GameObject canvas;
    private float canvasScaleX;

    public float fireball_speed;

    Wizard_FireBall_Controller wizard_fireBall_controller;

    [SerializeField]
    public GameObject inital_teleport_point;

    public float attackCoolTime;
    [HideInInspector]
    public bool canAttack = false;  // controled by Wizard_DetectPlayer.cs
    private bool isAttackCoolTime = false;

    protected override void Awake()
    {
        // Wizard's each body's part get
        renders = transform.Find("body").GetComponentsInChildren<SpriteRenderer>();

        wizard_fireBall_controller = gameObject.GetComponentInChildren<Wizard_FireBall_Controller>();

        Instantiate(inital_teleport_point, new Vector2(transform.position.x, transform.position.y + 0.25f), transform.rotation,
            transform.parent.Find("Teleport_Points"));

        scaleX = transform.localScale.x;

        canvas = transform.Find("Canvas").gameObject;
        canvasScaleX = canvas.transform.localScale.x;

        base.Awake();
        isRightDefaultValue = false;
    }

    protected override void Start()
    {
        base.Start();

        // get audio manager
        audioManager = gameObject.GetComponentInChildren<AudioManager_Wizard>();
    }

    protected override void Anim_Control()
    {
        animator.SetBool("canAttack", canAttack);
    }

    // this method called by Wizard_DetectTeleportPoint.cs
    public void Teleport(List<Transform> teleport_points)
    {
        if (isDead) { return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) { return; }   // while attacking, can't teleport
        if (!canTeleport || teleport_points.Count <= 1) { return; }     // if telepoint_point don't exist, return function

        // teleport cooltime
        canTeleport = false;
        StartCoroutine(CoolTimeTeleport());

        int random_index = Random.Range(0, teleport_points.Count);
        if (teleport_points[random_index].position.x == transform.position.x
            && teleport_points[random_index].position.y == transform.position.y + 0.25f) { return; }

        // teleport signal
        foreach (SpriteRenderer render in renders) { render.color = Color.black; }
        gameObject.layer = 9;   // enemydead

        // teleport sound
        audioManager.PlayAudio("Teleport", 0.5f);

        // actually teleport method
        StartCoroutine(MovePosition(teleport_points, random_index));
    }

    IEnumerator CoolTimeTeleport()
    {
        yield return new WaitForSeconds(teleportCoolTime);
        canTeleport = true;
    }

    IEnumerator MovePosition(List<Transform> teleport_points, int random_index)
    {
        yield return new WaitForSeconds(1f);
        transform.position = new Vector2(teleport_points[random_index].position.x, teleport_points[random_index].position.y - 0.25f);

        // Finish Teleport Signal
        StartCoroutine(FinishTeleportSignal());
    }

    IEnumerator FinishTeleportSignal()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (SpriteRenderer render in renders) { render.color = Color.white; }
        gameObject.layer = 8;   // enemydead
    }

    public override void Move()
    {
        // don't move left and right

        // prevent collider bug
        transform.position = new Vector2(transform.position.x - 0.01f, transform.position.y);
        transform.position = new Vector2(transform.position.x + 0.01f, transform.position.y);

        // look at player
        if (transform.position.x - player.transform.position.x < 0)
        {
            transform.localScale = new Vector3(
                scaleX * (-1), transform.localScale.y, transform.localScale.z);

            // prevent health bar flipped horizontal
            canvas.transform.localScale = new Vector3(
                canvasScaleX * (-1), canvas.transform.localScale.y, canvas.transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(
                scaleX, transform.localScale.y, transform.localScale.z);

            // prevent health bar flipped horizontal
            canvas.transform.localScale = new Vector3(
                canvasScaleX, canvas.transform.localScale.y, canvas.transform.localScale.z);
        }
    }

    protected override void DestroyEnemyObject()
    {
        isDead = true;

        // explosion signal
        foreach (SpriteRenderer render in renders) { render.color = new Color(1, 1, 1, 0.4f); }
        
        // dead sound play once
        if (!playedDeadSound)
        {
            playedDeadSound = true;
            StartCoroutine(PlayAudio_Explosion());
        }

        StartCoroutine(StartExplosion());
        StartCoroutine(DelayDestroy());
    }

    IEnumerator PlayAudio_Explosion()
    {
        yield return new WaitForSeconds(0.3f);
        audioManager.PlayAudio("Explosion", 0.5f); // explosion sound
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        transform.Find("Explosion").gameObject.SetActive(true);
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public override void Attack()
    {
        if (isDead) { return; }
        if (!canAttack || isAttackCoolTime) { return; }

        isAttackCoolTime = true;
        StartCoroutine(OnCoolTimeAttack());

        animator.SetTrigger("OnAttack");
        StartCoroutine(ShotFireBall());
    }

    IEnumerator OnCoolTimeAttack()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttackCoolTime = false;
    }

    IEnumerator ShotFireBall()
    {
        yield return new WaitForSeconds(0.3f);
        audioManager.PlayAudio("FireBall", 1f, 0.8f);
        wizard_fireBall_controller.Instantiate_Fireball(this);
    }

    public override void Hit(int player_attack_power)
    {
        base.Hit(player_attack_power);

        audioManager.PlayOneShotAudio("Hurt", 0.5f, 1.5f);
    }
}
