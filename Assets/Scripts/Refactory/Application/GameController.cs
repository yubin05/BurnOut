using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 게임 컨트롤러를 모아놓은 클래스
public class GameController
{
    public SoundController SoundController { get; private set; }

    public void Init(GameModel gameModel)
    {
        SoundController = new SoundController(gameModel);
    }
}

public class BaseController
{
    protected GameModel GameModel;

    public BaseController(GameModel gameModel)
    {
        GameModel = gameModel;
    }

    public virtual K Spawn<T, K>(int id, Transform parent=null) where T : Data where K : PoolObject
    {
        T data = GameModel.PresetData.ReturnData<T>(typeof(T).Name, id).Clone() as T;
        GameModel.RuntimeData.AddData(typeof(T).Name, data);

        K obj = Pooling.Instance.CreatePoolObject<K>(id);
        obj.Init(data);
        obj.transform.parent = parent;

        data.OnDataRemove = null;
        data.OnDataRemove += (_data) => 
        {
            GameModel.RuntimeData.RemoveData(typeof(T).Name, _data);
            Pooling.Instance.ReturnPoolObject(id, obj);
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