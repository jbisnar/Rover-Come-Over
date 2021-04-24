using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappateParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Delete", 1.1f);
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
