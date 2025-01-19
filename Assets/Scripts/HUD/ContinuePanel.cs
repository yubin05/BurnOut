using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ContinuePanel : View<ContinuePanelPresenter, ContinuePanelModel>
{
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;

    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI yesTxt;
    [SerializeField] private TextMeshProUGUI noTxt;

    public override void UpdateUI(ContinuePanelModel model)
    {
        titleTxt.UpdateTextInfoName(model.titleTxtId);
        yesTxt.UpdateTextInfoName(model.yesTxtId);
        noTxt.UpdateTextInfoName(model.noTxtId);

        yesBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => 
        {
            StageManager.Instance.SpawnPlayer(true);
        });

        noBtn.onClick.RemoveAllListeners();
        noBtn.onClick.AddListener(() => 
        {
            SceneManager.LoadScene(SceneName.StartScene);
        });
    }

    public override void OnShow()
    {
        transform.localScale = Vector3.zero;
        base.OnShow();  // gameObject.SetActive(true);

        transform.DOScale(1f, 1f).SetEase(Ease.OutBack);
    }
}

public class ContinuePanelPresenter : Presenter<ContinuePanelModel>
{
}

public class ContinuePanelModel : Model
{
    public int titleTxtId = 1009;
    public int yesTxtId = 1010;
    public int noTxtId = 1011;
}
