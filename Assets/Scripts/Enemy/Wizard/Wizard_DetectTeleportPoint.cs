using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_DetectTeleportPoint : MonoBehaviour
{
    List<Transform> teleport_points = new List<Transform>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Teleport_Point")
        {
            //Debug.LogFormat("{0}의 {1} 찾음", transform.parent.name, collision.gameObject.name);

            teleport_points.Add(collision.gameObject.transform);

            transform.parent.GetComponent<Wizard>().Teleport(teleport_points);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Teleport_Point")
        {
            //Debug.LogFormat("{0}의 {1} 벗어남", transform.parent.name, collision.gameObject.name);

            teleport_points.Remove(collision.gameObject.transform);
        }
    }

    //public List<Transform> Get_Teleport_Points()
    //{
    //    return teleport_points;
    //}
}
