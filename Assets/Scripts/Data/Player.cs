using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        Skills.AddSkill(new DoubleJump(80001));
        Skills.AddSkill(new DarkMagic(80002));
    }
}
