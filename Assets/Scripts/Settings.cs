using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider musicVolume;
    public Slider Sfx_Volume;
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI MusicText;
    public TextMeshProUGUI sfxtext;

    public Toggle Fullscreen;
    public Toggle vsync;

    public Button QualityLeft;
    public Button QualityRight;
    public TextMeshProUGUI QualityText;

    public Button Back;
    public Button Close;

    public GameObject MainMenuCanvas;

    public int quality;
    public float music;
    public float sfx;

    void Start()
    {
        MasterVolume.value = PlayerPrefs.GetFloat("master", 80);
        musicVolume.value = PlayerPrefs.GetFloat("music", 60);
        Sfx_Volume.value = PlayerPrefs.GetFloat("sfx", 70);
        if (PlayerPrefs.GetInt("fullscreen", 1) == 1)
            Fullscreen.isOn = true;
        else
            Fullscreen.isOn = false;
        if (PlayerPrefs.GetInt("vsync", 1) == 1)
            vsync.isOn = true;
        else
            vsync.isOn = false;
        quality = PlayerPrefs.GetInt("quality", 1);

        QualityLeft.onClick.AddListener(qualityDown);
        QualityRight.onClick.AddListener(QualityUp);
        Back.onClick.AddListener(GoBack);
        Close.onClick.AddListener(close);
    }

    void Update()
    {
        // update everything
        masterText.text = MasterVolume.value + "%";
        AudioListener.volume = MasterVolume.value / 100;

        MusicText.text = musicVolume.value + "%";
        music = musicVolume.value / 100;

        sfxtext.text = Sfx_Volume.value + "%";
        sfx = Sfx_Volume.value / 100;
        // TODO music i sfx kad bude audio

        if (Fullscreen.isOn == true)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        if (vsync.isOn == true)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        if (quality == 0)
        {
            QualityText.text = "LOW";
        }
        if (quality == 1)
        {
            QualityText.text = "HIGH";
        }
    }

    public void QualityUp()
    {
        quality = quality + 1;
        if (quality > 1)
        {
            quality = 0;
        }
        QualitySettings.SetQualityLevel(quality);
    }

    public void qualityDown()
    {
        quality = quality - 1;
        if (quality < 0)
        {
            quality = 1;
        }
        QualitySettings.SetQualityLevel(quality);
    }

    public void GoBack()
    {
        PlayerPrefs.SetFloat("master", MasterVolume.value);
        PlayerPrefs.SetFloat("music", musicVolume.value);
        PlayerPrefs.SetFloat("sfx", Sfx_Volume.value);
        if (Fullscreen.isOn)
            PlayerPrefs.SetInt("fullscreen", 1);
        else
            PlayerPrefs.SetInt("fullscreen", 0);
        if (vsync.isOn)
            PlayerPrefs.SetInt("vsync", 1);
        else
            PlayerPrefs.SetInt("vsync", 0);
        PlayerPrefs.SetInt("quality", quality);
        PlayerPrefs.Save();

        MainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void close()
    {
        MainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
