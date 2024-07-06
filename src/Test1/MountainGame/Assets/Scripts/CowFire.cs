using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowFire : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ����
    private Transform player; // ���� (��������, ������� �������)
    public float spawnRadius = 500f; // ������, � ������� ���� ����� ���� �������
    public float spawnInterval = 1f; // �������� ����� ��������� ����

    private float lastSpawnTime;

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (Time.time - lastSpawnTime > spawnInterval && IsPlayerWithinRadius())
        {
            SpawnBullet();
            lastSpawnTime = Time.time;
        }
    }

    bool IsPlayerWithinRadius()
    {
        return player != null && Vector3.Distance(transform.position, player.position) <= spawnRadius;
    }

    void SpawnBullet()
    {
        if (bulletPrefab != null)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // ���������� ����������� ����
            Vector3 direction = (player.position - transform.position).normalized;
            // ������������ ���� � ����������� ����
            bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            Debug.LogWarning("������ ���� �� ����������.");
        }
    }
}