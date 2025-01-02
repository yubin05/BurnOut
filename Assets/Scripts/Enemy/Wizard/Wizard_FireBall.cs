using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_FireBall : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 dir;

    // player
    GameObject player;
    Transform target_transform;

    // wizard
    [HideInInspector] public Wizard wizard_script;

    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        player = GameObject.FindWithTag("Player");
        target_transform = player.transform;

        //wizard_script = transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<Wizard>();

        moveSpeed = wizard_script.fireball_speed;

        // direction vector
        dir = target_transform.position - transform.position;
        dir = dir.normalized;

        StartCoroutine(DestroyObject());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target_transform.position, 0.1f);
        rigid.velocity = dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && !player.GetComponent<PlayerController>().isDamaged)    // Player
        {
            player.GetComponent<PlayerController>().Hit(wizard_script.attackPower);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
