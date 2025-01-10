using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

// 플레이어 스탯(체력, 마나 등) UI 바
public class PlayerStatsBar : View<PlayerStatsBarPresenter, PlayerStatsBarModel>
{
    [SerializeField] private Transform hpBarBg;
    [SerializeField] private Transform hpBar;   // 체력 UI - scale X 값으로 조정
    [SerializeField] private TextMeshProUGUI hpTxt; // 체력 텍스트 -> $"{현재 체력} / {최대 체력}"

    public override void UpdateUI(PlayerStatsBarModel model)
    {
        hpBar.DOScaleX((float)model.player.CurrentHp/model.player.BasicStat.MaxHp*hpBarBg.localScale.x, 1f);
        hpTxt.text = $"{model.player.CurrentHp} / {model.player.BasicStat.MaxHp}";        
    }
}

public class PlayerStatsBarPresenter : Presenter<PlayerStatsBarModel>
{
    public override void UpdateUI()
    {
        model.player = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player))[0];
        
        base.UpdateUI();
    }
}

public class PlayerStatsBarModel : Model
{
    public Player player;    // 현재 소환된 플레이어의 데이터
}
