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
    public Compass compass;
    [HideInInspector]
    public TextOutput textOutput;
    public GameObject pausePanel;
    public Toggle soundToggle;
    public Slider musicSlider;
    public Slider soundSlider;
    [SerializeField]
    private GameObject questPanel;
    public GameObject packButton;
    public GameObject packPrefab;
    public GameObject clouds, clouds1;
    public Transform targetParent;
    public Transform modButton;

    public GameObject kukuruznikPrefab;
    public GameObject istrebitelPrefab;

    private bool quest1, quest2, quest3;
    private int targetIndex = 0;
    public Text coinsText;
    [HideInInspector]
    public int coins = 0;

    private void Start()
    {
        targetIndex = 0;
        for (int i = 0; i < targetParent.childCount; i++)
        {
            targetParent.GetChild(i).GetComponent<Target>().id = i;
        }
        GetCurrentTarget();
        settings = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsManager>();
        sources[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        compass = transform.GetComponent<Compass>();
        textOutput = transform.GetComponent<TextOutput>();
        if (settings != null)
        {
            GetSettings();
        }
        Cursor.lockState = CursorLockMode.Locked;
        SoundSetting();
    }

    private void Update()
    {
        if (sources[0].Equals(null))
        {
            sources[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        }
        if (settings == null || sources[0].Equals(null))
        {
            return;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            QuestButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PackButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ModButtonClick();
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

    private void GetCurrentTarget() {
        for (int i = 0; i < targetParent.childCount; i++)
        {
            if (targetParent.GetChild(i).GetComponent<Target>().id == targetIndex)
            {
                compass.target = targetParent.GetChild(i).transform;
                targetParent.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                targetParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        targetIndex++;
    }

    public void PackButtonClick() {
        if (packButton.activeSelf)
        {
            packButton.SetActive(false);
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject pack = Instantiate(packPrefab, playerPos.position - Vector3.down * 5f, Quaternion.identity);
            pack.GetComponent<Pack>().gm = this;
            Destroy(pack, 20f);
        }
    }

    public void GettingPack() {
        packButton.SetActive(true);
    }

    public void QuestButtonClick() {
        if (questPanel.activeSelf)
        {
            questPanel.SetActive(false);
        }
        else
        {
            questPanel.SetActive(true);
        }
    }

    public void Quest2() {
        if (!quest2)
        {
            GetCurrentTarget();
            clouds.SetActive(false);
            clouds1.SetActive(true);
            GettingPack();
            quest2 = true;
            GetCoins();
            StartCoroutine(textOutput.PrintQuest2Texts());
        }
    }

    public void Quest3()
    {
        if (!quest3)
        {
            GetCurrentTarget();
            quest3 = true;
            GetCoins();
            StartCoroutine(textOutput.PrintQuest3Texts());
        }
    }

    public void Quest3Failed()
    {
        targetIndex--;
        for (int i = 0; i < targetParent.childCount; i++)
        {
            if (targetParent.GetChild(i).GetComponent<Target>().id != targetIndex)
            {
                targetParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        targetIndex++;
    }

    public void GetCoins() {
        coins += 110;
        coinsText.text = coins.ToString();
        if (coins >= 100)
        {
            modButton.GetComponent<Button>().interactable = true;
        }
    }

    public void ModButtonClick() {
        if (coins >= 100)
        {
            coins -= 100;
            coinsText.text = coins.ToString();
            modButton.GetComponent<Button>().interactable = false;
            modButton.gameObject.SetActive(false);
            ChangePlane();
        }
    }

    private void ChangePlane() {
        GameObject playerFirst = GameObject.FindGameObjectWithTag("Player");
        Vector3 currentPos = playerFirst.transform.position;
        Quaternion currentRot = playerFirst.transform.rotation;
        Destroy(playerFirst);
        Instantiate(istrebitelPrefab, currentPos, currentRot);
    }

    private void SoundSetting() {
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].Equals(null))
            {
                sources[i].mute = !soundToggle.isOn;
            }
        }
        sources[0].volume = soundSlider.value;
        sources[1].volume = soundSlider.value;
        sources[2].volume = musicSlider.value;
    }

    private void PauseButton()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].Equals(null))
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
            if (!sources[i].Equals(null))
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
