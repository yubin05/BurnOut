using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryDialogueCollider : MonoBehaviour
{
    GameObject player;
    PlayerController player_script;

    GameObject dialogue_canvas;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<PlayerController>();

        dialogue_canvas = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        dialogue_canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") 
        {
            // Player don't hit by enemy and can't move
            player.layer = 7; player_script.animator.Play("Idle");
            player_script.canInput = false;

            dialogue_canvas.SetActive(true);
        }
    }

    // this method called by TalkCursor.cs
    public void DestroyDialogueCollider()
    {
        // Player can hit by enemy and can move
        player.layer = 6; player_script.canInput = true;

        Destroy(gameObject);
    }
}
