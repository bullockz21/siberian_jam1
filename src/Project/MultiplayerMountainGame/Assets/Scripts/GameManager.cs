using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource[] sources;
    public AudioClip clickSound;
    private SettingsManager settings;
    [HideInInspector]
    public Compass compass;
    [HideInInspector]
    public TextOutput textOutput;
    public GameObject pausePanel;
    public Toggle soundToggle;
    public Slider musicSlider;
    public Slider soundSlider;
    public GameObject multiplayerPanel;

    private bool quest1, quest2;

    private float timer;

    [HideInInspector]
    public bool netStarted = false;

    private void Start()
    {
        netStarted = false;
        WaitNet();
        timer = Time.time;
        if (GameObject.FindGameObjectWithTag("SettingsManager") != null)
        {
            settings = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsManager>();
        }
        compass = transform.GetComponent<Compass>();
        textOutput = transform.GetComponent<TextOutput>();
        if (settings != null)
        {
            GetSettings();
        }
    }

    private void Update()
    {
        if (netStarted)
        {
            if (Time.time - timer > 1f && sources[0] == null)
            {
                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    sources[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
                    SoundSetting();
                    timer = Time.time;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pausePanel.activeSelf)
                {
                    ContinueButton();
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    PauseButton();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (compass != null && compass.distance < 700f && !quest1)
            {
                quest1 = true;
                StartCoroutine(textOutput.PrintQuest1Texts());
            }
        }
    }

    private void WaitNet() {
        Cursor.lockState = CursorLockMode.None;
        multiplayerPanel.SetActive(true);
        Time.timeScale = 0.0000001f;
    }

    public void StartNet()
    {
        Cursor.lockState = CursorLockMode.Locked;
        netStarted = true;
        multiplayerPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quest2() {
        if (!quest2)
        {
            quest2 = true;
            StartCoroutine(textOutput.PrintQuest2Texts());
        }
    }

    private void SoundSetting() {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] != null)
            {
                sources[i].mute = !soundToggle.isOn;
            }
        }
        if (sources[0] != null)
        {
            sources[0].volume = soundSlider.value;
        }
        sources[1].volume = soundSlider.value;
        sources[2].volume = musicSlider.value;
    }

    private void PauseButton() {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] != null)
            {
                sources[i].mute = true;
            }
        }
        pausePanel.SetActive(true);
        Time.timeScale = 0.0000001f;
    }

    public void ContinueButton() {
        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] != null)
            {
                sources[i].mute = !soundToggle.isOn;
            }
        }
        pausePanel.SetActive(false);
        sources[1].PlayOneShot(clickSound);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    private void GetSettings()
    {
        soundToggle.isOn = settings.soundToggle;
        soundSlider.value = settings.soundVolume;
        musicSlider.value = settings.musicVolume;
    }

    public void SetSoundToggle(bool value)
    {
        settings.soundToggle = value;
        SoundSetting();
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;
        SoundSetting();
    }

    public void SetSoundVolume(float value)
    {
        settings.soundVolume = value;
        SoundSetting();
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        sources[1].PlayOneShot(clickSound);
        SceneManager.LoadScene(0);
    }
}
