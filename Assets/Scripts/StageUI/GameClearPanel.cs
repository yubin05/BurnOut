using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : StageClearPanel
{
    GameManager gameManager;
    GameObject endingCreditPanel;

    float endingCreditCount;

    private void Awake()
    {
        endingCreditPanel = transform.Find("EndingCreditPanel").gameObject;
    }

    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        // ending credit timer
        endingCreditCount = 0f;
        StartCoroutine(OnActiveTimer());
    }
    IEnumerator OnActiveTimer()
    {
        while (endingCreditCount <= 5f)
        {
            endingCreditCount += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        endingCreditPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (endingCreditCount >= 5f && Input.anyKeyDown)
        {
            gameManager.LoadMainMenuScene();  // back to main menu
        }
    }
}
