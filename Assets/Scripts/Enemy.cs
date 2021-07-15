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

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }

        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if(_anim == null)
        {
            Debug.Log("The Animator is NULL");
        }

        //StartCoroutine(FireEnemyLaser());
    }

    void Update()
    {

        Calculatemovement();

        if (Time.time > _canFire)
        {
            //FireEnemyLaser();
            _fireRate = Random.Range(3f, 7f);
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -3.8f)
        {
            float randomX = Random.Range(-8f, 9f);
            transform.position =
                new Vector3(randomX, 7f, 0);
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

            //bugfix
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
            //bugfix
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }
    }
    //private void FireEnemyLaser()
    //{
    //    //yield return new WaitForSeconds(3.0f);
    //    while (_stopFiring == false)
    //    {
    //        //fire laser
    //        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
    //        //_audioSource.Play();
    //        yield return new WaitForSeconds(Random.Range(3f, 8f));
    //    }
    //}
}
