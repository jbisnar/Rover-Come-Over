using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    Alien_Actions alien;
    float turnspeeed = 45f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        alien = FindObjectOfType<Alien_Actions>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var alienRelPos = alien.transform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(alienRelPos.y, alienRelPos.x) * Mathf.Rad2Deg, Vector3.forward), turnspeeed * Time.deltaTime);
        rb.velocity = transform.right * rb.velocity.magnitude;
    }
}
