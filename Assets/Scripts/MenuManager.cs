using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject optionsMenuCanvas;
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;

    private bool isMenuActive = true;

    void Start()
    {
        // Get references to the buttons
        playButton = GameObject.Find("Play").GetComponent<Button>();
        optionsButton = GameObject.Find("Option").GetComponent<Button>();
        quitButton = GameObject.Find("Quit").GetComponent<Button>();

        // Add event listeners to the buttons
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptions);
        quitButton.onClick.AddListener(QuitGame);

        // Ensure the pause menu and options menu are initially hidden
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (optionsMenuCanvas != null)
        {
            optionsMenuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // If the menu is active, pause the game
        Time.timeScale = isMenuActive ? 0 : 1;
    }

    void PlayGame()
    {
        // Hide the menu and start the game
        isMenuActive = false;
        menuCanvas.SetActive(false);

        // Ensure the pause menu and options menu are hidden
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (optionsMenuCanvas != null)
        {
            optionsMenuCanvas.SetActive(false);
        }

        Time.timeScale = 1;
    }

    void ShowOptions()
    {
        menuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}