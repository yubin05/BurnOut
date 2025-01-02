using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectController : MonoBehaviour
{
    public GameObject targetGameObject;

    // Audio Manager
    GameObject audio_;
    AudioManager audioManager;

    // this variable controled by other scripts
    public float moveSpeed;
    public float throw_delay;

    // UI Controller
    CoolTimeUIController coolTimeController;

    private void Start()
    {
        // get audio manager
        audio_ = GameObject.Find("AudioManager_Player");
        audioManager = audio_.GetComponent<AudioManager_Player>();

        coolTimeController = GameObject.FindWithTag("PlayerUI").transform.Find("CoolTimeUI_Throw").transform.Find("foreground_image").
            GetComponent<CoolTimeUIController>();
    }

    // this method called by PlayerController.cs
    public void InstantiateClone(bool flipX)
    {
        // Instantiate
        GameObject newObject = Instantiate(targetGameObject, transform.position, transform.rotation);
        if (flipX) { newObject.transform.localScale *= -1; }

        // cooltime UI control
        StartCoroutine(coolTimeController.OnCoolTime(throw_delay));

        // sound play
        audioManager.PlayOneShotAudio("Throw");
    }
}
