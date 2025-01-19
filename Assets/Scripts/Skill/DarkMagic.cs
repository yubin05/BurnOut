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
            if (targetObj != null && !targetObj.ImmunitySystem.IsImmunity)
            {
                targetObj.OnHit(SkillData.Power);
                targetObj.DebuffSystem.StartDebuff(GameApplication.Instance.GameModel.PresetData.ReturnData<VFX>(nameof(VFX), SkillData.VFXId).LifeTime);
                var vfxObj = GameApplication.Instance.GameController.VFXController.Spawn<VFX, VFXObject>(SkillData.VFXId, Vector3.zero, Quaternion.identity, targetObj.transform);

                var target = targetObj.data as Character;
                // 대상 캐릭터가 죽으면 소환했던 VFX 오브젝트 삭제
                target.OnDataRemove += (data) => 
                {
                    var vfx = vfxObj.data as VFX;
                    vfx.RemoveData();
                };
            }
        }
    }
}
