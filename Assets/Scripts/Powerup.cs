using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;

    private float _speedC = 3;

    [SerializeField] //0=TripleShot 1=Speed 2=Shields 3=LaserRecharge
    private int powerupID;

    [SerializeField]
    private AudioClip _clip;

    private Player _player;
    private Vector3 direction;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            direction = (_player.transform.position) - transform.position;
            direction = direction.normalized;
            transform.Translate(direction * _speedC * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();

        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.LaserRecharge();
                        break;
                    case 4:
                        Debug.Log("HealthPowerup 1.0");
                        player.HealthPowerup();
                        break;
                    case 5:
                        Debug.Log("blackHoleActive 1.0");
                        player.BlackholePowerup();
                        break;
                    case 6:
                        player.Damage();
                        break;
                }

            }


            Destroy(this.gameObject);

        }
        else
        {
            if (other.CompareTag("EnemyLaser"))
            {
                Destroy(this.gameObject);
            }
        }
    }

}
