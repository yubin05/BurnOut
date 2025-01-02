using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkCursor : MonoBehaviour
{
    List<GameObject> Dialogue_Texts = new List<GameObject>();
    int activate_dialogue_index = 0;

    private void Awake()
    {
        Transform dialogue_panel = transform.parent.Find("Dialogue_Panel");
        foreach (Transform dialogue_text in dialogue_panel.transform)
        {
            Dialogue_Texts.Add(dialogue_text.gameObject);
            dialogue_text.gameObject.SetActive(false);
        }
        Dialogue_Texts[activate_dialogue_index].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { NextDialogue(); }        
    }
    void NextDialogue()
    {
        Dialogue_Texts[activate_dialogue_index++].SetActive(false);
        if (activate_dialogue_index >= Dialogue_Texts.Count)
        {
            transform.parent.parent.GetComponent<EntryDialogueCollider>().DestroyDialogueCollider();
        }
        else
        {
            Dialogue_Texts[activate_dialogue_index].SetActive(true);
        }
    }
}
