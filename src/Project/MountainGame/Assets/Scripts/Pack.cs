using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack : MonoBehaviour
{
    [HideInInspector]
    public GameManager gm;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Target"))
        {
            gm.Quest3();
        }
        else
        {
            if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("DontCheck"))
            {
                gm.Quest3Failed();
            }
        }
    }
}
