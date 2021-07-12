using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;

    //[SerializeField]
    //private GameObject _explosionPreFab;
    private AudioSource _audioSource;

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
    }

    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -3.8f) 
        {
            float randomX = Random.Range(-8f, 9f);
            transform.position =
                new Vector3(randomX, 7f, 0);

        }
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
}
