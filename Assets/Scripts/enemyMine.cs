using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMine : MonoBehaviour
{
    [SerializeField]
    private float _speedMine = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("FIRED SPACE MINE");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(
            Vector3.up * _speedMine * Time.deltaTime); 

        if (transform.position.y > 8f) 
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }

}
