using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Plane Status")]
    [Tooltip("How much the throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine trust when at 100% throttle.")]

    public float maxThrust = 1300f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    public float responsiveness = 60f;

    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public GameManager gm;

    public float throttle;
    public float roll;
    public float pitch;
    public float yaw;

    private float smoothTime = 3.0f;
    private float autoBalanceSpeed = 5.0f;

    public Transform[] things;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 2f) * responsiveness;
        }
    }
    Rigidbody rb;

    public float changePlaneMoveSpeed = 100.0f;
    private float changePlaneMoveProgress = 0.0f;
    private bool isNeedChangePlane = false;
    public GameObject changePlanePortal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();

        source.pitch = Input.GetAxis("Vertical") > 0 ? Mathf.Lerp(source.pitch, 1.5f, Time.deltaTime * smoothTime) : Mathf.Lerp(source.pitch, 1.0f, Time.deltaTime * smoothTime);

        // Рулвая система
        if (things[0] != null)
        {
            things[0].localRotation = Quaternion.Euler(Mathf.Lerp(things[0].localEulerAngles.x, things[0].localEulerAngles.x - pitch * 30f, Time.deltaTime * smoothTime), things[0].localEulerAngles.y, things[0].localEulerAngles.z);
        }
        if (things[1] != null)
        {
            things[1].localRotation = Quaternion.Euler(things[1].localEulerAngles.x, Mathf.Lerp(things[1].localEulerAngles.y, Mathf.Clamp(things[1].localEulerAngles.y, things[1].localEulerAngles.y - yaw * 15f, things[1].localEulerAngles.y - yaw * 15f), Time.deltaTime * smoothTime), things[1].localEulerAngles.z);
        }
        if (things[2] != null)
        {
            things[2].localRotation = Quaternion.Euler(Mathf.Lerp(things[2].localEulerAngles.x, Mathf.Clamp(things[2].localEulerAngles.x, things[2].localEulerAngles.x - roll * 5f, things[2].localEulerAngles.x - roll * 15f), Time.deltaTime * smoothTime), things[2].localEulerAngles.y, things[2].localEulerAngles.z);
        }
        if (things[3] != null)
        {
            things[3].localRotation = Quaternion.Euler(Mathf.Lerp(things[3].localEulerAngles.x, Mathf.Clamp(things[3].localEulerAngles.x, things[3].localEulerAngles.x + roll * 5f, things[3].localEulerAngles.x + roll * 15f), Time.deltaTime * smoothTime), things[3].localEulerAngles.y, things[3].localEulerAngles.z);
        }

        // Выравнивание самолета
        AutoBalance();

        if (isNeedChangePlane)
        {
            changePlanePortal.transform.position = Vector3.Lerp(changePlanePortal.transform.position, transform.position, changePlaneMoveProgress);
            changePlaneMoveProgress += changePlaneMoveSpeed * Time.deltaTime;
            changePlanePortal.transform.localScale = Vector3.Lerp(changePlanePortal.transform.localScale, new Vector3(500000f, 500000f, 50000f), changePlaneMoveProgress);
            if (changePlaneMoveProgress >= 0.05f)
            {
                gm.ChangePlane();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);
    }

    private void AutoBalance()
    {
        // Вращение самолета
        // Получаем текущий угол крена (roll)
        float currentRollAngle = transform.eulerAngles.z;

        // Вычисляем разницу между текущим углом крена и 0 градусов
        float rollAngleDifference = Mathf.DeltaAngle(currentRollAngle, 0f);

        // Определяем целевой угол крена, учитывая ближайший путь к нулю
        float targetRollAngle = currentRollAngle + Mathf.Sign(rollAngleDifference) * Mathf.Min(Mathf.Abs(rollAngleDifference), Time.deltaTime * autoBalanceSpeed);

        // Применяем измененный угол крена
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetRollAngle);

        // Рулевая система

        // Back thing
        if (things[0] != null)
        {
            // Получаем текущий угол поворота рулевой системы по оси X
            float bCurrentRotationX = things[0].localEulerAngles.x;

            // Вычисляем разницу между текущим углом поворота и 0 градусов
            float bRotationDifference = Mathf.DeltaAngle(bCurrentRotationX, 0f);

            // Определяем целевой угол поворота, учитывая ближайший путь к нулю
            float bTargetRotationX = bCurrentRotationX + Mathf.Sign(bRotationDifference) * Mathf.Min(Mathf.Abs(bRotationDifference));

            // Применяем измененный угол поворота
            things[0].localRotation = Quaternion.Euler(Mathf.Lerp(bCurrentRotationX, bTargetRotationX, Time.deltaTime * smoothTime), things[0].localEulerAngles.y, things[0].localEulerAngles.z);
        }

        // Middle thing
        if (things[1] != null)
        {
            // Получаем текущий угол поворота рулевой системы по оси X
            float mCurrentRotationY = things[1].localEulerAngles.y;

            // Вычисляем разницу между текущим углом поворота и 0 градусов
            float mRotationDifference = Mathf.DeltaAngle(mCurrentRotationY, 0f);

            // Определяем целевой угол поворота, учитывая ближайший путь к нулю
            float mTargetRotationY = mCurrentRotationY + Mathf.Sign(mRotationDifference) * Mathf.Min(Mathf.Abs(mRotationDifference));

            // Применяем измененный угол поворота
            things[1].localRotation = Quaternion.Euler(things[1].localEulerAngles.x, Mathf.Lerp(mCurrentRotationY, mTargetRotationY, Time.deltaTime * smoothTime / 3), things[1].localEulerAngles.z);
        }

        // Left thing
        if (things[2] != null)
        {
            // Получаем текущий угол поворота рулевой системы по оси X
            float lCurrentRotationX = things[2].localEulerAngles.x;

            // Вычисляем разницу между текущим углом поворота и 0 градусов
            float lRotationDifference = Mathf.DeltaAngle(lCurrentRotationX, 0f);

            // Определяем целевой угол поворота, учитывая ближайший путь к нулю
            float lTargetRotationX = lCurrentRotationX + Mathf.Sign(lRotationDifference) * Mathf.Min(Mathf.Abs(lRotationDifference));

            // Применяем измененный угол поворота
            things[2].localRotation = Quaternion.Euler(Mathf.Lerp(lCurrentRotationX, lTargetRotationX, Time.deltaTime * smoothTime / 3), things[2].localEulerAngles.y, things[2].localEulerAngles.z);
        }

        // Right thing
        if (things[3] != null)
        {
            // Получаем текущий угол поворота рулевой системы по оси X
            float rCurrentRotationX = things[3].localEulerAngles.x;

            // Вычисляем разницу между текущим углом поворота и 0 градусов
            float rRotationDifference = Mathf.DeltaAngle(rCurrentRotationX, 0f);

            // Определяем целевой угол поворота, учитывая ближайший путь к нулю
            float rTargetRotationX = rCurrentRotationX + Mathf.Sign(rRotationDifference) * Mathf.Min(Mathf.Abs(rRotationDifference));

            // Применяем измененный угол поворота
            things[3].localRotation = Quaternion.Euler(Mathf.Lerp(rCurrentRotationX, rTargetRotationX, Time.deltaTime * smoothTime / 3), things[3].localEulerAngles.y, things[3].localEulerAngles.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            gm.Quest2();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Target") && !collision.transform.CompareTag("Player") && !collision.transform.CompareTag("DontCheck"))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void needChangePlane()
    {
        changePlanePortal.SetActive(true);
        isNeedChangePlane = true;
    }
}