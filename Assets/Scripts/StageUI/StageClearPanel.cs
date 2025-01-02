using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearPanel : MonoBehaviour
{
    protected GameObject player;
    protected GameObject playerUI;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerUI = GameObject.FindWithTag("PlayerUI");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            player.SetActive(true); playerUI.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
