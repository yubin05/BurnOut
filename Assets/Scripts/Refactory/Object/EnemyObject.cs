using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CharacterObject
{
    public int MoveDirection { get; set; }

    public override void Init(Data data)
    {
        base.Init(data);

        var enemy = data as Enemy;
        enemy.Init();

        MoveDirection = -1;
    }

    protected virtual void Update()
    {
        FSM.ChangeState(new WalkState(this));

        var enemy = data as Enemy;
        transform.Translate(new Vector3(MoveDirection * (enemy.BasicStat.MoveSpeed*Time.deltaTime), 0, 0));
    }
}
