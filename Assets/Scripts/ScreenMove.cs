using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public GameObject rover;
    public GameObject ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y,rover.transform.position.y + 2), transform.position.z);
        ground.transform.position = new Vector3(0f,Mathf.Floor(transform.position.y),1f);
    }
}
