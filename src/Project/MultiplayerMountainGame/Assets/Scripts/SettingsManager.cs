using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [HideInInspector]
    public bool soundToggle;
    [HideInInspector]
    public float musicVolume;
    [HideInInspector]
    public float soundVolume;

    private void Awake()
    {
        soundToggle = true;
        musicVolume = 0.5f;
        soundVolume = 0.5f;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
