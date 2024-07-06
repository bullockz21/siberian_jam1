using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage playerCompass;
    public RawImage targetCompass;
    [HideInInspector]
    public Transform player;
    //[HideInInspector]
    public Transform target;
    [HideInInspector]
    public float distance = 999999f;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (player == null || target == null)
        {
            return;
        }
        playerCompass.uvRect = new Rect(player.localEulerAngles.y / 360, 0, 1, 1);

        distance = Vector3.Distance(player.position, target.position);

        // Направление от игрока к цели
        Vector3 directionToTarget = target.position - player.position;

        // Направление "вперед" для игрока в плоскости XZ
        Vector3 playerForward = player.forward;
        playerForward.y = 0f; // Обнуляем компоненту Y
        playerForward.Normalize();

        // Вычисляем угол между направлением игрока к цели и направлением "вперед" для игрока
        float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) - Mathf.Atan2(playerForward.x, playerForward.z);
        angleToTarget *= Mathf.Rad2Deg;

        // Преобразуем угол в диапазон от -180 до 180
        if (angleToTarget > 180)
        {
            angleToTarget -= 360;
        }
        else if (angleToTarget < -180)
        {
            angleToTarget += 360;
        }

        // Устанавливаем uvRect для targetCompass
        targetCompass.uvRect = new Rect(-angleToTarget / 360, 0, 1, 1);
    }
}
