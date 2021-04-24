using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover_AI : MonoBehaviour
{
    public Vector3 nextpos;
    public float movespeed;
    float movespeedNormal = 1f;
    float movespeedBoosted = 2.5f;
    float boostTime = 5f;
    float distReached = .1f;
    bool absorbing = false;
    float shieldTime = 2f;
    public GameObject absorbShield;
    float knockback = 10f;
    Vector3 knockbackdir;
    float accelSlow = 20f;
    bool stunned = false;
    float stunTime = 1f;

    public int maxhealth = 20;
    public int curhealth;
    public RectTransform healthBar;
    float healthbarWidth;

    public Game_Controller gc;
    public SpriteRenderer sr;
    public GameObject grave;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        nextpos = transform.position;
        movespeed = movespeedNormal;
        curhealth = maxhealth;
        healthbarWidth = healthBar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2(((float)curhealth / (float)maxhealth) * healthbarWidth, healthBar.sizeDelta.y);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(nextpos.y - transform.position.y, nextpos.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward), 180 * Time.deltaTime);
        if (stunned)
        {
            movespeed -= accelSlow * Time.deltaTime;
            if (movespeed < 0)
            {
                movespeed = 0;
            }
            transform.position += knockbackdir * movespeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * movespeed * Time.deltaTime;
        }
        if (Vector3.Distance(transform.position,nextpos) < distReached)
        {
            nextpos += new Vector3(0, 10, 0);
        }
    }

    public void Ping(Vector3 newpos)
    {
        nextpos = newpos;
    }
    public void ChargedPing(Vector3 newpos)
    {
        nextpos = newpos;
        movespeed = movespeedBoosted;
        StartCoroutine("EndBoost", boostTime);
    }

    public void Absorb()
    {
        if (dead) { return; }
        absorbing = true;
        absorbShield.SetActive(true);
        StartCoroutine("EndAbsorb", shieldTime);
    }

    public void Damage(int amount)
    {
        if (absorbing)
        {
            curhealth += amount;
            if (curhealth > maxhealth)
            {
                curhealth = maxhealth;
            }
        }
        else
        {
            curhealth -= amount;
            if (curhealth < 1 && !dead)
            {
                Die();
            }
        }
    }

    public void Knockback(Vector3 direction)
    {
        StopAllCoroutines();
        movespeed = knockback;
        knockbackdir = direction;
        stunned = true;
        StartCoroutine("EndStun", stunTime);
        gc.StopCounting();
    }

    public void Die()
    {
        dead = true;
        StopAllCoroutines();
        movespeed = 0;
        movespeedBoosted = 0;
        movespeedNormal = 0;
        sr.enabled = false;
        GameObject.Instantiate(grave, transform.position, transform.rotation, null);
        gc.GameOver();
        Debug.Log("Rover dead");
    }

    IEnumerator EndBoost(float delay)
    {
        yield return new WaitForSeconds(delay);
        movespeed = movespeedNormal;
    }

    IEnumerator EndStun(float delay)
    {
        yield return new WaitForSeconds(delay);
        stunned = false;
        movespeed = movespeedNormal;
        Ping(transform.position);
        gc.StartCounting();
    }

    IEnumerator EndAbsorb(float delay)
    {
        yield return new WaitForSeconds(delay);
        absorbing = false;
        absorbShield.SetActive(false);
    }
}
