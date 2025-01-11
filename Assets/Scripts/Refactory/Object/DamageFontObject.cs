using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageFontObject : EntityObject
{
    [SerializeField] private TextMeshProUGUI damageTxt;
    protected RectTransform rectTransform;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Update()
    {
        var damageFont = data as DamageFont;
        
        var a = CameraSystem.Instance.TargetCamera.WorldToScreenPoint(damageFont.InitWorldPositon);
        var b = CameraSystem.Instance.TargetCamera.WorldToScreenPoint(damageFont.EndWorldPosition);
        var t = LerpExtension.BezierCurve(Vector2.zero, new Vector2(0, 1), Vector2.one, damageFont.ArriveDeltaTime/damageFont.LifeTime).y;

        transform.position = Vector2.Lerp(a, b, t);
        damageFont.ArriveDeltaTime += Time.deltaTime;
    }

    public void SetDamageTxt()
    {
        var damageFont = data as DamageFont;
        damageTxt.text = damageFont.HitDamage.ToString();
    }
}
