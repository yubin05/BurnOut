using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, ICollider2D
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        var playerObj = other.GetComponent<PlayerObject>();
        if (playerObj != null && !playerObj.ImmunitySystem.IsImmunity && !playerObj.MotionHandler.IsDeath)
        {
            playerObj.OnDeath();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
    }
}
