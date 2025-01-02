using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.layer == 6)    // Player(layer)
        {
            collision.gameObject.GetComponent<PlayerController>().Dead();
        }
    }
}
