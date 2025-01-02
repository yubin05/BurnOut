using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Bandit : AudioManager
{
    public AudioClip audioJump;
    public AudioClip audioHurt;
    public AudioClip audioAttack;
    public AudioClip audioDeath;
    public AudioClip audioWalk;

    protected override AudioClip checkAudioName(string audioName)
    {
        AudioClip audioClip;
        if (audioName == "Jump") { audioClip = audioJump; }
        else if (audioName == "Hurt") { audioClip = audioHurt; }
        else if (audioName == "Attack") { audioClip = audioAttack; }
        else if (audioName == "Death") { audioClip = audioDeath; }
        else if (audioName == "Walk") { audioClip = audioWalk; }
        else
        {
            audioClip = null;
            Debug.LogError("Can't find audio clip");
        }

        return audioClip;
    }
}
