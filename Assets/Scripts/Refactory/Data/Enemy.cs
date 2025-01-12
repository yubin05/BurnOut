using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public enum MoveTypes { Continuous, Tracking }
    public MoveTypes MoveType { get; set; }
    
    public int JumpRate { get; set; }

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);
    }
}
