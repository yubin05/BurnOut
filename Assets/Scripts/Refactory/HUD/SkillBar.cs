using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBar : View<SkillBarPresenter, SkillBarModel>
{
    [SerializeField] private Transform parent;
    [SerializeField] private SkillIcon skillIcon;

    public override void UpdateUI(SkillBarModel model)
    {
        for (int i=parent.childCount-1; i>=0; i--) DestroyImmediate(parent.GetChild(i).gameObject);

        foreach (var skillData in model.SkillDatas)
        {
            var comp = Instantiate(skillIcon, parent);
            comp.SkillId = skillData.Key; comp.Init();
            skillData.Value.CoolDownSystem.OneTimeEvent += () => 
            {
                comp.UpdateCoolTimeInfo(skillData.Value);
            };
        }
    }
}

public class SkillBarPresenter : Presenter<SkillBarModel>
{
    public override void UpdateUI()
    {
        var player = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player))[0];
        model.SkillDatas = player.Skills.SkillDatas;

        base.UpdateUI();
    }
}

public class SkillBarModel : Model
{
    public Dictionary<int, Skill> SkillDatas;
}