using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        Skills.AddSkill(new DoubleJump(80001));
        Skills.AddSkill(new DarkMagic(80002));

        OnMaxHp -= StopChargeHp; OnMaxHp += StopChargeHp;   // 최대 체력이면 자동 충전 X
        NotOnMaxHp -= StartChargeHp; NotOnMaxHp += StartChargeHp;   // 최대 체력 아니면 자동 충전 시작

        OnMaxMp -= StopChargeMp; OnMaxMp += StopChargeMp;   // 최대 마나이면 자동 충전 X
        NotOnMaxMp -= StartChargeMp; NotOnMaxMp += StartChargeMp;   // 최대 마나 아니면 자동 충전 시작
    }
}
