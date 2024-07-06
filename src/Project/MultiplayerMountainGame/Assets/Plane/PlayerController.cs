using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public GameManager gm;
    private CharacterController controller;

    public Transform bulletPoint;
    public float baseSpeed = 20.0f;
    private float deltaSpeed;
    public float pitchSpeed = 20.0f;
    public float yawSpeed = 10.0f;
    public float rollSpeed = 20.0f;
    private float deltaPitchSpeed;
    private float autoBalanceSpeed = 5.0f;
    private float smoothTime = 3.0f;
    public Transform leftThing;
    public Transform rightThing;
    public Transform backThing;
    public Transform middleThing;

    [SerializeField]
    private Transform spawnedObjectPrefab;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public string message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    private void Start()
    {
        if (!IsOwner) return;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        deltaPitchSpeed = pitchSpeed;
        controller = GetComponent<CharacterController>();
        deltaSpeed = baseSpeed;
        transform.position = new Vector3(119f, 15.4f, -1375f);
    }

    private void Update()
    {        
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Transform bullet = Instantiate(spawnedObjectPrefab);
            bullet.transform.position = bulletPoint.transform.position;
            bullet.rotation = bulletPoint.rotation;
            bullet.GetComponent<NetworkObject>().Spawn(true);
            //randomNumber.Value = new MyCustomData
            //{
            //    _int = 10,
            //    _bool = false,
            //    message = "Hello",
            //};
        }

        float rotationInputX = Input.GetAxis("Horizontal");
        float rotationInputY = Input.GetAxis("Vertical");

        if (rotationInputY > 0)
        {
            deltaSpeed = Mathf.Lerp(baseSpeed, baseSpeed * 1.5f, Time.deltaTime * smoothTime * 5);
        }
        else
        {
            deltaSpeed = baseSpeed;
        }
        Vector3 moveVector = transform.forward * deltaSpeed;

        // Поворот по продольной оси (pitch)
        source.pitch = Input.GetAxis("Vertical") > 0 ? Mathf.Lerp(source.pitch, 1.5f, Time.deltaTime * smoothTime) : Mathf.Lerp(source.pitch, 1.0f, Time.deltaTime * smoothTime);
        deltaPitchSpeed = rotationInputY > 0 ? pitchSpeed : pitchSpeed / 2f;
        float pitch = rotationInputY * deltaPitchSpeed * Time.deltaTime;

        // Поворот по рулевой оси (yaw)
        float yaw = rotationInputX * yawSpeed * Time.deltaTime;

        // Поворот по вертикальной оси (roll)
        float roll = -rotationInputX * rollSpeed * Time.deltaTime;

        // Рулвая система
        backThing.localRotation = Quaternion.Euler(Mathf.Lerp(backThing.localEulerAngles.x, backThing.localEulerAngles.x - rotationInputY * 30f, Time.deltaTime * smoothTime), backThing.localEulerAngles.y, backThing.localEulerAngles.z);
        middleThing.localRotation = Quaternion.Euler(middleThing.localEulerAngles.x, Mathf.Lerp(middleThing.localEulerAngles.y, Mathf.Clamp(middleThing.localEulerAngles.y, middleThing.localEulerAngles.y - rotationInputX * 15f, middleThing.localEulerAngles.y - rotationInputX * 15f), Time.deltaTime * smoothTime), middleThing.localEulerAngles.z);
        leftThing.localRotation = Quaternion.Euler(Mathf.Lerp(leftThing.localEulerAngles.x, Mathf.Clamp(leftThing.localEulerAngles.x, leftThing.localEulerAngles.x - rotationInputX * 5f, leftThing.localEulerAngles.x - rotationInputX * 15f), Time.deltaTime * smoothTime), leftThing.localEulerAngles.y, leftThing.localEulerAngles.z);
        rightThing.localRotation = Quaternion.Euler(Mathf.Lerp(rightThing.localEulerAngles.x, Mathf.Clamp(rightThing.localEulerAngles.x, rightThing.localEulerAngles.x + rotationInputX * 5f, rightThing.localEulerAngles.x + rotationInputX * 15f), Time.deltaTime * smoothTime), rightThing.localEulerAngles.y, rightThing.localEulerAngles.z);

        // Применяем повороты
        transform.Rotate(pitch, yaw, roll);

        // Выравнивание самолета
        AutoBalance();

        controller.Move(moveVector * Time.deltaTime);
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
        // Получаем текущий угол поворота рулевой системы по оси X
        float bCurrentRotationX = backThing.localEulerAngles.x;

        // Вычисляем разницу между текущим углом поворота и 0 градусов
        float bRotationDifference = Mathf.DeltaAngle(bCurrentRotationX, 0f);

        // Определяем целевой угол поворота, учитывая ближайший путь к нулю
        float bTargetRotationX = bCurrentRotationX + Mathf.Sign(bRotationDifference) * Mathf.Min(Mathf.Abs(bRotationDifference));

        // Применяем измененный угол поворота
        backThing.localRotation = Quaternion.Euler(Mathf.Lerp(bCurrentRotationX, bTargetRotationX, Time.deltaTime * smoothTime), backThing.localEulerAngles.y, backThing.localEulerAngles.z);

        // Middle thing
        // Получаем текущий угол поворота рулевой системы по оси X
        float mCurrentRotationY = middleThing.localEulerAngles.y;

        // Вычисляем разницу между текущим углом поворота и 0 градусов
        float mRotationDifference = Mathf.DeltaAngle(mCurrentRotationY, 0f);

        // Определяем целевой угол поворота, учитывая ближайший путь к нулю
        float mTargetRotationY = mCurrentRotationY + Mathf.Sign(mRotationDifference) * Mathf.Min(Mathf.Abs(mRotationDifference));

        // Применяем измененный угол поворота
        middleThing.localRotation = Quaternion.Euler(middleThing.localEulerAngles.x, Mathf.Lerp(mCurrentRotationY, mTargetRotationY, Time.deltaTime * smoothTime / 3), middleThing.localEulerAngles.z);

        // Left thing
        // Получаем текущий угол поворота рулевой системы по оси X
        float lCurrentRotationX = leftThing.localEulerAngles.x;

        // Вычисляем разницу между текущим углом поворота и 0 градусов
        float lRotationDifference = Mathf.DeltaAngle(lCurrentRotationX, 0f);

        // Определяем целевой угол поворота, учитывая ближайший путь к нулю
        float lTargetRotationX = lCurrentRotationX + Mathf.Sign(lRotationDifference) * Mathf.Min(Mathf.Abs(lRotationDifference));

        // Применяем измененный угол поворота
        leftThing.localRotation = Quaternion.Euler(Mathf.Lerp(lCurrentRotationX, lTargetRotationX, Time.deltaTime * smoothTime / 3), leftThing.localEulerAngles.y, leftThing.localEulerAngles.z);

        // Right thing
        // Получаем текущий угол поворота рулевой системы по оси X
        float rCurrentRotationX = rightThing.localEulerAngles.x;

        // Вычисляем разницу между текущим углом поворота и 0 градусов
        float rRotationDifference = Mathf.DeltaAngle(rCurrentRotationX, 0f);

        // Определяем целевой угол поворота, учитывая ближайший путь к нулю
        float rTargetRotationX = rCurrentRotationX + Mathf.Sign(rRotationDifference) * Mathf.Min(Mathf.Abs(rRotationDifference));

        // Применяем измененный угол поворота
        rightThing.localRotation = Quaternion.Euler(Mathf.Lerp(rCurrentRotationX, rTargetRotationX, Time.deltaTime * smoothTime / 3), rightThing.localEulerAngles.y, rightThing.localEulerAngles.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            gm.Quest2();
        }
    }
}
