using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    // 해당 캐릭터가 가지고 있는 기본 스탯
    public CharacterStat BasicStat { get; set; }

    // 해당 캐릭터가 공격할 수 있는 오브젝트의 레이어
    public LayerMask AttackTargets { get; set; }

    // 캐릭터 오브젝트가 보고 있는 방향 (왼쪽:-1, 오른쪽:1)
    public enum MoveDirectionXs { Left=-1, Right=1 }
    protected MoveDirectionXs moveDirectionX;
    public MoveDirectionXs MoveDirectionX
    { 
        get { return moveDirectionX; }
        set
        {
            moveDirectionX = value;

            var scale = MyObject.transform.localScale;
            scale.x = (int)moveDirectionX;
            MyObject.transform.localScale = scale;            
        }
    }

    // 캐릭터가 가지고 있는 현재 체력
    protected int currentHp;
    public int CurrentHp
    { 
        get { return currentHp; }
        set
        {
            var preCurrentHp = currentHp;
            currentHp = Mathf.Clamp(value, 0, BasicStat.MaxHp);
            if (preCurrentHp != currentHp) OnChangeCurrentHp?.Invoke(currentHp);
        }
    }
    // 현재 체력 변화할 때마다 호출할 이벤트
    public event Action<int> OnChangeCurrentHp;

    // 캐릭터가 가지고 있는 현재 마나
    protected int currentMp;
    public int CurrentMp
    { 
        get { return currentMp; }
        set
        {
            var preCurrentMp = currentMp;
            currentMp = Mathf.Clamp(value, 0, BasicStat.MaxMp);
            if (preCurrentMp != currentMp) OnChangeCurrentMp?.Invoke(currentMp);
        }
    }
    // 현재 마나 변화할 때마다 호출할 이벤트
    public event Action<int> OnChangeCurrentMp;

    // 공격 딜레이 - 공격 속도 반영
    public float AttackDelayTime { get; set; }

    public bool IsDoubleJump { get; set; }  // 더블 점프 중인지 체크

    // 스킬
    public Skills Skills { get; set; }

    // 마나 자연 회복 코루틴
    public Coroutine CChargeMp { get; protected set; }

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        OnChangeCurrentHp = null;
        OnChangeCurrentMp = null;
        AttackDelayTime = 0f;

        IsDoubleJump = false;

        Skills = new Skills(this);
        CChargeMp = null;
    }

    // 방향 전환
    public void ChangeDirectionX()
    {
        var diretionX = (int)MoveDirectionX;
        diretionX *= -1;
        MoveDirectionX = (Character.MoveDirectionXs)diretionX;
    }

    // 자동 마나 충전
    public void StartChargeMp()
    {
        if (CChargeMp != null) MyObject.StopCoroutine(CChargeMp);
        CChargeMp = MyObject.StartCoroutine(ChangeMpProcess());
    }
    protected IEnumerator ChangeMpProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(BasicStat.MpChargeSecond);
            CurrentMp += BasicStat.MpChargeValue;
        }        
    }
}
