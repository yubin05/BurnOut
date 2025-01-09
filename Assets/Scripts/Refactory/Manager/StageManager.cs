using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StageManager<T> : MonoBehaviour where T : StageInfo
{
    [SerializeField] protected int stageId;
    [SerializeField] protected Transform startPoint;

    protected void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        var playerId = GameApplication.Instance.GameModel.PresetData.ReturnDatas<Player>(nameof(Player)).First().Id;
        var playerObj = SpawnPlayer(playerId);
        var player = playerObj.data as Player;

        CameraSystem.Instance.Init();
        CameraSystem.Instance.StartTracking(playerObj); // 카메라가 플레이어를 따라갑니다.        
        player.OnDataRemove += (player) => 
        {
            CameraSystem.Instance.StopTracking();   // 카메라가 플레이어를 따라가는 것을 멈춥니다.
        };

        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<T>(typeof(T).Name).Where(x => x.StageId == stageId).ToArray();
        SpawnEnemys(stageInfos);

        PlayBGM();
    }

    // 플레이어 캐릭터를 소환합니다.
    protected virtual PlayerObject SpawnPlayer(int playerId)
    {
        return GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(
            playerId, startPoint.transform.position, startPoint.transform.rotation
        );
    }

    // 스테이지 정보에 따른 적들을 소환합니다.
    protected virtual void SpawnEnemys(T[] stageInfos)
    {
        foreach (var stageInfo in stageInfos)
        {
            GameApplication.Instance.GameController.EnemyController.Spawn<Enemy, EnemyObject>(
                stageInfo.CharacterId, new Vector3(stageInfo.SpawnPointX, stageInfo.SpawnPointY, 0), Quaternion.identity
            );
        }
    }

    protected abstract void PlayBGM();
}
