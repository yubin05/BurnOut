using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : LocalSingleton<Message>
{
    [SerializeField] private Image bgImg;
    [SerializeField] private TextMeshProUGUI messageTxt;

    private void Start()
    {
        OnHide();
    }

    public void Init(int id, TextInfo.Types type)
    {
        switch (type)
        {
            case TextInfo.Types.Name: default: messageTxt.UpdateTextInfoName(id); break;
            case TextInfo.Types.Desc: messageTxt.UpdateTextInfoDesc(id); break;
        }

        if (!bgImg.gameObject.activeSelf) OnShow();

        CancelInvoke(nameof(OnHide));
        Invoke(nameof(OnHide), 1f);
    }

    public void OnShow() => bgImg.gameObject.SetActive(true);
    public void OnHide() => bgImg.gameObject.SetActive(false);
}
