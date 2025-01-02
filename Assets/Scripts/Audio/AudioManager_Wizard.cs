using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Wizard : AudioManager
{
    [SerializeField]
    public AudioClip audioHurt;
    public AudioClip audioTeleport;
    public AudioClip audioExplosion;    // Death
    public AudioClip audioFireBall;     // Attack

    protected override AudioClip checkAudioName(string audioName)
    {
        AudioClip audioClip;
        if (audioName == "Hurt") { audioClip = audioHurt; }
        else if (audioName == "Explosion") { audioClip = audioExplosion; }
        else if (audioName == "Teleport") { audioClip = audioTeleport; }
        else if (audioName == "FireBall") { audioClip = audioFireBall; }
        else
        {
            audioClip = null;
            Debug.LogError("Can't find audio clip");
        }

        return audioClip;
    }
}
