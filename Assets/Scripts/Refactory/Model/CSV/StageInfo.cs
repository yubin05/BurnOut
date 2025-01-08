using System;

public class StageInfo : Data
{
    public int StageId { get; set; } // 스테이지 ID
    public int CharacterId { get; set; } // 소환할 캐릭터 ID
    public float SpawnPointX { get; set; }   // 소환할 캐릭터 위치의 x좌표
    public float SpawnPointY { get; set; }   // 소환할 캐릭터 위치의 y좌표
}
