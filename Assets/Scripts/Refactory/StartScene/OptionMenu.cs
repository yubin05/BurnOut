using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : View<OptionMenuPresenter, OptionMenuModel>
{
    [Header("슬라이더")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI volumeTxt;
    [SerializeField] private TextMeshProUGUI bgmTxt;
    [SerializeField] private TextMeshProUGUI sfxTxt;
    [SerializeField] private List<TextMeshProUGUI> optionMenuTxts;

    [Header("컨텐츠")]
    [SerializeField] private MainMenu mainMenu;

    private void Update()
    {
        // 메뉴 선택
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (presenter.OptionMenuSelectIndex)
            {
                case 0: OnHide(); mainMenu.Init(); break;
            }
        }

        // 위로 이동
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectMenuUI(presenter.ChangeSelectIndex(1));
        }
        // 아래로 이동
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectMenuUI(presenter.ChangeSelectIndex(-1));
        }
    }

    public override void UpdateUI(OptionMenuModel model)
    {
        volumeTxt.UpdateTextInfoName(model.volumeTxtId);
        bgmTxt.UpdateTextInfoName(model.bgmTxtId);
        sfxTxt.UpdateTextInfoName(model.sfxTxtId);

        for (int i=0; i<optionMenuTxts.Count; i++)
        {
            optionMenuTxts[i].UpdateTextInfoName(model.optionMenuTxtIds[i]);
        }

        SelectMenuUI(model.optionMenuSelectIndex);

        bgmSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.value = GameApplication.Instance.GameModel.ClientData.PlayerSound.bgm;
        bgmSlider.onValueChanged.AddListener((bgm) => 
        {
            GameApplication.Instance.GameController.SoundController.ChangeBGMVolume(bgm);
        });

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.value = GameApplication.Instance.GameModel.ClientData.PlayerSound.sfx;
        sfxSlider.onValueChanged.AddListener((sfx) => 
        {
            GameApplication.Instance.GameController.SoundController.ChangeSFXVolume(sfx);
        });
    }

    // 메뉴 선택 표시해주는 함수 - 선택한 옵션 빨간색으로 표시
    private void SelectMenuUI(int index)
    {
        for (int i=0; i<optionMenuTxts.Count; i++)
        {
            if (i == index) optionMenuTxts[i].color = Color.red;
            else optionMenuTxts[i].color = Color.white;
        }
    }
}

public class OptionMenuPresenter : Presenter<OptionMenuModel>
{
    public int OptionMenuSelectIndex => model.optionMenuSelectIndex;

    // 메뉴 선택 변경해주는 함수
    public int ChangeSelectIndex(int direction)    // -1: 아래, 1: 위
    {
        int selectIndex = model.optionMenuSelectIndex;
        if (direction == -1)
        {
            if (++selectIndex > model.optionMenuTxtIds.Count-1) selectIndex = 0;
        }
        else if (direction == 1)
        {
            if (--selectIndex < 0) selectIndex=model.optionMenuTxtIds.Count-1;    
        }
        model.optionMenuSelectIndex = selectIndex;
        return model.optionMenuSelectIndex;
    }
}

public class OptionMenuModel : Model
{
    public int volumeTxtId = 5;
    public int bgmTxtId = 6;
    public int sfxTxtId = 7;
    public List<int> optionMenuTxtIds = new List<int>() { 8 };
    public int optionMenuSelectIndex = 0;
}