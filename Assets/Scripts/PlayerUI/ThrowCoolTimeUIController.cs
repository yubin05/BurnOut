using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCoolTimeUIController : CoolTimeUIController
{
    // this method triggered when next stage start
    // this method called by other script
    public void InitalizeCoolTime()
    {
        player_script.canThrow = true;
        cooltimeImage.fillAmount = 1f;
    }
}
