using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private GameManager gm;

    public GameObject Man;
    public int countCoins = 10;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Man.activeSelf)
        {
           Man.SetActive(false);
           gm.GetCoins(countCoins);
        }
    }
}
