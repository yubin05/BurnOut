using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// View
public class MainCanvas : View<MainCanvasPresenter, MainCanvasModel>
{
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI titleTxt;

    [Header("컨텐츠")]
    [SerializeField] private MainMenu mainMenu;

    public override void UpdateUI(MainCanvasModel model)
    {
        titleTxt.UpdateTextInfoName(model.titleTextInfoId);
        mainMenu.Init();
    }
}

// Presenter
public class MainCanvasPresenter : Presenter<MainCanvasModel>
{
}

// Model
public class MainCanvasModel : Model
{
    public int titleTextInfoId = 2;
}