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
    public Transform target;
    [HideInInspector]
    public float distance = 999999f;
    private float timer;

    private void Start()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - timer > 1f && player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                timer = Time.time;
            }
        }
        if (player != null)
        {
            playerCompass.uvRect = new Rect(player.localEulerAngles.y / 360, 0, 1, 1);

            distance = Vector3.Distance(player.position, target.position);

            // ����������� �� ������ � ����
            Vector3 directionToTarget = target.position - player.position;

            // ����������� "������" ��� ������ � ��������� XZ
            Vector3 playerForward = player.forward;
            playerForward.y = 0f; // �������� ���������� Y
            playerForward.Normalize();

            // ��������� ���� ����� ������������ ������ � ���� � ������������ "������" ��� ������
            float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) - Mathf.Atan2(playerForward.x, playerForward.z);
            angleToTarget *= Mathf.Rad2Deg;

            // ����������� ���� � �������� �� -180 �� 180
            if (angleToTarget > 180)
            {
                angleToTarget -= 360;
            }
            else if (angleToTarget < -180)
            {
                angleToTarget += 360;
            }

            // ������������� uvRect ��� targetCompass
            targetCompass.uvRect = new Rect(-angleToTarget / 360, 0, 1, 1);
        }
    }
}
