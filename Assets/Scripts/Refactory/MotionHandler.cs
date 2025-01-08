using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHandler : MonoBehaviour
{
    public bool IsJump { get; set; }
    public Action JumpEvent;

    public bool IsAttack { get; set; }
    public Action AttackEvent;

    public void Init()
    {
        IsJump = false;

        IsAttack = false;
        AttackEvent = null;
    }

    public void OnJump()
    {
        IsJump = true;
        JumpEvent?.Invoke();
    }

    public void StartAttack()
    {
        IsAttack = true;
    }
    public void OnAttack()
    {
        AttackEvent?.Invoke();
    }
    public void EndAttack()
    {
        IsAttack = false;
    }
}
