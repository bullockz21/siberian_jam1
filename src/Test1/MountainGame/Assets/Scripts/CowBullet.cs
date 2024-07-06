using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CowBullet : MonoBehaviour
{
    public float speed = 300f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Cow"))
        {
            Destroy(gameObject);
        }
    }
}
