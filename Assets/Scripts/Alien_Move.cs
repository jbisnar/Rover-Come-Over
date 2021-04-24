using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Move : MonoBehaviour
{
    Rigidbody2D rb;
    public SpriteRenderer sr;
    public Sprite unchargedSpr;
    public Sprite chargedSpr;
    public ParticleSystem particles;
    public ParticleSystem trail;

    float velCur;
    float velWalk = 4f;
    float velWalkCharging = 4f;
    float velWalkFrenzy = 8f;

    float velDash = 20f;
    float dashTime = .08f;
    bool canDash = true;
    float dashCool = .25f;
    bool frenzy = false;
    float frenzyTime = 4f;

    bool charged = false;
    float chargeTime = 2f;
    bool canCharge = true;
    float chargeCool = 4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr.sprite = unchargedSpr;
        velCur = velWalk;
        particles.Stop();
        trail.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        var temp = rb.velocity;
        temp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        temp = temp.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine("DashNormal", dashTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canCharge)
            {
                StartCoroutine("Charge", chargeTime);
            }
            else if (charged)
            {
                GetComponent<Alien_Actions>().Absorb();
            }
        }

        rb.velocity = temp * velCur;
    }

    public void ChangeCharge(bool nowcharged)
    {
        charged = nowcharged;
        GetComponent<Alien_Actions>().charged = charged;
        if (charged)
        {
            sr.sprite = chargedSpr;
            canDash = true;
            GetComponent<Alien_Actions>().canMelee = true;
        } else
        {
            sr.sprite = unchargedSpr;
            particles.Stop();
            StartCoroutine("ChargeCooldown", chargeCool);
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        canDash = false;
        canCharge = false;
        sr.enabled = false;
        velCur = 0f;
    }

    IEnumerator DashNormal(float delay)
    {
        velCur = velDash;
        canDash = false;
        canCharge = false;
        StartCoroutine("DashCooldown", dashCool);
        yield return new WaitForSeconds(delay);
        if (charged)
        {
            frenzy = true;
            ChangeCharge(false);
            GetComponent<Alien_Actions>().frenzy = true;
            trail.Play();
            StartCoroutine("EndFrenzy", frenzyTime);
        }
        if (frenzy)
        {
            velCur = velWalkFrenzy;
        }
        else
        {
            velCur = velWalk;
        }
    }
    IEnumerator DashCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDash = true;
        canCharge = true;
    }
    IEnumerator EndFrenzy(float delay)
    {
        yield return new WaitForSeconds(delay);
        frenzy = false;
        velCur = velWalk;
        GetComponent<Alien_Actions>().frenzy = false;
        trail.Stop();
    }

    IEnumerator Charge(float delay)
    {
        velCur = velWalkCharging;
        canDash = false;
        canCharge = false;
        particles.Play();
        GetComponent<Alien_Actions>().canMelee = false;
        yield return new WaitForSeconds(delay);
        ChangeCharge(true);
        velCur = velWalk;
    }
    IEnumerator ChargeCooldown(float delay)
    {
        canDash = true;
        yield return new WaitForSeconds(delay);
        canCharge = true;
    }
}
