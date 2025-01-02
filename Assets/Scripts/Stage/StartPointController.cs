using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointController : MonoBehaviour
{
    GameObject player;
    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.position = gameObject.transform.position;

        render = gameObject.GetComponent<SpriteRenderer>();
        render.color = new Color(1, 1, 1, 0f);
    }
}
