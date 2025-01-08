using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StageManager<T> : MonoBehaviour where T : StageInfo
{
    [SerializeField] protected int StageId;

    protected void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<T>(typeof(T).Name).Where(x => x.StageId == StageId).ToArray();
        SpawnEntitys(stageInfos);
    }

    // 스테이지 정보에 따른 엔티티들을 소환합니다.
    protected virtual void SpawnEntitys(T[] stageInfos)
    {
        // 임시 - 현재 Id 규칙이 정해져 있으므로 하드코딩으로 조건문 처리
        foreach (var stageInfo in stageInfos)
        {
            if (stageInfo.CharacterId > 20000 && stageInfo.CharacterId < 30000)
            {
                GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(
                    stageInfo.CharacterId, new Vector3(stageInfo.SpawnPointX, stageInfo.SpawnPointY, 0), Quaternion.identity
                );
            }
            else if (stageInfo.CharacterId > 30000 && stageInfo.CharacterId < 40000)
            {
                GameApplication.Instance.GameController.EnemyController.Spawn<Enemy, EnemyObject>(
                    stageInfo.CharacterId, new Vector3(stageInfo.SpawnPointX, stageInfo.SpawnPointY, 0), Quaternion.identity
                );
            }
        }
    }
}
