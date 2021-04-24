using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    int damage = 3;
    LineRenderer liner;
    float hitboxLifetime = .2f;
    float partifleLifetime = .7f;
    float faderate = 5f;
    float curalpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        liner = GetComponent<LineRenderer>();
        StartCoroutine("DeactivateHitbox", hitboxLifetime);
        StartCoroutine("Delete", partifleLifetime);

    }

    // Update is called once per frame
    void Update()
    {
        liner.startColor = new Color32(255, 192, 0, (byte)curalpha);
        curalpha -= faderate * Time.deltaTime;
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
        liner.enabled = false;
    }
    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
