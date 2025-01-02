using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    GameObject player;
    PlayerController player_script;
    int player_inital_sword_attack_power;
    int player_inital_throw_attack_power;
    int player_inital_health;
    int player_inital_max_health;

    GameObject playerUI;
    GameObject gameOverUI;
    GameObject boss_healthUI;

    [HideInInspector] public bool isGameOver;
    float gameOverUIFadeInTime;

    int screenWidth = 1920;  int screenHeight = 1080;

    private void Awake()
    {
        playerUI = GameObject.FindWithTag("PlayerUI");
        gameOverUI = GameObject.FindWithTag("GameOverUI");
    }

    private void Start()
    {
        Screen.SetResolution(screenWidth, screenHeight, true);
        isGameOver = false;

        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<PlayerController>();

        player_script.InitOnStart();
        
        // player stat initalization for restarting
        player_inital_sword_attack_power = player_script.player_sword_attack_power;
        player_inital_throw_attack_power = player_script.player_throw_attack_power;
        player_inital_health = player_script.health;
        player_inital_max_health = player_script.max_health;

        player.SetActive(false);
        playerUI.SetActive(false);

        isGameOver = false;
        gameOverUIFadeInTime = 3f;
    }

    // this method called by PlayerController.cs
    public void GameOver()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);     // self scene reload
        ActiveGameOverPanel();  // isGameOver=true by controled PlayerController.cs
        player.SetActive(false); playerUI.SetActive(false);

        // Boss Health UI inactive
        try { GameObject.FindWithTag("Boss").transform.Find("Boss_HealthUI").gameObject.SetActive(false); }
        catch (Exception) { }
    }

    void ActiveGameOverPanel()
    {
        gameOverUI.SetActive(true);

        Image[] gameOverUI_images = gameOverUI.GetComponentsInChildren<Image>();
        foreach (Image gameOverUI_image in gameOverUI_images)
        {
            Color gameOverUI_imageColor = gameOverUI_image.color;
            gameOverUI_imageColor.a = 0f;
            gameOverUI_image.color = gameOverUI_imageColor;
        }
        Text[] gameOverUI_texts = gameOverUI.GetComponentsInChildren<Text>();
        foreach (Text gameOverUI_text in gameOverUI_texts)
        {
            Color gameOverUI_textColor = gameOverUI_text.color;
            gameOverUI_textColor.a = 0f;
            gameOverUI_text.color = gameOverUI_textColor;
        }

        // fade in 효과
        //Image[] gameOverUI_images = gameOverUI.GetComponentsInChildren<Image>();
        foreach (Image gameOverUI_image in gameOverUI_images) gameOverUI_image.DOFade(1, gameOverUIFadeInTime);
        //Text[] gameOverUI_texts = gameOverUI.GetComponentsInChildren<Text>();
        foreach (Text gameOverUI_text in gameOverUI_texts) gameOverUI_text.DOFade(1, gameOverUIFadeInTime);
    }

    // this method called by Boss.cs
    public void ActiveStageClearPanelController(GameObject stageManager)
    {
        if (!isGameOver) { stageManager.GetComponent<StageManager>().ActiveStageClearPanel(); }
    }

    // this method called by GameOver.cs
    public void RevivePlayer()
    {
        player.SetActive(true); playerUI.SetActive(true);

        player_script.player_sword_attack_power = player_inital_sword_attack_power;
        player_script.player_throw_attack_power = player_inital_throw_attack_power;
        player_script.health = player_inital_health;
        player_script.max_health = player_inital_max_health;

        player_script.canInput = true;
        player.GetComponentInChildren<PlayerHealthUI>().setHealthUI(player_inital_health);
        player.GetComponentInChildren<PlayerHealthUI>().setMaxHealthUI(player_inital_max_health);
        player_script.Vincible();

        isGameOver = false;     // 게임 오버 플래그 해제 - 재시작 시, 게임매니저는 초기화 되지 않기 때문
    }

    // this method called by other scripts in need of loading main menu scene
    // because prevent overlap DontDestroyOnLoad object
    public void LoadMainMenuScene()
    {
        isGameOver = false;     // 게임 오버 플래그 해제 - 재시작 시, 게임매니저는 초기화 되지 않기 때문

        Destroy(gameOverUI); Destroy(playerUI);
        SceneManager.LoadScene(0);
        Destroy(player); Destroy(gameObject);
    }
}
