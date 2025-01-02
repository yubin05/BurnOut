using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    GameObject stageClearPanel;
    GameObject player;

    GameObject playerUI;
    [SerializeField] public bool playerCanDash;     // determine whether player can dash or not

    void Awake()
    {
        stageClearPanel = GameObject.FindWithTag("StageClearPanel");
        //stageClearPanel.SetActive(false);     // this method execute by stageClearPanel script
        player = GameObject.FindWithTag("Player");

        playerUI = GameObject.FindWithTag("PlayerUI");
        playerUI.GetComponentInChildren<ThrowCoolTimeUIController>().InitalizeCoolTime();
        if (playerCanDash) 
        { 
            player.transform.Find("Skill_Dash").gameObject.SetActive(true);
            playerUI.transform.Find("CoolTimeUI_Dash").gameObject.SetActive(true);
        }
        else 
        { 
            player.transform.Find("Skill_Dash").gameObject.SetActive(false);
            playerUI.transform.Find("CoolTimeUI_Dash").gameObject.SetActive(false);
        }
    }

    // this method called by Boss.cs
    public void ActiveStageClearPanel()
    {
        stageClearPanel.SetActive(true);
        player.SetActive(false);
        playerUI.SetActive(false);
    }
}
