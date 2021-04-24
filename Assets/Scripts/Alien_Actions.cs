using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Actions : MonoBehaviour
{
    public bool charged = false;
    Vector3 aim;
    float meleeActiveTime = .05f;
    float meleeCool = .5f;
    float meleeCoolFrenzy = .25f;
    public bool canMelee = true;
    float pingCool = 2f;
    bool canPing = true;
    public bool frenzy = false;
    public Camera cam;
    public GameObject meleeSwing;
    public GameObject meleeLightning;
    GameObject currentLightning;
    public GameObject ping;
    public GameObject chargedPing;
    bool absorbing = false;
    float shieldTime = 2f;
    public GameObject absorbShield;
    Rover_AI rover;

    public int maxhealth = 20;
    public int curhealth;
    public RectTransform healthBar;
    float healthbarWidth;

    public Transform weaponhand;

    public Game_Controller gc;
    public GameObject grave;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        rover = FindObjectOfType<Rover_AI>();
        curhealth = maxhealth;
        healthbarWidth = healthBar.rect.width;
        aim = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2(((float) curhealth / (float) maxhealth) * healthbarWidth, healthBar.sizeDelta.y);

        var mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
        aim = mousePos - new Vector3(transform.position.x, transform.position.y);
        aim = aim.normalized;
        if (Input.GetMouseButtonDown(0) && canMelee)
        {
            if (charged)
            {
                SwingCharged();
            }
            else
            {
                SwingNormal();
            }
            weaponhand.localScale = new Vector3(-weaponhand.localScale.x, 1f, 1f);
        }
        if (Input.GetMouseButtonDown(1) && canPing)
        {
            Ping(mousePos);
        }
        weaponhand.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)aim);
        if (currentLightning != null)
        {
            currentLightning.transform.position = transform.position;
            currentLightning.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)aim);
        }
    }

    public void SwingNormal()
    {
        var swingpos = transform.position + aim * 1f;
        var swingrot = Quaternion.AngleAxis(Mathf.Atan2(aim.y,aim.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
        var spawnedSwing = GameObject.Instantiate(meleeSwing, swingpos, swingrot, transform);
        canMelee = false;
        if (frenzy)
        {
            StartCoroutine("SwingCooldown", meleeCoolFrenzy);
        }
        else
        {
            StartCoroutine("SwingCooldown", meleeCool);
        }
    }
    public void SwingCharged()
    {
        var swingpos = transform.position + aim * 1f;
        var swingrot = Quaternion.AngleAxis(Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
        var spawnedSwing = GameObject.Instantiate(meleeSwing, swingpos, swingrot, transform);
        var spawnedLightning = GameObject.Instantiate(meleeLightning, transform.position, swingrot, null);
        currentLightning = spawnedLightning;
        canMelee = false;
        GetComponent<Alien_Move>().ChangeCharge(false);
        StartCoroutine("SwingCooldown", meleeCool);
    }

    public void Ping(Vector3 mousePos)
    {
        if (charged)
        {
            GameObject.Instantiate(chargedPing, mousePos, transform.rotation, null);
            rover.ChargedPing(new Vector3(mousePos.x, mousePos.y, 0f));
            GetComponent<Alien_Move>().ChangeCharge(false);
        }
        else
        {
            GameObject.Instantiate(ping, mousePos, transform.rotation, null);
            rover.Ping(new Vector3(mousePos.x, mousePos.y, 0f));
        }
        canPing = false;
        StartCoroutine("PingCooldown", pingCool);
    }

    public void Absorb()
    {
        absorbing = true;
        absorbShield.SetActive(true);
        GetComponent<Alien_Move>().ChangeCharge(false);
        rover.Absorb();
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
                GetComponent<Alien_Move>().Die();
            }
        }
    }
    public void Die()
    {
        dead = true;
        StopAllCoroutines();
        canMelee = false;
        canPing = false;
        weaponhand.gameObject.SetActive(false);
        GameObject.Instantiate(grave, transform.position, transform.rotation, null);
        gc.GameOver();
        Debug.Log("Alien dead");
    }

    IEnumerator SwingCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMelee = true;
    }

    IEnumerator PingCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPing = true;
    }

    IEnumerator EndAbsorb(float delay)
    {
        yield return new WaitForSeconds(delay);
        absorbing = false;
        absorbShield.SetActive(false);
    }
}
