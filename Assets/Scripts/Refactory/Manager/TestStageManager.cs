using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageManager : StageManager<TestStageInfo>
{
    protected override void PlayBGM()
    {
        GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(10003, Vector3.zero, Quaternion.identity);
    }
}
