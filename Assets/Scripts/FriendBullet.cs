using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendBullet : MonoBehaviour
{
    int damage = 1;
    public GameObject bounceOffBullet;
    public Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ren.isVisible)
        {
            StartCoroutine("Delete", 2f);
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
        Enemy_Alien enemy = collision.GetComponent<Enemy_Alien>();
        if (enemy != null)
        {
            enemy.Damage(damage);
            GameObject.Destroy(gameObject);
        }
        Enemy_Mine emine = collision.GetComponent<Enemy_Mine>();
        if (emine != null)
        {
            emine.Detonate();
            GameObject.Destroy(gameObject);
        }
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
