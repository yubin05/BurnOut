using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 쿨타임 시스템 (스킬 등에서 사용)
/// </summary>
public class CoolDownSystem
{
    public float CoolTimeData { get; private set; } // 쿨타임 데이터 (한번 등록되면 변화 X)
    public float CoolTime { get; private set; } // 실제 쿨타임 체크할 때 사용할 변수
    public bool IsCoolDown
    {
        get
        {
            return CoolTime > 0 ? true : false;
        }
    }

    private Coroutine cCoolTime;
    public event Action OneTimeEvent;

    public CoolDownSystem(float coolTime)
    {
        CoolTimeData = coolTime;
        cCoolTime = null;
        OneTimeEvent = null;
    }

    public void StartCoolDown()
    {
        if (cCoolTime != null) CoolDownCoroutineHelper.StopCoroutine(cCoolTime);
        cCoolTime = CoolDownCoroutineHelper.StartCoroutine(CoolDownProcess());
    }
    private IEnumerator CoolDownProcess()
    {
        CoolTime = CoolTimeData;
        OneTimeEvent?.Invoke();
        while (CoolTime > 0)
        {
            yield return null;
            CoolTime -= Time.deltaTime;
            OneTimeEvent?.Invoke();
        }
    }
}
