using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Skill
{
    public DoubleJump(int skillId) : base(skillId) {}

    public override void Use(Character caster)
    {
        if (!CheckSkill(caster)) return;

        var casterObj = caster.MyObject as CharacterObject;
        caster.IsDoubleJump = true;

        casterObj.FSM.ChangeState(new JumpState(casterObj));
        casterObj.Rigidbody2D.AddForce(new Vector2((int)caster.MoveDirectionX, 0.5f).normalized*SkillData.Power, ForceMode2D.Impulse);
        GameApplication.Instance.GameController.VFXController.Spawn<VFX, VFXObject>(SkillData.VFXId, Vector3.zero, Quaternion.identity, casterObj.BackNode);
    }
}
