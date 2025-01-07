using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.Animator.SetBool("IsWalk", true);
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
        characterObject.Animator.SetBool("IsWalk", false);
    }
}
