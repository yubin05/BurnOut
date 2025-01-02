using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject gameManager;

    private GameObject player;
    private GameObject playerUI;
    private GameObject gameOverUI;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerUI = GameObject.FindWithTag("PlayerUI");
    }

    private void Start()
    {
        gameOverUI = GameObject.FindWithTag("GameOverUI");
        gameOverUI.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        player.SetActive(true);
        playerUI.SetActive(true);
        DontDestroyOnLoads();
    }
    void DontDestroyOnLoads()
    {
        DontDestroyOnLoad(playerUI);
        DontDestroyOnLoad(gameManager);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(gameOverUI);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
