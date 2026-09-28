using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Esc (or gamepad Start) toggles the pause screen. Freezes time and audio while paused.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] string mainMenuScene = "Main";

    [Header("Buttons")]
    [SerializeField] UnityEngine.UI.Button resumeButton;
    [SerializeField] UnityEngine.UI.Button settingsButton;
    [SerializeField] UnityEngine.UI.Button mainMenuButton;
    [SerializeField] UnityEngine.UI.Button settingsBackButton;
    [SerializeField] UnityEngine.UI.Button settingsCloseButton;

    bool isPaused;
    CursorLockMode previousLockState;
    bool previousCursorVisible;

    void Awake()
    {
        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        settingsBackButton.onClick.AddListener(CloseSettings);
        settingsCloseButton.onClick.AddListener(CloseSettings);

        pausePanel.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void Update()
    {
        bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool startPressed = Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame;
        if (!escPressed && !startPressed) return;

        if (settingsMenu.activeSelf) CloseSettings();
        else if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        previousLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pausePanel.SetActive(true);
        Select(resumeButton);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisible;

        pausePanel.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pausePanel.SetActive(true);
        Select(settingsButton);
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    void OnDestroy()
    {
        // Never leave the game frozen if this object goes away while paused
        if (isPaused)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }

    static void Select(UnityEngine.UI.Button button)
    {
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
