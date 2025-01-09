using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MotionHandler : MonoBehaviour
{
    private Animator animator;

    public bool IsMove { get; private set; }
    public event Action MoveEvent;

    public bool IsJump { get; private set; }
    public event Action JumpEvent;

    public bool IsAttack { get; private set; }
    public event Action AttackEvent;

    public bool IsHit { get; private set; }
    public event Action HitEvent;

    public bool IsDeath { get; private set; }
    public event Action DeathEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        IsMove = false;
        MoveEvent = null;

        IsJump = false;
        JumpEvent = null;

        IsAttack = false;
        AttackEvent = null;

        IsHit = false;
        HitEvent = null;

        IsDeath = false;
        DeathEvent = null;
    }

    // 이동 시작
    public void StartMove()
    {
        IsMove = true;
        animator.SetBool(nameof(IsMove), IsMove);
    }
    // 이동 이벤트
    public void OnMove()
    {
        MoveEvent?.Invoke();
    }
    // 이동 끝
    public void EndMove()
    {
        IsMove = false;
        animator.SetBool(nameof(IsMove), IsMove);
    }

    // 점프 시작
    public void StartJump()
    {
        IsJump = true;
        animator.SetBool(nameof(IsJump), IsJump);
        animator.SetTrigger(nameof(OnJump));
    }
    // 점프 이벤트
    public void OnJump()
    {
        JumpEvent?.Invoke();
    }
    // 점프 끝
    public void EndJump()
    {
        IsJump = false;
        animator.SetBool(nameof(IsJump), IsJump);
    }

    // 공격 시작
    public void StartAttack()
    {
        IsAttack = true;
        animator.SetBool(nameof(IsAttack), IsAttack);
        animator.SetTrigger(nameof(OnAttack));
    }
    // 공격 이벤트
    public void OnAttack()
    {
        AttackEvent?.Invoke();
    }
    // 공격 끝
    public void EndAttack()
    {
        IsAttack = false;
        animator.SetBool(nameof(IsAttack), IsAttack);
    }

    // 피격 시작
    public void StartHit()
    {
        IsHit = true;
        animator.SetBool(nameof(IsHit), IsHit);
        animator.SetTrigger(nameof(OnHit));
    }
    // 피격 이벤트
    public void OnHit()
    {
        HitEvent?.Invoke();
    }
    // 피격 끝
    public void EndHit()
    {
        IsHit = false;
        animator.SetBool(nameof(IsHit), IsHit);
    }

    // 죽음 시작
    public void StartDeath()
    {
        IsDeath = true;
        animator.SetBool(nameof(IsDeath), IsDeath);
        animator.SetTrigger(nameof(OnDeath));
    }
    // 죽음 이벤트
    public void OnDeath()
    {
        DeathEvent?.Invoke();
    }
    // 죽으면 데이터가 사라지므로 End 모션은 존재하지 않음
}
