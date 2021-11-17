using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRadar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            //take evasive action
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
