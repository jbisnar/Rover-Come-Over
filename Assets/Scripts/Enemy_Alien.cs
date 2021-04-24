using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Alien : MonoBehaviour
{
    int maxhealth = 2;
    int curhealth;

    // Start is called before the first frame update
    void Start()
    {
        curhealth = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        curhealth -= amount;
        if (curhealth < 1)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
