using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f; // �������� ����
    public float lifetime = 3f; // ����� ����� ����

    void Start()
    {
        // ���������� ���� ����� lifetime ������
        Invoke("DespawnBullet", lifetime);
    }

    void Update()
    {
        // ������� ���� �����
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void DespawnBullet()
    {
        // ���� ���� ������� ������, ���������� Despawn(true)
        transform.GetComponent<NetworkObject>().Despawn(true);
        Destroy(gameObject);
    }
}
