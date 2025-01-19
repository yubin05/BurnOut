using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 모든 게임 컨트롤러를 모아놓은 클래스
/// </summary>
public class GameController
{
    public SoundController SoundController { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public EnemyController EnemyController { get; private set; }
    public HeadBarController HeadBarController { get; private set; }
    public DamageFontController DamageFontController { get; private set; }
    public VFXController VFXController { get; private set; }

    public void Init(GameModel gameModel)
    {
        SoundController = new SoundController(gameModel);
        PlayerController = new PlayerController(gameModel);
        EnemyController = new EnemyController(gameModel);
        HeadBarController = new HeadBarController(gameModel);
        DamageFontController = new DamageFontController(gameModel);
        VFXController = new VFXController(gameModel);
    }
}

/// <summary>
/// 모든 컨트롤러의 베이스(부모)가 되는 클래스
/// </summary>
public abstract class BaseController
{
    protected GameModel GameModel;

    public BaseController(GameModel gameModel)
    {
        GameModel = gameModel;
    }

    public virtual K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent=null) where T : Data where K : PoolObject
    {
        T data = GameModel.PresetData.ReturnData<T>(typeof(T).Name, id).Clone() as T;
        GameModel.RuntimeData.AddData(typeof(T).Name, data);    // 데이터 추가

        K obj = Pooling.Instance.CreatePoolObject<K>(id);   // 오브젝트 풀링 가져오기
        obj.transform.SetParent(parent);
        if (parent == null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        else
        {
            obj.transform.localPosition = position;
            obj.transform.localRotation = rotation;
        }
        obj.Init(data);

        data.OnDataRemove = null;
        data.OnDataRemove += (_data) => 
        {
            GameModel.RuntimeData.RemoveData(typeof(T).Name, _data);    // 데이터 삭제
            Pooling.Instance.ReturnPoolObject(id, obj); // 오브젝트 풀링 반납
        };
        
        return obj;
    }
}

/// <summary>
/// 사운드 관련 컨트롤러 (사운드 오브젝트 소환 등)
/// </summary>
public class SoundController : BaseController
{
    public SoundController(GameModel gameModel) : base(gameModel)
    {
    }

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var soundObj = base.Spawn<T, K>(id, position, rotation, parent);
        var sound = soundObj.data as SoundInfo;

        switch (sound.Type)
        {
            case SoundInfo.Types.BGM: default: (soundObj as SoundObject).AudioSource.volume = GameModel.ClientData.PlayerSound.bgm; break;
            case SoundInfo.Types.SFX: (soundObj as SoundObject).AudioSource.volume = GameModel.ClientData.PlayerSound.sfx; break;
        }

        return soundObj;
    }

    public void ChangeBGMVolume(float volume) => GameManager.Instance.ChangeBGMVolume(volume);
    public void ChangeSFXVolume(float volume) => GameManager.Instance.ChangeSFXVolume(volume);
}

/// <summary>
/// 캐릭터 관련 컨트롤러 (캐릭터 오브젝트 소환 등)
/// </summary>
public class CharacterController : BaseController
{
    public CharacterController(GameModel gameModel) : base(gameModel)
    {
    }

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var characterObj = base.Spawn<T, K>(id, position, rotation, parent);
        var character = characterObj.data as Character;

        character.BasicStat = GameModel.PresetData.ReturnData<CharacterStat>(nameof(CharacterStat), id).Clone() as CharacterStat;
        character.CurrentHp = character.BasicStat.MaxHp;
        character.CurrentMp = character.BasicStat.MaxMp;

        // 캐릭터 마나 자동 충전
        character.StartChargeMp();

