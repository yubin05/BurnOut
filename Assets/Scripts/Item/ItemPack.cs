using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPack : MonoBehaviour
{
    [SerializeField] public int addValue;

    protected AudioSource audioSource;
    [SerializeField] public AudioClip playAudioClip;

    protected bool bGetItem;
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bGetItem = false;   // 코루틴 딜레이로 인한 중복 획득 방지하기 위한 변수
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (!bGetItem && collider.gameObject.tag == "Player")
        {
            bGetItem = true;

            GameObject player = collider.gameObject;
            PlayerController player_script = player.GetComponent<PlayerController>();

            // actually item implement
            TriggeredItem(player, player_script);
            EatItem();  // player get this item
        }
    }

    protected abstract void TriggeredItem(GameObject player, PlayerController player_script);

    protected virtual void EatItem()
    {
        gameObject.layer = 17;  // Treat it as an item you ate
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        
        PlaySound();
        StartCoroutine(DelayDestroy());
    }

    protected virtual void PlaySound() 
    {
        audioSource.clip = playAudioClip;
        audioSource.Play();
    }

    protected IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(playAudioClip.length);
        Destroy(gameObject);
    }
}
