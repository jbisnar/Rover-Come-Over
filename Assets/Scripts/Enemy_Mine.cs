using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine : MonoBehaviour
{
    public GameObject explosion;
    public Renderer ren;
    bool onScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!onScreen && ren.isVisible)
        {
            onScreen = true;
        }
        else if (onScreen && !ren.isVisible)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void Detonate()
    {
        var spawnedexp = GameObject.Instantiate(explosion, transform.position, transform.rotation, null);
        GameObject.Destroy(gameObject);
    }
}
