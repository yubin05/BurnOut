using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().Dead();
        }

        // destroy falling floor when contact deadzone
        if (collider.gameObject.layer == 15)
        {
            Destroy(collider.gameObject);
        }
    }
}
