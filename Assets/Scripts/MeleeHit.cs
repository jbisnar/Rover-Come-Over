using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    float lifetime = .2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Delete", lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_Alien enemy = collision.GetComponent<Enemy_Alien>();
        if (enemy != null)
        {
            enemy.Damage(1);
        }
        Enemy_Bullet ebullet = collision.GetComponent<Enemy_Bullet>();
        if (ebullet != null)
        {
            ebullet.Reflect(transform.up);
        }
        Enemy_BallLightning eball = collision.GetComponent<Enemy_BallLightning>();
        if (eball != null)
        {
            eball.Dissipate();
        }
        Enemy_Mine emine = collision.GetComponent<Enemy_Mine>();
        if (emine != null)
        {
            emine.Detonate();
        }
        Rover_AI rover = collision.GetComponent<Rover_AI>();
        if (rover != null)
        {
            rover.Knockback(transform.up);
        }
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
