using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Floor") { collider.isTrigger = true; }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Floor") { collider.isTrigger = false; }
    }
}
