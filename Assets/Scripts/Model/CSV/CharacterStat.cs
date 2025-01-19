using System;

// 캐릭터 스탯 클래스
public class CharacterStat : Data
{
    public int MaxHp { get; set; }
    public int MaxMp { get; set; }
    public float MoveSpeed { get; set; }
    public int AttackDamage { get; set; }
    public int AbilityPower { get; set; }
    public int AttackSpeed { get; set; }
    public float JumpPower { get; set; }
    public float StaggerDuration { get; set; }
    public float RespawnImmunityTime { get; set; }
    public float MpChargeSecond { get; set; }
    public int MpChargeValue { get; set; }
}
