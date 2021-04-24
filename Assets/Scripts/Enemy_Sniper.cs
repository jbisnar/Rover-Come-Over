using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sniper : MonoBehaviour
{
    public GameObject laser;
    float idleTime = 3f;
    float aimingTime = 3f;
    bool aiming = false;
    bool idle = true;
    Rover_AI rover;
    float roverSpeed = 1f;
    Vector3 aim;
    public Renderer ren;
    bool onScreen = false;
    public Transform weaponhand;
    public SpriteRenderer weaponsr;
    public LineRenderer aimLaser;

    // Start is called before the first frame update
    void Start()
    {
        rover = FindObjectOfType<Rover_AI>();
        aim = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onScreen && ren.isVisible)
        {
            onScreen = true;
            StartCoroutine("StartShooting", Random.Range(0f, 1f));
        }
        else if (onScreen && !ren.isVisible && !aiming)
        {
            onScreen = false;
            Destroy(gameObject);
        }

        if (!aiming && !idle)
        {
            var roverRelPos = transform.position - rover.transform.position;
            var predictPos = rover.transform.position + (rover.transform.up * roverSpeed * aimingTime);
            var predictRelPos = predictPos - transform.position;
            aim = predictRelPos.normalized;
            aiming = true;
            StartCoroutine("Shoot", aimingTime);
        }
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
        idle = false;
        StartCoroutine("Shoot", aimingTime);
    }
    IEnumerator Shoot(float delay)
    {
        aimLaser.enabled = true;
        yield return new WaitForSeconds(delay);
        aimLaser.enabled = false;
        var spawnedlaser = GameObject.Instantiate(laser, transform.position, Quaternion.FromToRotation(Vector3.up,(Vector3)aim),null);
        aiming = false;
        idle = true;
        if (onScreen)
        {
            StartCoroutine("Wait", idleTime);
        }
    }
    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        idle = false;
    }
}
