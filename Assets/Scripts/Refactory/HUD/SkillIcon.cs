using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIcon : View<SkillIconPresenter, SkillIconModel>
{
    public int SkillId { get; set; }

    [SerializeField] private Image skillIconImg;
    [SerializeField] private Image inputKeyIconImg;
    [SerializeField] private Image coolTimeImg;

    [SerializeField] private TextMeshProUGUI coolTimeTxt;

    public override void UpdateUI(SkillIconModel model)
    {
        skillIconImg.sprite = Resources.Load<Sprite>(GameApplication.Instance.GameModel.PresetData.ReturnData<IconInfo>(nameof(IconInfo), SkillId).Path);
        inputKeyIconImg.sprite = Resources.Load<Sprite>(GameApplication.Instance.GameModel.PresetData.ReturnData<IconInfo>(nameof(IconInfo), GameApplication.Instance.GameModel.ClientData.PlayerKeyCodes.playerKeyCodes.Find(x => x.Id == SkillId).KeyCodeId).Path);

        coolTimeTxt.gameObject.SetActive(false);
    }

    public void UpdateCoolTimeInfo(Skill skill)
    {
        var coolDownSystem = skill.CoolDownSystem;
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

public class SkillIconPresenter : Presenter<SkillIconModel>
{
}

public class SkillIconModel : Model
{   
}
