using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.Animator.SetBool("IsJump", true);
        characterObject.Animator.SetTrigger("OnJump");
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
        characterObject.Animator.SetBool("IsJump", false);
    }
}
