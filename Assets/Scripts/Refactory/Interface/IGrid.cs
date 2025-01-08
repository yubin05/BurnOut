using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrid
{
    public void OnCollisionEnter2D(Collision2D other);
    public void OnCollisionExit2D(Collision2D other);
}
