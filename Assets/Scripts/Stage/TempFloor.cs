using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TempFloor : MonoBehaviour
{
    private IEnumerator fallFloorCoroutine; // to stop the coroutine
    private Tilemap tilemap;

    private bool isFalling = false;

    private void Start()
    {
        fallFloorCoroutine = FallFloorTrigger();
        tilemap = GetComponent<Tilemap>();
    }

    // Player land floor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" &&
            collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            StartFallFloor();
        }
    }

    // Player escape floor without downjump
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" &&
            collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            StopFallFloor();
        }
    }
    // Player escape floor with downjump
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" &&
            collider.GetType() == typeof(CapsuleCollider2D))
        {
            StopFallFloor();
        }
    }

    void StartFallFloor()
    {
        tilemap.color = new Color(1, 45 / 255f, 45 / 255f, 1);  // red
        StartCoroutine(fallFloorCoroutine);
    }
    void StopFallFloor()
    {
        if (!isFalling) { tilemap.color = new Color(1, 1, 1, 1); }  // recovery color
        StopCoroutine(fallFloorCoroutine);
        fallFloorCoroutine = FallFloorTrigger();   // coroutine initalize
    }

    IEnumerator FallFloorTrigger()
    {
        yield return new WaitForSeconds(1f);
        FallFloor();
    }

    void FallFloor()
    {
        isFalling = true;
        gameObject.layer = 15;  // falling floor

        // fall floor actually
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezePositionX |
            RigidbodyConstraints2D.FreezeRotation;
    }
}