using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 머리 위에 나타나는 HUD(체력 등을 표시)
/// </summary>
public class HeadBar : Entity
{
    public CharacterObject TargetObj { get; set; } // 헤드바의 대상이 되는 캐릭터 오브젝트
}
