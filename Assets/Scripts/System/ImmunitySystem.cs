using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무적 시스템
public class ImmunitySystem : MonoBehaviour
{
    public bool IsImmunity { get; private set; }    // 무적 상태인지 체크
    public event Action OnStartImmunity;    // 무적 시작 이벤트
    public event Action OnStopImmunity;     // 무적 종료 이벤트

    public string OriginLayerName { get; private set; }
    public string ImmunityLayerName { get; private set; }
    private Coroutine CImmunity;    

    public void Init()
    {
        IsImmunity = false;
        OnStartImmunity = null;
        OnStopImmunity = null;

        OriginLayerName = LayerMask.LayerToName(gameObject.layer);
        ImmunityLayerName = "Immunity";
        CImmunity = null;
    }

    // 무적 시스템을 활성화합니다.
    public void StartImmunity(float immunityDuration=float.MaxValue)
    {
        if (CImmunity != null) StopCoroutine(CImmunity);
        CImmunity = StartCoroutine(ImmunityProcess(immunityDuration));
    }
    private IEnumerator ImmunityProcess(float immunityDuration)
    {
        IsImmunity = true;
        OnStartImmunity?.Invoke();

        yield return new WaitForSeconds(immunityDuration);

        IsImmunity = false;
        OnStopImmunity?.Invoke();
    }
}
