using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Targeted()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled=true;
    }

    public void unTargeted()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled=false;
    }
}
