using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mine_Detector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rover_AI rover = collision.GetComponent<Rover_AI>();
        if (rover != null)
        {
            StartCoroutine("DetonateDelay", .5f);
        }
    }

    IEnumerator DetonateDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponentInParent<Enemy_Mine>().Detonate();
    }
}
