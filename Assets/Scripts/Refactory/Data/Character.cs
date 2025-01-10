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
            currentHp = value;
            if (StageManager.Instance)
            {
                StageManager.Instance.PlayerStatsBar.UpdateUI();   
            }
        }
    }

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);
    }
}
