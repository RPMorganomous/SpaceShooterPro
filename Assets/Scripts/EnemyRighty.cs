using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRighty : MonoBehaviour
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

    private Vector3 direction = Vector3.right;

    private string enemyType;

    private bool BlackHoleIsOnNow = false;

    [SerializeField]
    private GameObject _explosionPreFab;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();


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

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
                lasers[i].tag = "EnemyLaser";
            }

        }

    }

    void Calculatemovement()
    {

        if (BlackHoleIsOnNow == true)
        {
            _speed = .5f;
            direction = new Vector3(0, 3, 0) - transform.position;
        }

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.x < -12f || transform.position.x > 12f)
        {
            Destroy(this.gameObject);
        }

        // to make horizontal ships reappear on other side of screen
        //if (transform.position.y < -3.8f)
        //{
        //    float randomX = UnityEngine.Random.Range(-8f, 9f);
        //    transform.position =
        //        new Vector3(randomX, 7f, 0);
        //}
        //else if (transform.position.x > 10f)
        //{
        //    float randomY = UnityEngine.Random.Range(0f, 6f);
        //    transform.position = new Vector3(-10, randomY, 0);
        //}
        //else if (transform.position.x < -10f)
        //{
        //    float randomY = UnityEngine.Random.Range(0f, 6f);
        //    transform.position = new Vector3(10, randomY, 0);
        //}
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
            _anim.SetTrigger("OnRightyDeath");
            //_speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(25);
            }

            _audioSource.Play();
            _anim.SetTrigger("OnRightyDeath");
            //_speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

        }

        if (other.tag == "BlackHole")
        {
            if (_player != null)
            {
                _player.AddScore(25);
            }

            _audioSource.Play();
            _anim.SetTrigger("OnRightyDeath");
            _speed = 0;
            _canFire = 10;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

        }
        if (other.tag == "FireBall")
        {
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _canFire = 10;

            Destroy(this.gameObject, 2.8f);
        }
    }
}
