using System;
using System.Collections.Generic;

public class PlayerKeyCodes : Data
{
    public List<PlayerKeyCode> playerKeyCodes;
}
public class PlayerKeyCode : Data
{
    public int KeyCodeId { get; set; }
}