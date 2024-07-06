using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField]
    private Button hostBtn;
    [SerializeField]
    private Button clientBtn;

    [HideInInspector]
    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            gm.StartNet();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            gm.StartNet();
        });
    }
}
