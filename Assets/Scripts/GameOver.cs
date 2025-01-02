using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Restart()
    {
        transform.root.gameObject.SetActive(false);
        gameManager.RevivePlayer();
        SceneManager.LoadScene(1);  // load Stage1
    }

    public void BackToMainMenu()
    {
        transform.root.gameObject.SetActive(false);
        gameManager.LoadMainMenuScene();  // load MainMenu
    }

    public void Quit()
    {
        Application.Quit();
    }
}
