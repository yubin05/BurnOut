using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntryBossRoom : MonoBehaviour
{
    GameObject player;
    GameObject boss;
    GameObject entryBossRoomDoor;

    Tilemap tilemap;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");
        entryBossRoomDoor = GameObject.FindWithTag("EntryBossRoomDoor");

        tilemap = entryBossRoomDoor.GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // player stop wait a minute
            player.GetComponent<PlayerController>().canInput = false;
            //StartCoroutine(CanMovePlayer());

            // close door in boss room
            entryBossRoomDoor.SetActive(true);
            StartCoroutine(AppearDoor());
        }
    }

    //IEnumerator CanMovePlayer()
    //{
    //    yield return new WaitForSeconds(1f);

    //    // player can move
    //    player.GetComponent<PlayerController>().canInput = true;

    //    // prevent iteration
    //    Destroy(gameObject);
    //}

    IEnumerator AppearDoor()
    {
        for (int i=0; i<=255; i+=5)
        {
            tilemap.color = new Color(1, 1, 1, i / 255f);
            yield return new WaitForFixedUpdate();
        }

        // boss entry
        boss.SetActive(true);

        CanMovePlayer();
    }

    void CanMovePlayer()
    {        
        // player can move
        player.GetComponent<PlayerController>().canInput = true;

        // prevent iteration
        Destroy(gameObject);
    }
}
