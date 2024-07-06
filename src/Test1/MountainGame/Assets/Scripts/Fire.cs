using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPrefab;
    public Transform[] bulletPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < bulletPoint.Length; i++)
            {
                Transform bullet = Instantiate(bulletPrefab);
                Debug.Log(bullet);
                bullet.transform.position = bulletPoint[i].transform.position;
                bullet.rotation = bulletPoint[i].rotation;
            }
        }
    }
}
