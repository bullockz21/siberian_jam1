using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearScript : MonoBehaviour
{

    [HideInInspector]
    public bool moveUp = false;
    public bool moveState = false;
    public float moveTime = 0.005f;

    private float time = 0.0f;
    

    public void moveCloudUp() {
        moveState = true;
        moveUp = true;
    }
    public void moveCloudDown() { 
        moveState = true;
        moveUp = false;
    }

    public void stop() {
        moveState = false;
    }

    public void Update()
    {
        if (!moveState)
            return;
        if (moveUp)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 4500f, time), transform.position.z);
            time += moveTime * Time.deltaTime;
            if (transform.position.y >= 4500f)
            {
                time = 0.0f;
                moveState=false;
            }
        }
        else {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, 3100f, time), transform.position.z);
            time += moveTime * Time.deltaTime;
            if (transform.position.y <= 3100f)
            {
                time = 0.0f;
                moveState = false;
            }
        }
    }
}