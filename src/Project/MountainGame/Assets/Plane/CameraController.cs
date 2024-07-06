using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraPoint;
    private Transform lookAt;

    private float offset = -3.5f;
    private float smoothTime = 0.63f;
    private Vector3 velocity = Vector3.zero;

    private float defaultDistanceToTarget = 35f;
    private float maxDistanceToTarget = 50f;
    private float minDistanceToTarget = 9.5f;
    private float currentDistanceToTarget;
    private float distanceChangeSpeed = 15f;

    void Start()
    {
        // —охран€ем начальное рассто€ние от камеры до цели
        //currentDistanceToTarget = Vector3.Distance(transform.position, lookAt.position);
        currentDistanceToTarget = defaultDistanceToTarget;
    }

    private void Update()
    {
        if (cameraPoint == null)
        {
            cameraPoint = GameObject.FindGameObjectWithTag("CameraPoint").transform;
        }
        if (lookAt == null)
        {
            lookAt = GameObject.FindGameObjectWithTag("LookAt").transform;
        }
        if (cameraPoint == null || lookAt == null)
        {
            return;
        }
        // ¬ычисл€ем смещенную позицию взгл€да дл€ учета offset
        Vector3 lookAtPositionWithOffset = new Vector3(lookAt.position.x, lookAt.position.y - offset, lookAt.position.z);

        if (Input.GetMouseButton(1))
        {
            if (transform.parent == null)
            {
                transform.parent = lookAt;
                // ѕлавно переходим к дистанции
                currentDistanceToTarget = defaultDistanceToTarget;
            }

            // »змен€ем рассто€ние камеры от самолета при вращении колесика мыши
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheel != 0)
            {
                currentDistanceToTarget = Mathf.Clamp(currentDistanceToTarget - scrollWheel * 10f, minDistanceToTarget, maxDistanceToTarget);
            }

            // ¬ращение камеры вокруг самолета
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // ќбновл€ем позицию камеры после вращени€
            Vector3 desiredPosition = lookAt.position - transform.forward * currentDistanceToTarget;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * distanceChangeSpeed);

            // ѕоворачиваем камеру
            transform.RotateAround(lookAt.position, Vector3.up, mouseX);
            transform.RotateAround(lookAt.position, transform.right, -mouseY);
            return;
        }

        if (transform.parent != null)
        {
            transform.parent = null;
        }

        // ѕлавное перемещение камеры к желаемой позиции
        transform.position = Vector3.SmoothDamp(transform.position, cameraPoint.position, ref velocity, smoothTime);

        // Ќаправл€ем камеру на смещенную позицию взгл€да
        transform.LookAt(lookAtPositionWithOffset);
    }
}
