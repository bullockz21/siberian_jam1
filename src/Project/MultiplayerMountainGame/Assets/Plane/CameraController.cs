using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform cameraPoint;
    [HideInInspector]
    public Transform lookAt;

    public float offset = -3.5f;

    private float timer;

    private void Start()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - timer > 1f && lookAt == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                lookAt = GameObject.FindGameObjectWithTag("Player").transform;
                cameraPoint = lookAt.GetChild(1);
                timer = Time.time;
            }
        }

        if (lookAt != null)
        {
            // ¬ычисл€ем смещенную позицию взгл€да дл€ учета offset
            Vector3 lookAtPositionWithOffset = new Vector3(lookAt.position.x, lookAt.position.y - offset, lookAt.position.z);

            // ѕлавное перемещение камеры к желаемой позиции
            transform.position = Vector3.Lerp(transform.position, cameraPoint.position, Time.deltaTime * 3.0f);

            // Ќаправл€ем камеру на смещенную позицию взгл€да
            transform.LookAt(lookAtPositionWithOffset);
        }
    }
}
