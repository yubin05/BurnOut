using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public CharacterStat BasicStat { get; set; }

    // 캐릭터 오브젝트가 보고 있는 방향 (왼쪽:-1, 오른쪽:1)
    public enum MoveDirectionXs { Left=-1, Right=1 }
    private MoveDirectionXs moveDirectionX;
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

    private int currentHp;
    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
        }
    }

    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);
    }
}
