using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPowerupTargeting : MonoBehaviour
{
    Enemy parentScript;

    void Start()
    {
        parentScript = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            parentScript.FireEnemyLaser();
        }
    }
}
