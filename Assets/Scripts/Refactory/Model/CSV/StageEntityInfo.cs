using System;

/// <summary>
/// 스테이지 소환 엔티티 정보
/// </summary>
public class StageEntityInfo : Data
{
    public enum Types { Player, Enemy, Sound }

    public int StageId { get; set; } // 스테이지 ID
    public int EntityId { get; set; } // 소환할 개체 ID
    public Types Type { get; set; } // 소환할 개체 타입
    public float SpawnPointX { get; set; }   // 소환할 캐릭터 위치의 x좌표
    public float SpawnPointY { get; set; }   // 소환할 캐릭터 위치의 y좌표
}
