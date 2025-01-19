using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSystem : MonoBehaviour
{
    public bool IsDebuff { get; private set; }
    public event Action OnStartDebuff;
    public event Action OnStopDebuff;

    private Coroutine cDebuff;

    public void Init()
    {
        IsDebuff = false;
        OnStartDebuff = null;
        OnStopDebuff = null;

        cDebuff = null;
    }

    // 디버프 시스템을 활성화합니다.
    public void StartDebuff(float duration)
    {
        if (cDebuff != null) StopCoroutine(cDebuff);
        cDebuff = StartCoroutine(DebuffProcess(duration));
    }
    private IEnumerator DebuffProcess(float duration)
    {
        IsDebuff = true;
        OnStartDebuff?.Invoke();

        yield return new WaitForSeconds(duration);

        IsDebuff = false;
        OnStopDebuff?.Invoke();
    }
    public void StopDebuff()
    {
        if (cDebuff != null) StopCoroutine(cDebuff);

        IsDebuff = false;
        OnStopDebuff?.Invoke();
    }
}
