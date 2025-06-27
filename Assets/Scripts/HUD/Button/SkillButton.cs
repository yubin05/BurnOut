using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillButton<T> : ActionButton where T : SkillButton<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>();
            return instance;
        }
    }

    [SerializeField] protected SkillIcon skillIcon; // 스킬 아이콘

    protected SkillButtonPresenter<T> presenter;
    public virtual void Init(int skillId)
    {
        presenter = new SkillButtonPresenter<T>();
        presenter.Init(this, skillId);

        m_button.onClick.RemoveListener(presenter.UseSkill);
        m_button.onClick.AddListener(presenter.UseSkill);        
    }

    public virtual void UpdateUI(SkillButtonModel<T> model)
    {
        skillIcon.Init(model.SkillId);

        // 스킬 쿨타임 이미지 이벤트 등록
        var players = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        if (players != null)
        {
            var player = players[0]; var skill = player.Skills.SkillDatas[model.SkillId];
            skill.CoolDownSystem.OneTimeEvent -= skillIcon.UpdateCoolTimeInfo;
            skill.CoolDownSystem.OneTimeEvent += skillIcon.UpdateCoolTimeInfo;
        }
    }

    // 추후 스킬 변경하는 기능 추가하면 사용할 스킬 변경 기능
    public virtual void ChangeSkill(int skillId)
    {
        presenter.UpdateSkill(skillId);
    }

    protected sealed override void Init() {}  // 스킬은 Id값을 넣어 초기화할 것이므로 매개변수 없는 Init은 상속받지 못하게 변경
}

public class SkillButtonPresenter<T> where T : SkillButton<T>
{
    protected SkillButton<T> view;
    protected SkillButtonModel<T> model;

    public void Init(SkillButton<T> _view, int skillId)
    {
        view = _view;
        model = new SkillButtonModel<T>();

        UpdateSkill(skillId);
    }

    public void UpdateSkill(int skillId)
    {
        model.SkillId = skillId;
        view.UpdateUI(model);
    }

    public void UseSkill()
    {
        var players = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        if (players != null)
        {
            var player = players[0];
            var playerObj = player.MyObject as PlayerObject;
            if (playerObj.MotionHandler.IsHit || playerObj.MotionHandler.IsDeath) return;

            player.Skills.Use(model.SkillId);
        }
    }
}

public class SkillButtonModel<T> where T : SkillButton<T>
{
    public int SkillId;     // 각 스킬 버튼에서 적용할 스킬 Id
}