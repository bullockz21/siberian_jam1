using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f; // Скорость пули
    public float lifetime = 10f; // Время жизни пули

    void Start()
    {
        // Уничтожаем пулю через lifetime секунд
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Двигаем пулю вперёд
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cow"))
        {
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CowGrab();
            Destroy(gameObject);
        }
    }
}
