using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDoor : MonoBehaviour
{
    [SerializeField] GameObject[] triggerObjs;   // 이 오브젝트들이 모두 파괴되거나 비활성화면 로직 발동

    AudioSource audioSource;
    [SerializeField] AudioClip destroyAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();        
    }

    void Start()
    {
        InvokeRepeating("CheckDestroyObjs", 0, 1);
    }

    void CheckDestroyObjs()
    {
        foreach (GameObject triggerObj in triggerObjs)
        {
            if (!(triggerObj == null || triggerObj.activeSelf == false)) return;
        }

        audioSource.clip = destroyAudioClip;
        audioSource.Play();
        // 문 파괴
        Invoke("Destroy", 0.5f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
