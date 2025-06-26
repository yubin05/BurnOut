using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : View<MainMenuPresenter, MainMenuModel>
{
    [Header("버튼")]
    [SerializeField] private List<Button> mainMenuBtns;

    [Header("텍스트")]
    [SerializeField] private List<TextMeshProUGUI> mainMenuTxts;

    [Header("컨텐츠")]
    [SerializeField] private OptionMenu optionMenu;

    // 모바일 포팅하면서 버튼 터치로 변경함에 따라 필요 없어짐
    //private void Update()
    //{
    //    // 메뉴 선택
    //    //if (Input.GetKeyDown(KeyCode.Return))
    //    //{
    //    //    switch (presenter.MainMenuSelectIndex)
    //    //    {
    //    //        case 0: SceneManager.LoadScene(SceneName.TestStage); break;
    //    //        case 1: OnHide(); optionMenu.Init(); break;
    //    //        case 2: GameManager.Instance.QuitGame(); break;
    //    //    }
    //    //}

    //    //// 위로 이동
    //    //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //    //{
    //    //    SelectMenuUI(presenter.ChangeSelectIndex(1));
    //    //}
    //    //// 아래로 이동
    //    //else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
    //    //{
    //    //    SelectMenuUI(presenter.ChangeSelectIndex(-1));
    //    //}
    //}

    // 버튼 기능들
    public void PressStartBtn() { SceneManager.LoadScene(SceneName.StageScene); }    // 시작
    public void PressOptionBtn() { OnHide(); optionMenu.Init(); }    // 옵션
    public void PressQuitBtn() { GameManager.Instance.QuitGame(); }     // 나가기

    // UI 갱신
    public override void UpdateUI(MainMenuModel model)
    {
        // 버튼 텍스트 초기화
        for (int i = 0; i < mainMenuTxts.Count; i++)
        {
            mainMenuTxts[i].UpdateTextInfoName(model.mainMenuTxtIds[i]);
        }

        //SelectMenuUI(model.mainMenuSelectIndex);

        optionMenu.OnHide();
    }

    // 모바일 포팅하면서 버튼 터치로 변경함에 따라 필요 없어짐
    // 메뉴 선택 표시해주는 함수 - 선택한 옵션 빨간색으로 표시
    //private void SelectMenuUI(int index)
    //{
    //    for (int i=0; i<mainMenuTxts.Count; i++)
    //    {
    //        if (i == index) mainMenuTxts[i].color = Color.red;
    //        else mainMenuTxts[i].color = Color.white;
    //    }
    //}
}

public class MainMenuPresenter : Presenter<MainMenuModel>
{
    // 모바일 포팅하면서 버튼 터치로 변경함에 따라 필요 없어짐
    //public int MainMenuSelectIndex => model.mainMenuSelectIndex;

    // 메뉴 선택 변경해주는 함수
    //public int ChangeSelectIndex(int direction)    // -1: 아래, 1: 위
    //{
    //    int selectIndex = model.mainMenuSelectIndex;
    //    if (direction == -1)
    //    {
    //        if (++selectIndex > model.mainMenuTxtIds.Count-1) selectIndex = 0;
    //    }
    //    else if (direction == 1)
    //    {
    //        if (--selectIndex < 0) selectIndex=model.mainMenuTxtIds.Count-1;    
    //    }
    //    model.mainMenuSelectIndex = selectIndex;
    //    return model.mainMenuSelectIndex;
    //}
}

public class MainMenuModel : Model
{
    public List<int> mainMenuTxtIds = new List<int>() { 1001, 1003, 1004 };
    //public int mainMenuSelectIndex = 0;
}