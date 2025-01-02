using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Player : AudioManager
{
    [SerializeField] public AudioClip audioWalk;
    [SerializeField] public AudioClip audioRun;
    [SerializeField] public AudioClip audioJump;
    [SerializeField] public AudioClip audioLanding;
    [SerializeField] public AudioClip audioAttack;
    [SerializeField] public AudioClip audioDeath;
    [SerializeField] public AudioClip audioHurt;
    [SerializeField] public AudioClip audioDash;
    [SerializeField] public AudioClip audioThrow;

    protected override AudioClip checkAudioName(string audioName)
    {
        AudioClip audioClip;
        if (audioName == "Walk") { audioClip = audioWalk; }
        else if (audioName == "Run") { audioClip = audioRun; }
        else if (audioName == "Jump") { audioClip = audioJump; }
        else if (audioName == "Landing") { audioClip = audioLanding; }
        else if (audioName == "Attack") { audioClip = audioAttack; }
        else if (audioName == "Death") { audioClip = audioDeath; }
        else if (audioName == "Hurt") { audioClip = audioHurt; }
        else if (audioName == "Dash") { audioClip = audioDash; }
        else if (audioName == "Throw") { audioClip = audioThrow; }
        else
        {
            audioClip = null;
            Debug.LogError("Can't find audio clip");
        }

        return audioClip;
    }
}
