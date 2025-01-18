using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.MotionHandler.StartAttack();  
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
        // 공격이 끝난 후, 점프 중이면 점프 모션으로 변경
        if (characterObject.MotionHandler.IsJump)
        {
            characterObject.MotionHandler.StartJump();
        }
    }
}
