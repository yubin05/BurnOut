using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeObject : EnemyObject
{
    public override void Init(Data data)
    {
        base.Init(data);

        MotionHandler.HitEvent += () => 
        {
            GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10007, Vector3.zero, Quaternion.identity);
        };
    }

    public override void OnDeath()
    {
        base.OnDeath();

        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10008, Vector3.zero, Quaternion.identity);
    }
}
