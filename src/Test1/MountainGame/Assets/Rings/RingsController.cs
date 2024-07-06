using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingsController : MonoBehaviour
{
    public bool isNotHavePrevRing = true;
    public bool isFinish = false;
    public GameObject PrevRing;
    public GameObject NextRing;
    public GameObject NextNextRing;
    private GameManager gm;
    public int countCoins = 10;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PrevRing)
            {
                isNotHavePrevRing = !PrevRing.activeSelf;
            }
            if (NextRing && isNotHavePrevRing)
            {
                NextRing.SetActive(true);
                gameObject.SetActive(false);
                gm.GetCoins(countCoins);
            }
            if (isFinish && isNotHavePrevRing)
            {
                gameObject.SetActive(false);
                gm.GetCoins(countCoins);
            }
            if (NextNextRing)
            {
                NextNextRing.SetActive(true);
            }
        }
    }
}
