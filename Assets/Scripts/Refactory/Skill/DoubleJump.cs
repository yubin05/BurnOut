using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Skill
{
    public DoubleJump(int skillId) : base(skillId) {}

    public override void Use(Character caster)
    {
        if (caster.CurrentMp < SkillData.Cost)
        {
            Debug.LogWarning($"{caster}의 마나가 부족합니다.");
            return;
        }

        caster.CurrentMp -= SkillData.Cost;
        caster.IsDoubleJump = true;
        
        var casterObj = caster.MyObject as CharacterObject;

        casterObj.FSM.ChangeState(new JumpState(casterObj));
        casterObj.Rigidbody2D.AddForce(new Vector2((int)caster.MoveDirectionX, 0.5f).normalized*caster.BasicStat.JumpPower, ForceMode2D.Impulse);
        GameApplication.Instance.GameController.VFXController.Spawn<VFX, VFXObject>(70001, Vector3.zero, Quaternion.identity, casterObj.BackNode);
    }
}
