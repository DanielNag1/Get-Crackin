using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #region singleton
    public static Menu Instance { get; private set; }
    #endregion

    #region Variables
    public GameObject pauseMenu, optionsMenu, mainMenu, pauseOptionMenu;
    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;
    public GameObject backButton, backButtonPause, playButton, optionsMainMenuButton, quitGameButton, continueButton;
    public GameObject pauseMenuUI;
    private bool _pauseMenuActive = false;
    public GameObject levelLoader;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            PauseGame();
            if (pauseMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                    || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (EventSystem.current.currentSelectedGameObject == pauseResumeButton)
                    {
                        ResumeGame();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == pauseOptionMenu)
                    {
                        OpenOptionFromPause();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == mainMenuButton)
                    {
                        OpenMainMenu();
                    }
                }

            }
            else if (pauseOptionMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                    || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (EventSystem.current.currentSelectedGameObject == backButtonPause && pauseMenu.activeInHierarchy)
                    {
                        BackToPause();
                    }
                }
            }
        }

        else if(SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            if (optionsMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                    || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (EventSystem.current.currentSelectedGameObject == backButton)
                    {
                        OpenMainMenu();
                    }
                    //else if (EventSystem.current.currentSelectedGameObject == backButtonPause && pauseMenu.activeInHierarchy)
                    //{
                    //    BackToPause();
                    //}
                }
            }

            else if (mainMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (EventSystem.current.currentSelectedGameObject == playButton)
                    {
                        PlayGame();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == optionsMainMenuButton)
                    {
                        OpenOptions();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == quitGameButton)
                    {
                        ExitGame();
                    }
                    else if (EventSystem.current.currentSelectedGameObject == continueButton)
                    {
                        ExitGame();
                    }
                }
            }
        }
    }

    private void PauseGame()
    {
        mainMenu.SetActive(false);
        pauseOptionMenu.SetActive(false);
        if (pauseMenu.activeInHierarchy)
        {
            GameObject.Find("Player").GetComponent<Move>().enabled = false;
            //Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseResumeButton);
        }
    }

    private void GamePaused()
    {
        GameObject.Find("Player").GetComponent<Move>().enabled = false;
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    public void PlayGame()
    {
        levelLoader.GetComponent<LoadLevel>().LoadNextLevel();
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Quit");
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Application.wantsToQuit += WantsToQuit;
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameObject.Find("Player").GetComponent<Move>().enabled = true;
        Time.timeScale = 1f;
    }

    private static bool WantsToQuit()
    {
        return InputSave.Instance.WantToQuit();
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OpenOptionFromPause()
    {
        pauseOptionMenu.SetActive(true);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButtonPause);
    }

    public void OpenMainMenu()
    {
        _pauseMenuActive = false;
        pauseOptionMenu.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
        //mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    public void BackToPause()
    {
        pauseOptionMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }
    #endregion
}