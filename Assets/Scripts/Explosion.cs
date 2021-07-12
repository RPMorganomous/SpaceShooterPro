using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //private AudioManager _audioManager;
    void Start()
    {
        //_audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        //_audioManager._playExplosion();
        Destroy(this.gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
