using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_FireBall_Controller : MonoBehaviour
{
    [SerializeField]
    public GameObject fireball;

    // called by Wizard.cs
    public void Instantiate_Fireball(Wizard wizardScript)
    {
        GameObject fireballObj =
            Instantiate(fireball, new Vector2(transform.position.x, transform.position.y + 0.8f), transform.rotation, FindObjectOfType<PoolManager>().transform);
        fireballObj.GetComponent<Wizard_FireBall>().wizard_script = wizardScript;
    }
}