        return characterObj;
    }

    // 죽은 후, 다시 소환할때 사용
    public virtual K ReSpawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null) where T : Character where K : CharacterObject
    {
        var characterObj = Spawn<T, K>(id, position, rotation, parent);
        var character = characterObj.data as Character;

        // 다시 부활할 때는 잠깐 동안 무적 부여
        characterObj.ImmunitySystem.StartImmunity(character.BasicStat.RespawnImmunityTime);

        return characterObj;
    }
}
/// <summary>
/// 플레이어 관련 컨트롤러(플레이어 오브젝트 소환 등)
/// </summary>
public class PlayerController : CharacterController
{
    public PlayerController(GameModel gameModel) : base(gameModel)
    {
    }

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var playerObj = base.Spawn<T, K>(id, position, rotation, parent);
        var player = playerObj.data as Player;

        player.AttackTargets = LayerMask.GetMask(nameof(Enemy));

        return playerObj;
    }
}
/// <summary>
/// 적 관련 컨트롤러(적 오브젝트 소환 등)
/// </summary>
public class EnemyController : CharacterController
{
    public EnemyController(GameModel gameModel) : base(gameModel)
    {
    }

    public override K Spawn<T, K>(int id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var enemyObj = base.Spawn<T, K>(id, position, rotation, parent);
        var enemy = enemyObj.data as Enemy;
        
        enemy.AttackTargets = LayerMask.GetMask(nameof(Player));

        // 캐릭터 데이터가 사라지면 캐릭터가 가지고 있던 헤드바 데이터도 삭제해줘야 함
        enemy.OnDataRemove += (data) => 
        {
            var headBarObj = (enemyObj as CharacterObject).HeadBarObject;
            var headBar = headBarObj.data as HeadBar;
            headBar.RemoveData();
        };

        return enemyObj;
    }
}

/// <summary>
/// 헤드바 관련 컨트롤러(헤드바 오브젝트 소환 등)
/// </summary>
public class HeadBarController : BaseController
{
    public HeadBarController(GameModel gameModel) : base(gameModel) {}

    public K Spawn<T, K>(CharacterObject characterObject, int id) where T : HeadBar where K : HeadBarObject
    {
        var headBarObj = Spawn<T, K>(id, CameraSystem.Instance.TargetCamera.WorldToScreenPoint(characterObject.HeadBarNode.position), Quaternion.identity, StageManager.Instance.DynamicOverlayCanvas.transform);
        var headBar = headBarObj.data as HeadBar;

        headBar.TargetObj = characterObject;
        headBarObj.transform.SetSiblingIndex(StageManager.Instance.PlayerStatsBar.transform.GetSiblingIndex()-1);

        // 헤드바 데이터가 사라지면 대상이 가지고 있는 헤드바 오브젝트 데이터 null 처리
        headBar.OnDataRemove += (data) => 
        {
            characterObject.HeadBarObject = null;
        };

        return headBarObj;
    }
}

/// <summary>
/// 데미지 폰트 관련 컨트롤러(데미지 폰트 오브젝트 소환 등)
/// </summary>
public class DamageFontController : BaseController
{
    public DamageFontController(GameModel gameModel) : base(gameModel) {}

    public K Spawn<T, K>(CharacterObject characterObject, int id, int hitDamage) where T : DamageFont where K : DamageFontObject
    {
        var damageFontObj = Spawn<T, K>(id, CameraSystem.Instance.TargetCamera.WorldToScreenPoint(characterObject.HeadBarNode.position), Quaternion.identity, StageManager.Instance.DynamicOverlayCanvas.transform);
        var damageFont = damageFontObj.data as DamageFont;

        damageFont.HitDamage = hitDamage;
        damageFontObj.SetDamageTxt();

        damageFont.InitWorldPositon = characterObject.HeadBarNode.position;
        var pos = damageFont.InitWorldPositon; pos.y += damageFont.MoveWorldPosY;
        damageFont.EndWorldPosition = pos;
        
        damageFontObj.transform.SetAsFirstSibling();

        return damageFontObj;
    }
}

/// <summary>
/// VFX 관련 컨트롤러(VFX 오브젝트 소환 등)
/// </summary>
public class VFXController : BaseController
{
    public VFXController(GameModel gameModel) : base(gameModel) {}
}