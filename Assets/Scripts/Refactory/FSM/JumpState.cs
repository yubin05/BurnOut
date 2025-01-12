using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.MotionHandler.StartJump();
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
    }
}
