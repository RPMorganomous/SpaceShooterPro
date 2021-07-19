using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShot;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _laserHitPlayerSound;

    private AudioSource _audioSource;
    private AudioSource _explosion;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 0.8f, 0);

    [SerializeField]
    private float _fireRate = 0.15f;

    [SerializeField]
    private int _lives = 3;

    private float _canFire = -1.0f;

    private bool _tripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldsActive = false;

    private SpawnManager _spawnManager;

    int lastRunnerTripleShot = 0;
    int lastRunnerSpeedBoost = 0;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown("space") && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
            _audioSource.Play();

        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _audioSource.Play();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput =
              Input.GetAxis("Horizontal");

        float verticalInput =
            Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_speedBoostActive == true)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        

        transform.position
            = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position
                = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position
                = new Vector3(11, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_shieldsActive == true)
        {
            _shieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;  
        }

        _lives--;

        switch (_lives)
        {
            case 2:
                _rightEngine.SetActive(true);
                break;
            case 1:
                _leftEngine.SetActive(true);
                break;
        }

        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

        

    }

    public void TripleShotActive()
    {
        if (_tripleShotActive == false)
        {
            _tripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }
        else
        {
            lastRunnerTripleShot = lastRunnerTripleShot + 1;
            Debug.Log("lastRunner = " + lastRunnerTripleShot);
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        if (lastRunnerTripleShot == 0)
        {
            _tripleShotActive = false;
        }
        else
        {
            lastRunnerTripleShot = lastRunnerTripleShot - 1;
            _tripleShotActive = false;
            Debug.Log("TS OFF");
            TripleShotActive();
        }
    } 

    public void SpeedBoostActive()
    {
        if (_speedBoostActive == false)
        {
            _speedBoostActive = true;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
        else
        {
            lastRunnerSpeedBoost = lastRunnerSpeedBoost + 1;
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        if (lastRunnerSpeedBoost == 0)
        {
            _speedBoostActive = false;
        }
        else
        {
            lastRunnerSpeedBoost = lastRunnerSpeedBoost - 1;
            _speedBoostActive = false;
            SpeedBoostActive();
        }
        
    }

    public void ShieldsActive()
    {
        if (_shieldsActive == false)
        {
            _shieldsActive = true;
            _shieldVisualizer.SetActive(true);
        }
        else
        {
            //could add a shield multiplier here?
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("Hit by " + other.tag);

    //    if (other.tag == "EnemyLaser")
    //    {
    //        //_laserHitPlayerSound.Play();
    //        Destroy(other.gameObject);
    //        Damage();
    //    }
    //}

}
