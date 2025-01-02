using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Rino : AudioManager
{
    public AudioClip audioHurt;
    public AudioClip audioDeath;
    public AudioClip audioRun;

    protected override AudioClip checkAudioName(string audioName)
    {
        AudioClip audioClip;
        if (audioName == "Hurt") { audioClip = audioHurt; }
        else if (audioName == "Death") { audioClip = audioDeath; }
        else if (audioName == "Run") { audioClip = audioRun; }
        else
        {
            audioClip = null;
            Debug.LogError("Can't find audio clip");
        }

        return audioClip;
    }
}
