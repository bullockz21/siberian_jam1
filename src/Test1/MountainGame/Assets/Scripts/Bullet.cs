using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f; // �������� ����
    public float lifetime = 10f; // ����� ����� ����

    void Start()
    {
        // ���������� ���� ����� lifetime ������
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // ������� ���� �����
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
