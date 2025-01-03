using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 데이터 + 게임 컨트톨러 등을 관리할 클래스
public class GameApplication : GlobalSingleton<GameApplication>
{
    public GameModel GameModel { get; private set; } = new GameModel();
    public void Init()
    {
        GameModel.Init();
    }
}
