using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    int damage = 4;
    public GameObject friendlyBullet;
    public GameObject bounceOffBullet;
    public Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!ren.isVisible)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rover_AI rover = collision.GetComponent<Rover_AI>();
        if (rover != null)
        {
            var spawnedbullet = GameObject.Instantiate(bounceOffBullet, transform.position, transform.rotation, null);
            GameObject.Destroy(gameObject);
        }
        Alien_Actions alien = collision.GetComponent<Alien_Actions>();
        if (alien != null)
        {
            alien.Damage(damage);
            GameObject.Destroy(gameObject);
        }
    }

    public void Reflect(Vector2 direction)
    {
        var spawnedbullet = GameObject.Instantiate(friendlyBullet, transform.position, Quaternion.FromToRotation(Vector3.right, (Vector3) direction), null);
        spawnedbullet.GetComponent<Rigidbody2D>().velocity = direction * GetComponent<Rigidbody2D>().velocity.magnitude;
        GameObject.Destroy(gameObject);
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
