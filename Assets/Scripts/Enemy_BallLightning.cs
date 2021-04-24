using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BallLightning : MonoBehaviour
{
    int damage = 4;
    public GameObject dissipateParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rover_AI rover = collision.GetComponent<Rover_AI>();
        if (rover != null)
        {
            rover.Damage(damage);
            Dissipate();
        }
    }

    public void Dissipate()
    {
        var spawnedParticles = GameObject.Instantiate(dissipateParticles, transform.position, transform.rotation, null);
        GameObject.Destroy(gameObject);
    }
}
