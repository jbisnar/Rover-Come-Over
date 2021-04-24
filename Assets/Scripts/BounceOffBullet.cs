using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffBullet : MonoBehaviour
{
    Rigidbody2D rb;
    float minTurnSpeed = 360f;
    float maxTurnSpeed = 1800f;
    public SpriteRenderer bullet;
    float lifetime = 1f;
    float faderate = 1f;
    float curalpha = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var turnSpeed = Random.Range(minTurnSpeed, maxTurnSpeed);
        turnSpeed *= new int[] { -1, 1 }[Random.Range(0,2)];
        rb.angularVelocity = turnSpeed;
        StartCoroutine("Delete", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        bullet.color = new Color(1, 1, 1, curalpha);
        curalpha -= faderate * Time.deltaTime;
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
