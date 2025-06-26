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

    [Header("언어")]
    [SerializeField] private TextMeshProUGUI languagetitleTxt;
    [SerializeField] private List<TextMeshProUGUI> languageTxts;
    [SerializeField] private List<Toggle> languageToggles;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI volumeTxt;
    [SerializeField] private TextMeshProUGUI bgmTxt;
    [SerializeField] private TextMeshProUGUI sfxTxt;
    [SerializeField] private List<TextMeshProUGUI> optionMenuTxts;

    [Header("컨텐츠")]
    [SerializeField] private MainMenu mainMenu;

    // 버튼 터치 방식으로 변경함에 따라 필요 없어짐
    //private void Update()
    //{
    //    // 메뉴 선택
    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        switch (presenter.OptionMenuSelectIndex)
    //        {
    //            case 0: OnHide(); mainMenu.Init(); break;
    //        }
    //    }

    //    // 위로 이동
    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        SelectMenuUI(presenter.ChangeSelectIndex(1));
    //    }
    //    // 아래로 이동
    //    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
    //    {
    //        SelectMenuUI(presenter.ChangeSelectIndex(-1));
    //    }
    //}

    // 버튼 기능들
    public void PressBackBtn() { OnHide(); mainMenu.Init(); }   // 뒤로 가기

    public override void UpdateUI(OptionMenuModel model)
    {
        // 볼륨 관련 슬라이더 이벤트 설정
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

        // 볼륨 관련 텍스트 설정
        volumeTxt.UpdateTextInfoName(model.volumeTxtId);
        bgmTxt.UpdateTextInfoName(model.bgmTxtId);
        sfxTxt.UpdateTextInfoName(model.sfxTxtId);

        // 언어 관련 설정        
        for (int i=0; i<languageToggles.Count; i++)
        {
            int index = i; var languageToggle = languageToggles[index];
            languageToggle.onValueChanged.RemoveAllListeners();
            languageToggle.onValueChanged.AddListener((isOn) => 
            {
                if (isOn) GameManager.Instance.ChanageLanauage((TextInfo.LanguageTypes)index);
            });
        }
        languageToggles[(int)GameApplication.Instance.GameModel.ClientData.PlayerLanguage.language].isOn = true;

        // 언어 관련 텍스트 설정
        languagetitleTxt.UpdateTextInfoName(model.languageTitleTxtId);
        for (int i=0; i<languageTxts.Count; i++)
        {
            var languageTxt = languageTxts[i];
            languageTxt.UpdateTextInfoName(model.languageTxtIds[i]);
        }

        // 기타 옵션 메뉴 설정
        //SelectMenuUI(model.optionMenuSelectIndex);

        // 기타 옵션 메뉴 텍스트 설정
        for (int i=0; i<optionMenuTxts.Count; i++)
        {
            optionMenuTxts[i].UpdateTextInfoName(model.optionMenuTxtIds[i]);
        }
    }

    // 버튼 터치 방식으로 변경함에 따라 필요 없어짐
    // 메뉴 선택 표시해주는 함수 - 선택한 옵션 빨간색으로 표시
    //private void SelectMenuUI(int index)
    //{
    //    for (int i=0; i<optionMenuTxts.Count; i++)
    //    {
    //        if (i == index) optionMenuTxts[i].color = Color.red;
    //        else optionMenuTxts[i].color = Color.white;
    //    }
    //}
}

public class OptionMenuPresenter : Presenter<OptionMenuModel>
{
    //public int OptionMenuSelectIndex => model.optionMenuSelectIndex;

    // 버튼 터치 방식으로 변경함에 따라 필요 없어짐
    //// 메뉴 선택 변경해주는 함수
    //public int ChangeSelectIndex(int direction)    // -1: 아래, 1: 위
    //{
    //    int selectIndex = model.optionMenuSelectIndex;
    //    if (direction == -1)
    //    {
    //        if (++selectIndex > model.optionMenuTxtIds.Count-1) selectIndex = 0;
    //    }
    //    else if (direction == 1)
    //    {
    //        if (--selectIndex < 0) selectIndex=model.optionMenuTxtIds.Count-1;    
    //    }
    //    model.optionMenuSelectIndex = selectIndex;
    //    return model.optionMenuSelectIndex;
    //}
}

public class OptionMenuModel : Model
{
    public int volumeTxtId = 1005;
    public int bgmTxtId = 1006;
    public int sfxTxtId = 1007;

    public int languageTitleTxtId = 1012;
    public List<int> languageTxtIds = new List<int>() { 1013, 1014 };

    public List<int> optionMenuTxtIds = new List<int>() { 1008 };
    //public int optionMenuSelectIndex = 0;
}