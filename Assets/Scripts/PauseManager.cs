using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public Button restartButton;
    public Button optionsButton;
    public Button quitButton;
    public GameObject optionsMenuCanvas; 

    private bool isPaused = false;

    void Start()
    {
        // Get references to buttons and ensure menus are initially hidden.
        restartButton = pauseMenuCanvas.transform.Find("Restart Button").GetComponent<Button>();
        optionsButton = pauseMenuCanvas.transform.Find("Option Button").GetComponent<Button>();
        quitButton = pauseMenuCanvas.transform.Find("Quit Button").GetComponent<Button>();
        Button closeOptionsButton = optionsMenuCanvas.transform.Find("Close Button").GetComponent<Button>();

        restartButton.onClick.AddListener(RestartGame);
        optionsButton.onClick.AddListener(ShowOptions);
        quitButton.onClick.AddListener(QuitGame);
        closeOptionsButton.onClick.AddListener(CloseOptionsMenu);

        pauseMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseMenuCanvas.SetActive(false); // Deactivate after scene reload.
    }

    public void ShowOptions()
    {
        pauseMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}