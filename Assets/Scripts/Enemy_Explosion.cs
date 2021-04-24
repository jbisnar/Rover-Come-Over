using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Explosion : MonoBehaviour
{
    int damage = 6;
    float hitboxActiveTime = .2f;
    float particleTime = .6f;
    public SpriteRenderer explosionsr;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DeactivateHitbox",hitboxActiveTime);
        StartCoroutine("Delete", particleTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rover_AI rover = collision.GetComponent<Rover_AI>();
        if (rover != null)
        {
            rover.Damage(damage);
        }
    }

    IEnumerator DeactivateHitbox(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = false;
        explosionsr.enabled = false;
    }
    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
