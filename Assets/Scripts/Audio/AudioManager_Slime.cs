using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Slime : AudioManager
{    
    public AudioClip audioHurt; 
    public AudioClip audioDeath;

    protected override AudioClip checkAudioName(string audioName)
    {
        AudioClip audioClip;
        if (audioName == "Hurt") { audioClip = audioHurt; }
        else if (audioName == "Death") { audioClip = audioDeath; }
        else
        {
            audioClip = null;
            Debug.LogError("Can't find audio clip");
        }

        return audioClip;
    }
}
