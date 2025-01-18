using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeadBarObject : EntityObject
{
    [SerializeField] private Transform bgHeadBarTrans;
    [SerializeField] private Transform headBarTrans;

    private void Update()
    {
        var headBar = data as HeadBar;
        if (headBar.TargetObj == null) return;

        // 대상 오브젝트 위치 따라가기
        transform.position = CameraSystem.Instance.TargetCamera.WorldToScreenPoint(headBar.TargetObj.HeadBarNode.position);
    }

    public void UpdateHeadBar()
    {
        var headBar = data as HeadBar;
        if (headBar.TargetObj == null) return;

        var target = headBar.TargetObj.data as Character;
        headBarTrans.DOScaleX((float)target.CurrentHp/target.BasicStat.MaxHp*bgHeadBarTrans.localScale.x, 0f);
    }
}
