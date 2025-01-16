using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.MotionHandler.StartDeath();
        // Death 상태에 진입하면 모든 모션 다 End
        characterObject.MotionHandler.EndMove();
        characterObject.MotionHandler.EndAttack();
        characterObject.MotionHandler.EndJump();
        characterObject.MotionHandler.EndHit();
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
    }
}
