using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(
            Vector3.down * _speed * Time.deltaTime); //Vector3.up if player is behind enemy

        if (transform.position.y < -8f) //or > something
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
