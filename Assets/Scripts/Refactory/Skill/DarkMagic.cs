using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMagic : Skill
{
    public DarkMagic(int skillId) : base(skillId) {}

    public override void Use(Character caster)
    {
        if (!CheckSkill(caster)) return;

        var hitBoxs = Physics2D.OverlapBoxAll(caster.MyObject.transform.position, Vector2.one*10f, 0, caster.AttackTargets);
        foreach (var hitBox in hitBoxs)
        {
            var targetObj = hitBox.GetComponent<CharacterObject>();
            if (targetObj != null)
            {
                targetObj.OnHit(SkillData.Power);
                GameApplication.Instance.GameController.VFXController.Spawn<VFX, VFXObject>(70002, Vector3.zero, Quaternion.identity, targetObj.transform);
            }
        }
    }
}
