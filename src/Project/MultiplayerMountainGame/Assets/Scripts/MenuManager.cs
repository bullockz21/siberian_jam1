using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip clickSound;
    public GameObject settingsPanel;
    private SettingsManager settings;
    public Toggle soundToggle;
    public Slider musicSlider;
    public Slider soundSlider;

    private void Start()
    {
        settings = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsManager>();
        source = GetComponent<AudioSource>();
        if (settings != null)
        {
            GetSettings();
        }
    }

    private void GetSettings() {
        soundToggle.isOn = settings.soundToggle;
        musicSlider.value = settings.musicVolume;
        soundSlider.value = settings.soundVolume;
        source.mute = !soundToggle.isOn;
        source.volume = soundSlider.value;
    }

    public void SetSoundToggle(bool value) {
        settings.soundToggle = value;
        source.mute = !value;
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;
    }

    public void SetSoundVolume(float value)
    {
        settings.soundVolume = value;
        source.volume = value;
    }

    public void PlayButton() {
        source.PlayOneShot(clickSound);
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        source.PlayOneShot(clickSound);
        settingsPanel.SetActive(true);
    }

    public void BackButton() {
        source.PlayOneShot(clickSound);
        settingsPanel.SetActive(false);
    }

    public void QuitButton()
    {
        source.PlayOneShot(clickSound);
        Application.Quit();
    }
}
