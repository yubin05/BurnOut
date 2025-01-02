using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_DetectPlayer : MonoBehaviour
{
    GameObject wizard;
    Wizard wizard_script;

    private void Start()
    {
        wizard = transform.parent.gameObject;
        wizard_script = wizard.GetComponent<Wizard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            //gameManager_script.SetDetectTeleportPointTrigger(transform.parent.name, true);

            wizard_script.canAttack = true;

            transform.parent.Find("Detect_Teleport_Point").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //gameManager_script.SetDetectTeleportPointTrigger(transform.parent.name, false);

            wizard_script.canAttack = false;

            transform.parent.Find("Detect_Teleport_Point").gameObject.SetActive(false);
        }
    }
}
