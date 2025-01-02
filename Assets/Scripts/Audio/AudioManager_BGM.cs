using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_BGM : AudioManager
{
    [SerializeField] AudioClip playClip;    // 해당 씬에서 재생할 브금

    protected override void Start()
    {
        base.Start();

        audioSource.clip = playClip;

        audioSource.loop = true;
        audioSource.Play();
    }
}
