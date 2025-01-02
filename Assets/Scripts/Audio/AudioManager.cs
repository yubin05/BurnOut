using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    protected AudioSource audioSource;
    protected static AudioClip playAudioClip;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playAudioClip = null;
    }

    protected virtual AudioClip checkAudioName(string audioName)
    {
        // this method require to override in subclass
        return null;
    }

    public virtual void PlayAudio(string audioName, float volume=1f, float pitch=1f)
    {
        playAudioClip = checkAudioName(audioName);
        audioSource.clip = playAudioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        playAudioClip = null;
    }

    public virtual void PlayOneShotAudio(string audioName, float volume=1f, float pitch=1f)
    {
        playAudioClip = checkAudioName(audioName);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(playAudioClip, volume);
        playAudioClip = null;

        // revert volume
        audioSource.volume = 1f;
    }
}
