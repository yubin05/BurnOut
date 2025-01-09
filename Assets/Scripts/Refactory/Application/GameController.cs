using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 게임 컨트롤러를 모아놓은 클래스
public class GameController
{
    public SoundController SoundController { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public EnemyController EnemyController { get; private set; }

    public void Init(GameModel gameModel)
    {
        SoundController = new SoundController(gameModel);
        PlayerController = new PlayerController(gameModel);
        EnemyController = new EnemyController(gameModel);
    }
}

public class BaseController
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
        obj.transform.parent = parent;
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

// 사운드 관련 컨트롤러
// Ex) 사운드 플레이할 때 등에서 사용
public class SoundController : BaseController
{
    public SoundController(GameModel gameModel) : base(gameModel)
    {
    }
}

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

        return characterObj;
    }
}
// 플레이어 관련 컨트롤러
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
// 적 관련 컨트롤러
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

        return enemyObj;
    }
}