using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollision2D
{
    public void OnCollisionEnter2D(Collision2D other);
    public void OnCollisionExit2D(Collision2D other);
}

public interface ICollider2D
{
    public void OnTriggerEnter2D(Collider2D other);
    public void OnTriggerExit2D(Collider2D other);
}
