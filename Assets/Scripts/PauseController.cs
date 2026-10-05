using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject settingsMenu;

    public Button Resume;
    public Button SettingsBtn;
    public Button main_menu;

    public bool paused = false;

    void Start()
    {
        PausePanel.SetActive(false);
        settingsMenu.SetActive(false);

        Resume.onClick.AddListener(resume);
        SettingsBtn.onClick.AddListener(OpenSettings);
        main_menu.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (paused == false)
            {
                paused = true;
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                paused = false;
                PausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void resume()
    {
        paused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
