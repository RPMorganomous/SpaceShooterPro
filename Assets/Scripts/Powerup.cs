using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;

    [SerializeField] //0=TripleShot 1=Speed 2=Shields 3=LaserRecharge
    private int powerupID;

    [SerializeField]
    private AudioClip _clip;

    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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
                        player.HealthPowerup();
                        break;
                }

            }

            Destroy(this.gameObject);

        }

    }

}
