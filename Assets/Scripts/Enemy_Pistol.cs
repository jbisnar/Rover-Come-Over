using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pistol : MonoBehaviour
{
    public GameObject bullet;
    float shootDelay = 4f;
    float bulletSpeed = 2f;
    Rover_AI rover;
    float roverSpeed = 1f;
    Vector3 aim;
    public Renderer ren;
    bool onScreen = false;
    public Transform weaponhand;
    public SpriteRenderer weaponsr;

    // Start is called before the first frame update
    void Start()
    {
        rover = FindObjectOfType<Rover_AI>();
        aim = Vector3.up * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onScreen && ren.isVisible)
        {
            onScreen = true;
            StartCoroutine("StartShooting", Random.Range(0f, 1f));
        }
        else if (onScreen && !ren.isVisible)
        {
            onScreen = false;
            Destroy(gameObject);
        }

        var roverRelPos = transform.position - rover.transform.position;
        var angleA = Vector2.Angle(roverRelPos,rover.transform.up);
        var a = (rover.movespeed * rover.movespeed) - (bulletSpeed * bulletSpeed);
        var b = -2 * roverRelPos.magnitude * rover.movespeed * Mathf.Cos(angleA * Mathf.Deg2Rad);
        var c = roverRelPos.magnitude * roverRelPos.magnitude;
        var determinant = Mathf.Sqrt((b * b) - (4 * a * c));
        var t = (-b - determinant) / (2 * a);

        var predictPos = rover.transform.position + (rover.transform.up * roverSpeed * t);
        var predictRelPos = predictPos - transform.position;
        aim = predictRelPos.normalized;
        weaponhand.rotation = Quaternion.FromToRotation(Vector3.right, (Vector3)aim);
        if (aim.x < 0)
        {
            weaponsr.flipY = true;
        }
        else
        {
            weaponsr.flipY = false;
        }
    }

    IEnumerator StartShooting(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine("Shoot", shootDelay);
    }
    IEnumerator Shoot(float delay)
    {
        if (aim.magnitude > 0)
        {
            var spawnedbullet = GameObject.Instantiate(bullet, transform.position, transform.rotation, null);
            spawnedbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(aim.x * bulletSpeed, aim.y * bulletSpeed);
        }
        yield return new WaitForSeconds(delay);
        if (onScreen)
        {
            StartCoroutine("Shoot", shootDelay);
        }
    }
}
