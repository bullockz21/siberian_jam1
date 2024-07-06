using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowFire : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    private Transform player; // Цель (например, летящий самолет)
    public float spawnRadius = 500f; // Радиус, в котором пуля может быть создана
    public float spawnInterval = 1f; // Интервал между созданием пуль

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
            // Определяем направление пули
            Vector3 direction = (player.position - transform.position).normalized;
            // Поворачиваем пулю в направлении цели
            bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            Debug.LogWarning("Префаб пули не установлен.");
        }
    }
}