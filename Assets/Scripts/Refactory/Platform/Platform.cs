using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected void OnCollisionEnter2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            characterObject.IsJump = false;
        }
    }
}
