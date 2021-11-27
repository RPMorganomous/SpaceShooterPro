using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRadar : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().DodgeLaser(true, other.transform.position.x);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            transform.parent.GetComponent<Enemy>().DodgeLaser(false, 0);
        }
    }
}
