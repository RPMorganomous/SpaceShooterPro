using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedHigh = 7f;
    [SerializeField]
    private float _speedLow = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _laserPrefab, _torpedoPrefab;

    [SerializeField]
    private GameObject _tripleShot, _tripleShotTorpedoes;

    [SerializeField]
    private GameObject _blackHole;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _outOfAmmoSound;
    [SerializeField]
    private AudioClip _laserHitPlayerSound;
    [SerializeField]
    private AudioClip _blackHoleSound;

    private AudioSource _audioSource;
    private AudioSource _audioSourceBH;

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
    private bool _blackHoleActive = false;
    public static bool BlackHoleIsOn = false;

    private SpawnManager _spawnManager;

    int lastRunnerTripleShot = 0;
    int lastRunnerSpeedBoost = 0;

    [SerializeField]
    private GameObject _shieldVisualizer;
    private Material _shieldRenderer;
    private int _shieldLevel = 3;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    public int _ammo = 15;
    public int _torpedoes = 0;

    public static Action onBlackHoleAction;

    public static Func<Vector3, int> onBlackHoleFunc;

    private GameObject BlackHoleOnScreen;

    bool shiftBoost = false;
    float shiftBoostPower = 100;
    public Image BoostPower;

    public Image[] _thrusterPoints;

    private Shake shake;

    [SerializeField]
    private bool _invincibility;

    [SerializeField]
    private Sprite _turnLeft, _turnRight, _turnNone;
    private SpriteRenderer spriteRenderer;

    private bool torpedosArmed = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioSourceBH = GameObject.Find("BlackHoleAudioSource").GetComponent<AudioSource>();
        _outOfAmmoSound = GetComponent<Player>()._outOfAmmoSound;
        _shieldRenderer = _shieldVisualizer.GetComponent<SpriteRenderer>().material;
        _shieldRenderer.SetColor("_Color", Color.white);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

        shiftBoostPower = 100f;
        _uiManager.UpdateAmmo(_ammo);

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

        if (_audioSourceBH == null)
        {
            Debug.LogError("The Audio Source on the Black Hole is NULL");
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
                _speed = _speedHigh;
                shiftBoost = true;
         
            StartCoroutine(SpeedShiftPowerDownRoutine());
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speedLow;
            shiftBoost = false;
            StartCoroutine(SpeedShiftPowerUpRoutine());
        }

        SpeedShiftBarFiller();

        CalculateMovement();

        if (Input.GetKeyDown("space") && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_blackHoleActive == true & BlackHoleIsOn == false)
            {

                if(onBlackHoleAction != null)
                {
                    BlackHoleOnScreen = Instantiate(_blackHole, new Vector3(0, 3, 0), Quaternion.identity);
                    onBlackHoleAction();
                    BlackHoleIsOn = true;
                    _audioSourceBH.Play();
                    shake.CamShakeBH();
                }
                StartCoroutine(BlackHolePowerDownRoutine());
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (torpedosArmed == true && _torpedoes > 0)
            {
                FireTorpedo();
            }
            else
            {
                _audioSource.clip = _outOfAmmoSound;
                _audioSource.Play();
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_invincibility == false)
            {
                _invincibility = true;
            }
            else
            {
                _invincibility = false;
            }
        }

        //if (Input.GetKeyDown(KeyCode.1)) maybe add an ammo order here?

        ColorChanger();
    }

    bool DisplayThrusterPoint(float _thrustRemaining, int pointNumber)
    {
        return ((pointNumber * 10) >= _thrustRemaining);
    }

    void ColorChanger()
    {
        Color shiftBoosterColor = Color.Lerp(Color.red, Color.green, shiftBoostPower / 100);

        BoostPower.color = shiftBoosterColor;
    }

    void SpeedShiftBarFiller()
    {
        BoostPower.fillAmount = shiftBoostPower / 100;

        for (int i = 0; i < _thrusterPoints.Length; i++)
        {
            _thrusterPoints[i].enabled = !DisplayThrusterPoint(shiftBoostPower, i);
        }
    }

    private void FireTorpedo()
    {
        if (_tripleShotActive == true)
        {
            GameObject laser = Instantiate(_tripleShotTorpedoes, transform.position, Quaternion.identity);
            Laser[] lasers = laser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].ArmTorpedo();
            }
            _torpedoes--;
            _audioSource.clip = _laserSound;
            _audioSource.Play();
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity).GetComponent<Laser>().ArmTorpedo();
            _torpedoes--;
            _audioSource.clip = _laserSound;
            _audioSource.Play();
        }

        if (_torpedoes == 0)
        {
            torpedosArmed = false;
        }
        _uiManager.UpdateTorpedo(_torpedoes);
    }

    private void FireLaser()
    {
        if (_ammo > 0)
        {
            _canFire = Time.time + _fireRate;

            if (_tripleShotActive == true)
            {

                        GameObject laser = Instantiate(_tripleShot, transform.position, Quaternion.identity);
                        _ammo--;
                        _audioSource.clip = _laserSound;
                        _audioSource.Play();
            }
            else
            {
                    _ = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                    _ammo--;
                    _audioSource.clip = _laserSound;
                    _audioSource.Play();
            }
            _uiManager.UpdateAmmo(_ammo);
        }
        else
        {
            _audioSource.clip = _outOfAmmoSound;
            _audioSource.Play();
        }
    }

    public void BlackholePowerup()
    {
        _blackHoleActive = true;
        Debug.Log("Black Hole Active = true");
    }

    public void LaserRecharge()
    {
        _ammo += 15;
        _audioSource.clip = _laserSound;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void TorpedoPowerup()
    {
        _torpedoes += 12;
        Debug.Log("Torpedoes = " + _torpedoes);
        _audioSource.clip = _laserSound;
        _uiManager.UpdateTorpedo(_torpedoes);
        torpedosArmed = true;
    }

    void CalculateMovement()
    {
        float horizontalInput =
              Input.GetAxis("Horizontal");

        if (horizontalInput > 0)
        {
            spriteRenderer.sprite = _turnRight;
        }

        if (horizontalInput < 0)
        {
            spriteRenderer.sprite = _turnLeft;
        }

        if (horizontalInput == 0)
        {
            spriteRenderer.sprite = _turnNone;
        }

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
        if (_shieldsActive == true )
        {
            _shieldLevel--;
            switch (_shieldLevel)
            {
                case 0:
                    _shieldsActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;

                case 1:
                    _shieldRenderer.SetColor("_Color", Color.red);
                    break;

                case 2:
                    _shieldRenderer.SetColor("_Color", Color.grey);
                    break;
            }

            return;  
        }
        else
        {

        }

        if (_invincibility == false)
        {
            _lives--;
        }
        
        shake.CamShake();

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

    public void HealthPowerup()
    {
        if (_lives < 3)
        {
            _lives++;

            switch (_lives)
            {
                case 3:
                    _rightEngine.SetActive(false);
                    _leftEngine.SetActive(false);
                    break;
                case 2:
                    _rightEngine.SetActive(true);
                    _leftEngine.SetActive(false);
                    break;
                case 1:
                    _rightEngine.SetActive(true);
                    _leftEngine.SetActive(true);
                    break;
            }

            _uiManager.UpdateLives(_lives);
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
            lastRunnerTripleShot++;
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
            lastRunnerTripleShot--;
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
            lastRunnerSpeedBoost++;
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
            lastRunnerSpeedBoost--;
            _speedBoostActive = false;
            SpeedBoostActive();
        }
        
    }

    IEnumerator SpeedShiftPowerUpRoutine()
    {
        if (shiftBoost == false & shiftBoostPower < 100f)
        {
            yield return new WaitForSeconds(0.01f);
            shiftBoostPower += 1f;
            StartCoroutine(SpeedShiftPowerUpRoutine());
        }
    }

    IEnumerator SpeedShiftPowerDownRoutine()
    {
        if (shiftBoost == true & shiftBoostPower > 0f)
        {
            yield return new WaitForSeconds(0.01f);
            shiftBoostPower -= 1f;
            if (shiftBoostPower == 0f)
            {
                _speed = _speedLow;
                shiftBoost = false;
            }
            StartCoroutine(SpeedShiftPowerDownRoutine());
        }
        
    }

    IEnumerator BlackHolePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(BlackHoleOnScreen);
        _blackHoleActive = false;
        BlackHoleIsOn = false;
        if (onBlackHoleAction != null)
        {
            onBlackHoleAction();
        }
    }

    public void ShieldsActive()
    {
            _shieldLevel = 3;
            _shieldsActive = true;
            _shieldRenderer.SetColor("_Color", Color.white);
            _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
