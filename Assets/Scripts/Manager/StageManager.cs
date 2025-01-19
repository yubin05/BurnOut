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

    [SerializeField] private SkillBar playerSkillBar;   // 스킬 정보 등을 담은 UI바
    public SkillBar PlayerSkillBar => playerSkillBar;

    [SerializeField] private Canvas dynamicOverlayCanvas;  // 동적 오버레이 캔버스
    public Canvas DynamicOverlayCanvas => dynamicOverlayCanvas;

    [SerializeField] private ContinuePanel continuePanel;   // 캐릭터 사망 시 뜨는 부활 메시지 패널
    public ContinuePanel ContinuePanel => continuePanel;

    private Vector3 lastPlayerPos;  // 플레이어가 마지막으로 죽은 위치
    private Coroutine cSpawnEnemy = null;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SpawnPlayer(false);
        SpawnEnemy(true);
        PlayBGM();
    }

    // 플레이어 캐릭터를 소환합니다.
    public void SpawnPlayer(bool isRespawn)
    {
        var playerObj = SpawnPlayerProcess(isRespawn);
        StartCameraTracking(playerObj, true);
        ContinuePanel.OnHide();
    }
    // 플레이어 캐릭터를 소환 프로세스
    private PlayerObject SpawnPlayerProcess(bool isRespawn)
    {
        var playerInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Player>(nameof(Player));
        foreach (var playerInfo in playerInfos) playerInfo.RemoveData();

        var stageEntityInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId);
        var stagePlayerInfos = stageEntityInfos.Where(x => x.Type == StageEntityInfo.Types.Player);
        var stagePlayerInfo = stagePlayerInfos.First();

        PlayerObject playerObj = null;
        if (isRespawn)
        {
            playerObj = GameApplication.Instance.GameController.PlayerController.ReSpawn<Player, PlayerObject>(
                stagePlayerInfo.EntityId, new Vector2(lastPlayerPos.x, lastPlayerPos.y), Quaternion.identity
            );
        }
        else
        {
            playerObj = GameApplication.Instance.GameController.PlayerController.Spawn<Player, PlayerObject>(
                stagePlayerInfo.EntityId, new Vector2(stagePlayerInfo.SpawnPointX, stagePlayerInfo.SpawnPointY), Quaternion.identity
            );
        }        

        var player = playerObj.data as Player;
        player.OnChangeCurrentHp += (updateHp) => 
        {
            PlayerStatsBar.UpdateUI();
        };
        player.OnChangeCurrentMp += (updateMp) => 
        {
            PlayerStatsBar.UpdateUI();
        };

        player.OnDataRemove += (player) => 
        {
            lastPlayerPos = playerObj.transform.position;
            ContinuePanel.Init();
        };

        playerStatsBar.Init();
        playerSkillBar.Init();
        return playerObj;
    }

    // 카메라 추적을 시작합니다.
    public void StartCameraTracking(CharacterObject characterObject, bool forceMove=false)
    {
        CameraSystem.Instance.StartTracking(StageId, characterObject, forceMove); // 카메라가 캐릭터를 따라갑니다.

        var character = characterObject.data as Character;
        character.OnDataRemove += (character) => 
        {
            CameraSystem.Instance.StopTracking();   // 카메라가 캐릭터를 따라가는 것을 멈춥니다.
        };
    }

    public EnemyObject[] SpawnEnemy(bool isRespawn)
    {
        var stageInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageInfo>(nameof(StageInfo)).Where(x => x.Id == stageId);
        var stageInfo = stageInfos.First();

        if (isRespawn) StartRespawnEnemy(stageInfo.EnemySpawnTime);
        return SpawnEnemyProcess();
    }
    // 스테이지 정보에 따른 적들을 소환합니다.
    public EnemyObject[] SpawnEnemyProcess()
    {
        var enemyInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Enemy>(nameof(Enemy));
        foreach (var enemyInfo in enemyInfos) enemyInfo.RemoveData();

        var stageEntityInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId).ToArray();
        var stageEnemyInfos = stageEntityInfos.Where(x => x.Type == StageEntityInfo.Types.Enemy);

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

    // 적을 재소환합니다.
    public void StartRespawnEnemy(float delayTime)
    {
        if (cSpawnEnemy != null) StopCoroutine(cSpawnEnemy);
        StartCoroutine(RespawnEnemyProcess(delayTime));
    }
    private IEnumerator RespawnEnemyProcess(float delayTime)
    {
        var stageEntityInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId).ToArray();
        var stageEnemyInfos = stageEntityInfos.Where(x => x.Type == StageEntityInfo.Types.Enemy).ToArray();
        var length = stageEnemyInfos.Length;

        WaitForSeconds delay = new WaitForSeconds(delayTime);
        while (true)
        {
            yield return delay;

            // 현재 적 수가 스테이지 CSV 데이터 개수보다 적을 경우, CSV 데이터 랜덤 하나 가져와서 소환
            if (GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<Enemy>(nameof(Enemy)).Length < length)
            {
                var stageEnemyInfo = stageEnemyInfos[UnityEngine.Random.Range(0, length)];

                var enemyObj = GameApplication.Instance.GameController.EnemyController.ReSpawn<Enemy, EnemyObject>(
                    stageEnemyInfo.EntityId, new Vector2(stageEnemyInfo.SpawnPointX, stageEnemyInfo.SpawnPointY), Quaternion.identity
                );
            }            
        }
    }
    public void StopRespawnEnemy()
    {
        if (cSpawnEnemy != null) StopCoroutine(cSpawnEnemy);
    }

    // 스테이지 백그라운드 음악을 재생합니다.
    public SoundObject PlayBGM()
    {
        var soundInfos = GameApplication.Instance.GameModel.RuntimeData.ReturnDatas<SoundInfo>(nameof(SoundInfo));
        foreach (var soundInfo in soundInfos) soundInfo.RemoveData();

        var stageEntityInfos = GameApplication.Instance.GameModel.PresetData.ReturnDatas<StageEntityInfo>(nameof(StageEntityInfo)).Where(x => x.StageId == stageId).ToArray();
        var stageSoundInfos = stageEntityInfos.Where(x => x.Type == StageEntityInfo.Types.Sound);

        var stageSoundInfo = stageSoundInfos.First();
        var soundObj = GameApplication.Instance.GameController.SoundController.Spawn<SoundInfo, SoundObject>(
            10003, new Vector2(stageSoundInfo.SpawnPointX, stageSoundInfo.SpawnPointY), Quaternion.identity
        );

        return soundObj;
    }
}
