using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;

    private AudioSource _audioSource;

    private bool _stopFiring = false;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    private Vector3 direction = Vector3.down;

    private string enemyType;

    private bool BlackHoleIsOnNow = false;

    private int _movementType;

    [SerializeField]
    private float _amplitude = 1;
    [SerializeField]
    private float _frequency = 1;

    private bool pastHalf = false;
    private float circleFlip;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _movementType = UnityEngine.Random.Range(0, 2);
        circleFlip = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log("circleFlip = " + circleFlip);

        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }

        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.Log("The Animator is NULL");
        }

        if (Player.BlackHoleIsOn == true)
        {
            _speed = .5f;
            direction = new Vector3(0, 3, 0) - transform.position;
        }
    }

    private void Awake()
    {
        Player.onBlackHoleAction += BlackHole;
    }

    public void BlackHole()
    {
        Debug.Log("BlackHole was called");
        if (BlackHoleIsOnNow == false)
        {
            BlackHoleIsOnNow = true;
        }
        else 
        {
            BlackHoleIsOnNow = false;
        }
    }

    private void OnDestroy()
    {
        Player.onBlackHoleAction -= BlackHole;
    }

    void Update()
    {

        Calculatemovement();

        if (Time.time > _canFire)
        {
            _fireRate = UnityEngine.Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(
                _enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
                lasers[i].tag = "EnemyLaser";
            }
        }
    }

    void Calculatemovement()
    {

        if (transform.position.y < 0)
        {
            pastHalf = true;
        }

        if (BlackHoleIsOnNow == true)
        {
            _speed = .5f;
            direction = new Vector3(0, 3, 0) - transform.position;
        }
        else
        {
            switch (_movementType)
            {
                case 0: // zig zag
                    direction = new Vector3(Mathf.Cos(Time.time * 3), -1, 0);
                    break;
                case 1: // circle
                    if (pastHalf == true)
                    {
                        if (circleFlip > .5f)
                        {
                            direction = new Vector3(Mathf.Cos(Time.time * _frequency) * _amplitude, Mathf.Sin(Time.time * _frequency) * _amplitude, 0);
                        }
                        else
                        {
                            direction = new Vector3((Mathf.Sin(Time.time * _frequency) * _amplitude), (Mathf.Cos(Time.time * _frequency) * _amplitude), 0);
                        }
                    }
                    else
                    {
                        direction = Vector3.down;
                    }
                    break;
                case 2: // down
                    direction = Vector3.down;
                    break;
            }
        }

        transform.Translate(direction * _speed * Time.deltaTime);
        if (_movementType == 1)
        {
            if (pastHalf == false)
            {
                if (transform.position.y < -3.8f)
                {
                    float randomX = UnityEngine.Random.Range(-8f, 9f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }
                else if (transform.position.x > 10f)
                {
                    float randomY = UnityEngine.Random.Range(0f, 6f);
                    transform.position = new Vector3(-10, randomY, 0);
                }
                else if (transform.position.x < -10f)
                {
                    float randomY = UnityEngine.Random.Range(0f, 6f);
                    transform.position = new Vector3(10, randomY, 0);
                }
            }
            else if (transform.position.x > 10f)
            {
                transform.position = new Vector3(-10, transform.position.y, 0);
            }
            else if (transform.position.x < -10f)
            {
                transform.position = new Vector3(10, transform.position.y, 0);
            }
        }
        else
        {
            if (transform.position.y < -3.8f)
            {
                float randomX = UnityEngine.Random.Range(-8f, 9f);
                transform.position = new Vector3(randomX, 7f, 0);
            }
            else if (transform.position.x > 10f)
            {
                float randomY = UnityEngine.Random.Range(0f, 6f);
                transform.position = new Vector3(-10, randomY, 0);
            }
            else if (transform.position.x < -10f)
            {
                float randomY = UnityEngine.Random.Range(0f, 6f);
                transform.position = new Vector3(10, randomY, 0);
            }
        }

    }

    private void FireEnemyLaser()
    {
        _canFire = Time.time + _fireRate;

        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Hit: " + other.transform.name);

        Player player = other.transform.GetComponent<Player>();

        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }

            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }

        if (other.tag == "BlackHole")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

        }

    }
}
