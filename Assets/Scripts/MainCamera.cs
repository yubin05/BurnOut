using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            gameObject.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y,
                gameObject.transform.position.z
            );
        }
        
    }
}
