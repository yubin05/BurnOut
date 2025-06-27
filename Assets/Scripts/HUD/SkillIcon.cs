using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface ISkillIconView
{
    public void UpdateUI(SkillIconModel model);
}
public class SkillIcon : MonoBehaviour, ISkillIconView
{
    [SerializeField] private Image skillIconImg;
    [SerializeField] private Image coolTimeImg;
    [SerializeField] private TextMeshProUGUI coolTimeTxt;

    protected SkillIconPresenter presenter;
    public void Init(int skillId)
    {
        presenter = new SkillIconPresenter(this);
        presenter.Init(skillId);
    }
    public void UpdateUI(SkillIconModel model)
    {
        skillIconImg.sprite = Resources.Load<Sprite>(model.SkillIconInfo.Path);

        coolTimeTxt.gameObject.SetActive(false);
    }

    // 스킬 쿨타임 정보 표기 - CoolDownSystem의 OneTimeEvent에 등록하여 호출
    public void UpdateCoolTimeInfo()
    {
        var coolDownSystem = presenter.CoolDownSystem;
        if (coolDownSystem == null) return;

        coolTimeImg.fillAmount = coolDownSystem.CoolTime / coolDownSystem.CoolTimeData;

        float coolTime = coolDownSystem.CoolTime;        
        if (coolTime > 1)
        {
            coolTimeTxt.text = Mathf.RoundToInt(coolTime).ToString();
            coolTimeTxt.gameObject.SetActive(true);

        }
        else if (coolTime > 0)  // 1초 이하는 소수점까지 표시
        {

            coolTimeTxt.text = coolTime.ToString("N1");
            coolTimeTxt.gameObject.SetActive(true);
        }
        else
        {
            coolTimeTxt.gameObject.SetActive(false);
        }
    }
}

public class SkillIconPresenter
{
    protected ISkillIconView view;
    protected SkillIconModel model;
    
    public SkillIconPresenter(SkillIcon _view)
    {
        view = _view;
        model = new SkillIconModel();
    }
    public void Init(int skillId)
    {
        model.SkillId = skillId;
        model.SkillIconInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<IconInfo>(nameof(IconInfo), model.SkillId);

        // 스킬 쿨타임 시스템 등록 - 쿨타임 이미지 갱신 효과 구현 위함
        var players = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        if (players != null) model.CoolDownSystem = players[0].Skills.SkillDatas[model.SkillId].CoolDownSystem;
        else model.CoolDownSystem = null;

        view.UpdateUI(model);
    }

    public CoolDownSystem CoolDownSystem => model.CoolDownSystem;
}

public class SkillIconModel
{
    public int SkillId;
    public IconInfo SkillIconInfo;
    public CoolDownSystem CoolDownSystem;
}
