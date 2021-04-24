using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLightning : MonoBehaviour
{
    float lifetime = 4f;
    float particleLifetime = .4f;
    Collider2D[] hitboxes;

    // Start is called before the first frame update
    void Start()
    {
        hitboxes = GetComponentsInChildren<Collider2D>();
        StartCoroutine("Delete", lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_Bullet ebullet = collision.GetComponent<Enemy_Bullet>();
        if (ebullet != null)
        {
            ebullet.Reflect(transform.up);
        }
        Enemy_BallLightning eball = collision.GetComponent<Enemy_BallLightning>();
        if (eball != null)
        {
            eball.Dissipate();
        }
        Enemy_Mine emine = collision.GetComponent<Enemy_Mine>();
        if (emine != null)
        {
            emine.Detonate();
        }
    }

    IEnumerator DisableHitboxes(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < hitboxes.Length; i++) {
            hitboxes[i].enabled = false;
        }
    }
    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
