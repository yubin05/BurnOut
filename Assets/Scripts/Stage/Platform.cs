using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected GameObject player;
    protected PlayerController player_script;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<PlayerController>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Player is Landing
        if (collision.gameObject.tag == "Player" && collision.collider == player.GetComponent<CapsuleCollider2D>())
        {
            //Debug.Log("Landing");
            player_script.Landing();
        }
            
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (!player_script.bInputJumpKey) return;

        if (collision.gameObject.tag == "Player" && collision.collider == player.GetComponent<CapsuleCollider2D>())
        {
            player_script.isGrounded = false;
        }
    }
}
