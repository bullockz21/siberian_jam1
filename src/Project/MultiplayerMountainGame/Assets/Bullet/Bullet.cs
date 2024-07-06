using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f; // Скорость пули
    public float lifetime = 3f; // Время жизни пули

    void Start()
    {
        // Уничтожаем пулю через lifetime секунд
        Invoke("DespawnBullet", lifetime);
    }

    void Update()
    {
        // Двигаем пулю вперёд
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void DespawnBullet()
    {
        // Если есть сетевой объект, используем Despawn(true)
        transform.GetComponent<NetworkObject>().Despawn(true);
        Destroy(gameObject);
    }
}
