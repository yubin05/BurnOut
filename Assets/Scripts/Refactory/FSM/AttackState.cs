using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(CharacterObject characterObject) : base(characterObject) {}

    public override void OnStateEnter(CharacterObject characterObject)
    {
        characterObject.Animator.SetTrigger("OnAttack");
        characterObject.Animator.SetBool("IsAttack", true);
    }

    public override void OnStateUpdate(CharacterObject characterObject)
    {
    }

    public override void OnStateExit(CharacterObject characterObject)
    {
        characterObject.Animator.SetBool("IsAttack", false);
        // 공격 상태 같은 경우는 모션이 다 끝나지 않아도 상태 Exit가 가능하므로 임의로 EndAttack 호출
        characterObject.MotionHandler.EndAttack();
    }
}
