using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Play;
    public Button Continue;
    public Button Settings;
    public Button Quit;

    public GameObject SettingsCanvas;


    private void Start()
    {
        Play.onClick.AddListener(OnPlayClicked);
        Settings.onClick.AddListener(OpenSettings);
        Quit.onClick.AddListener(quit);
        Settings.onClick.AddListener(OpenSettings);
    }

    public void OpenSettings()
    {
        SettingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        Application.Quit();
    }
}
