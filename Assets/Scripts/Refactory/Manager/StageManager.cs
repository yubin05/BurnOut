using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : LocalSingleton<StageManager>
{
    [SerializeField] private int stageId; // 스테이지 아이디
    public int StageId => stageId;
    
    [SerializeField] private PlayerStatsBar playerStatsBar;   // 체력, 마나 등의 정보를 담은 UI바
    public PlayerStatsBar PlayerStatsBar => playerStatsBar;

    [SerializeField] private Canvas dynamicOverlayCanvas;  // 동적 오버레이 캔버스
    public Canvas DynamicOverlayCanvas => dynamicOverlayCanvas;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        var playerObj = SpawnPlayer();
        SpawnEnemys();
        StartCameraTracking(playerObj, true);
        // PlayBGM();
    }

    // 플레이어 캐릭터를 소환합니다.
    public PlayerObject SpawnPlayer()
    {
        var playerInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        foreach (var playerInfo in playerInfos) playerInfo.RemoveData();

        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId);
        var stagePlayerInfos = stageInfos.Where(x => x.Type == StageEntityInfo.Types.Player);

        var stagePlayerInfo = stagePlayerInfos.First();
        var playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(
            stagePlayerInfo.EntityId, new Vector2(stagePlayerInfo.SpawnPointX, stagePlayerInfo.SpawnPointY), Quaternion.identity
        );

        var player = playerObj.data as Player;
        player.OnChangeCurrentHp += (updateHp) => 
        {
            PlayerStatsBar.UpdateUI();
        };

        playerStatsBar.Init();
        return playerObj;
    }

    public void StartCameraTracking(CharacterObject characterObject, bool forceMove=false)
    {
        CameraSystem.Instance.StartTracking(StageId, characterObject, forceMove); // 카메라가 캐릭터를 따라갑니다.

        var character = characterObject.data as Character;
        character.OnDataRemove += (character) => 
        {
            CameraSystem.Instance.StopTracking();   // 카메라가 캐릭터를 따라가는 것을 멈춥니다.
        };
    }

    // 스테이지 정보에 따른 적들을 소환합니다.
    public EnemyObject[] SpawnEnemys()
    {
        var enemyInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Enemy>(nameof(Enemy));
        foreach (var enemyInfo in enemyInfos) enemyInfo.RemoveData();

        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId).ToArray();
        var stageEnemyInfos = stageInfos.Where(x => x.Type == StageEntityInfo.Types.Enemy);

        var enemyObjs = new List<EnemyObject>();
        foreach (var stageEnemyInfo in stageEnemyInfos)
        {
            var enemyObj = GameApplication.Instance.GameController.EnemyController.Spawn<Enemy, EnemyObject>(
                stageEnemyInfo.EntityId, new Vector2(stageEnemyInfo.SpawnPointX, stageEnemyInfo.SpawnPointY), Quaternion.identity
            );
            enemyObjs.Add(enemyObj);
        }

        return enemyObjs.ToArray();
    }

    // 스테이지 백그라운드 음악을 재생합니다.
    public SoundObject PlayBGM()
    {
        var soundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo));
        foreach (var soundInfo in soundInfos) soundInfo.RemoveData();

        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId).ToArray();
        var stageSoundInfos = stageInfos.Where(x => x.Type == StageEntityInfo.Types.Sound);

        var stageSoundInfo = stageSoundInfos.First();
        var soundObj = GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(
            10003, new Vector2(stageSoundInfo.SpawnPointX, stageSoundInfo.SpawnPointY), Quaternion.identity
        );

        return soundObj;
    }
}
