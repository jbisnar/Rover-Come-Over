using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shotgun : MonoBehaviour
{
    public GameObject bullet;
    float shootDelay = 3f;
    float bulletSpeed = 5f;
    Alien_Move alien;
    Vector3 aim;
    float deviationAngle = 30f;
    Vector3 aimLeft;
    Vector3 aimRight;
    public Renderer ren;
    bool onScreen = false;
    public Transform weaponhand;
    public SpriteRenderer weaponsr;

    // Start is called before the first frame update
    void Start()
    {
        alien = FindObjectOfType<Alien_Move>();
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

        var alienRelPos = transform.position - alien.transform.position;
        var alienvel = alien.GetComponent<Rigidbody2D>().velocity;
        var angleA = Vector2.Angle(alienRelPos, alienvel);
        var a = (alienvel.magnitude * alienvel.magnitude) - (bulletSpeed * bulletSpeed);
        var b = -2 * alienRelPos.magnitude * alienvel.magnitude * Mathf.Cos(angleA * Mathf.Deg2Rad);
        var c = alienRelPos.magnitude * alienRelPos.magnitude;
        var determinant = Mathf.Sqrt((b * b) - (4 * a * c));
        var t = (-b - determinant) / (2 * a);

        var predictPos = alien.transform.position + ((Vector3) alienvel.normalized * alienvel.magnitude * t);
        var predictRelPos = predictPos - transform.position;
        aim = -alienRelPos.normalized;
        var aimAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        aimLeft = new Vector3(Mathf.Cos((aimAngle - deviationAngle) * Mathf.Deg2Rad),Mathf.Sin((aimAngle - deviationAngle) * Mathf.Deg2Rad));
        aimRight = new Vector3(Mathf.Cos((aimAngle + deviationAngle) * Mathf.Deg2Rad), Mathf.Sin((aimAngle + deviationAngle) * Mathf.Deg2Rad));
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
        var spawnedbullet = GameObject.Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector3.right,(Vector3) aim), null);
        spawnedbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(aim.x * bulletSpeed, aim.y * bulletSpeed);
        var spawnedbulletL = GameObject.Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector3.right, (Vector3) aimLeft), null);
        spawnedbulletL.GetComponent<Rigidbody2D>().velocity = new Vector2(aimLeft.x * bulletSpeed, aimLeft.y * bulletSpeed);
        var spawnedbulletR = GameObject.Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector3.right, (Vector3) aimRight), null);
        spawnedbulletR.GetComponent<Rigidbody2D>().velocity = new Vector2(aimRight.x * bulletSpeed, aimRight.y * bulletSpeed);
        yield return new WaitForSeconds(delay);
        if (onScreen)
        {
            StartCoroutine("Shoot", shootDelay);
        }
    }
}
