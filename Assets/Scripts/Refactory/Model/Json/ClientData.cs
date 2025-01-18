using System;
using System.Collections.Generic;

/// <summary>
/// 클라이언트에 저장하고 있는 데이터
/// </summary>
public class ClientData : Data
{
    public string PlayerLanguageFileName => "PlayerLanguage";
    public PlayerLanguage PlayerLanguage;

    public string PlayerSoundFileName => "PlayerSound";
    public PlayerSound PlayerSound;

    public string PlayerKeyCodeFileName => "PlayerKeyCode";
    public PlayerKeyCodes PlayerKeyCodes;
}
