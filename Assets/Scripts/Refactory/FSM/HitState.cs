using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : BaseState
{
    public HitState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.MotionHandler.EndAttack();  // 공격 도중 맞으면 IsAttack이 해제되지 않으므로 임의로 해제
        characterObject.MotionHandler.StartHit();
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
    }
}
