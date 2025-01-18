using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬의 베이스가 되는 클래스
/// </summary>
public abstract class Skill
{
    // 스킬 데이터
    public SkillInfo SkillData { get; protected set; }

    // 쿨타임 시스템
    public CoolDownSystem CoolDownSystem { get; protected set; }

    public Skill(int skillId)
    {
        SkillData = GameApplication.Instance.GameModel.PresetData.ReturnData<SkillInfo>(nameof(SkillInfo), skillId);
        CoolDownSystem = new CoolDownSystem(SkillData.CoolTime);
    }

    // 스킬 사용할 때 써야 하는 메서드
    public abstract void Use(Character caster);

    // 스킬 사용할 수 있는 지 체크
    public bool CheckSkill(Character caster)
    {
        if (CoolDownSystem.IsCoolDown)
        {
            Debug.LogWarning($"{this}의 스킬이 쿨타임입니다.");
            return false;
        }
        if (caster.CurrentMp < SkillData.Cost)
        {
            Debug.LogWarning($"{caster}의 마나가 부족합니다.");
            return false;
        }        

        caster.CurrentMp -= SkillData.Cost;
        CoolDownSystem.StartCoolDown();
        return true;
    }
}
