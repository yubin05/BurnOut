using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Data
{
    public float LifeTime { get; protected set; }
    public void Init(float lifeTime=-1f)
    {
        LifeTime = lifeTime;
        StartLifeTime(LifeTime);
    }

    private Coroutine LifeTimeCoroutine;
    public void StartLifeTime(float lifeTime)
    {
        if (lifeTime <= 0) return;  // 생명 시간이 0 이하면 잘못 들어온 값으로 판단하여 리턴

        if (LifeTimeCoroutine != null) CoroutineHelper.StopCoroutine(LifeTimeCoroutine);
        LifeTimeCoroutine = CoroutineHelper.StartCoroutine(LifeTimeProcess(lifeTime));
    }
    private IEnumerator LifeTimeProcess(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        RemoveData();
    }
}
