using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Menu : MonoBehaviour
{
    #region singleton
    public static Menu Instance { get; private set; }
    #endregion

    #region Variables
    public GameObject pauseMenu, optionsMenu, mainMenu, pauseOptionMenu, creditsMenu;
    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;
    public GameObject backButtonPause, playButton, optionsMainMenuButton, quitGameButton, backToMainMenu, creditButton;
    public GameObject optionsPauseSoundSlider;
    public GameObject levelLoader;
    public GameObject videoPlayer;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (pauseOptionMenu.activeInHierarchy)
        {
            GameObject.Find("Player").GetComponent<Move>().enabled = false;
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                BackToPause();
            }
        }
        if (pauseMenu.activeInHierarchy)
        {
            EvaluateActivePauseMenuButton();

            GameObject.Find("Player").GetComponent<Move>().enabled = false;
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject.Find("Player").GetComponent<Move>().enabled = false;
                Time.timeScale = 0f;

                if (EventSystem.current.currentSelectedGameObject == pauseResumeButton)
                {
                    ResumeGame();
                }
                else if (EventSystem.current.currentSelectedGameObject == optionButton)
                {
                    OpenOptionFromPause();
                }
                else if (EventSystem.current.currentSelectedGameObject == mainMenuButton)
                {
                    OpenMainMenu();
                }
                else if (EventSystem.current.currentSelectedGameObject == quitToDesktopButton)
                {
                    ExitGame();
                }
            }
        }

        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            EvaluateActiveMainMenuButton();

            if (optionsMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    BackToMainMenu();
                }
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Key pressed");
                if (EventSystem.current.currentSelectedGameObject == playButton)
                {
                    Debug.Log("play game");
                    PlayGame();
                }
                else if (EventSystem.current.currentSelectedGameObject == optionsMainMenuButton)
                {
                    Debug.Log("open options");
                    OpenOptions();
                }
                else if (EventSystem.current.currentSelectedGameObject == creditButton)
                {
                    Debug.Log("Credits open");
                    OpenCredits();
                }
                else if (EventSystem.current.currentSelectedGameObject == quitGameButton)
                {
                    ExitGame();
                }
            }

            if (creditsMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CloseCredits();
                }
            }
        }
    }

    #region Mainmenu

    public void PlayGame()
    {
        levelLoader.GetComponent<LoadLevel>().LoadNextLevel();
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsPauseSoundSlider);
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);
        videoPlayer.GetComponent<VideoPlayer>().Play();
    }

    private void CloseCredits()
    {
        Debug.Log("back to main");
        BackToMainMenu();
    }

    public void BackToMainMenu()
    {
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    #endregion

    #region Pausemenu

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameObject.Find("Player").GetComponent<Move>().enabled = true;
        Time.timeScale = 1f;
    }

    public void OpenOptionFromPause()
    {
        pauseMenu.SetActive(false);
        pauseOptionMenu.SetActive(true);
        optionsPauseSoundSlider.GetComponent<Slider>().value = SoundEngine.Instance.SetMasterVolume;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButtonPause);
    }

    public void OpenMainMenu()
    {
        pauseOptionMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelLoader.GetComponent<LoadLevel>().LoadMainMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void BackToPause()
    {
        pauseOptionMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    #endregion

    #region EvaluateButtons

    private void EvaluateActiveMainMenuButton()
    {
        Debug.Log("Current selected: " + EventSystem.current.currentSelectedGameObject);
        if (playButton != EventSystem.current.currentSelectedGameObject)
        {
            playButton.GetComponent<Animator>().Play("Normal");
        }
        if (optionsMainMenuButton != EventSystem.current.currentSelectedGameObject)
        {
            optionsMainMenuButton.GetComponent<Animator>().Play("Normal");
        }
        if (creditButton != EventSystem.current.currentSelectedGameObject)
        {
            creditButton.GetComponent<Animator>().Play("Normal");
        }
        if (quitGameButton != EventSystem.current.currentSelectedGameObject)
        {
            quitGameButton.GetComponent<Animator>().Play("Normal");
        }
    }

    private void EvaluateActivePauseMenuButton()
    {
        Debug.Log("Current selected: " + EventSystem.current.currentSelectedGameObject);
        if (pauseResumeButton != EventSystem.current.currentSelectedGameObject)
        {
            pauseResumeButton.GetComponent<Animator>().Play("Normal");
        }
        if (optionButton != EventSystem.current.currentSelectedGameObject)
        {
            optionButton.GetComponent<Animator>().Play("Normal");
        }
        if (mainMenuButton != EventSystem.current.currentSelectedGameObject)
        {
            mainMenuButton.GetComponent<Animator>().Play("Normal");
        }
        if (quitToDesktopButton != EventSystem.current.currentSelectedGameObject)
        {
            quitToDesktopButton.GetComponent<Animator>().Play("Normal");
        }
    }

    #endregion

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseOptionMenu.SetActive(false);
    }

    #region ExitGame

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Quit");
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            Application.wantsToQuit += WantsToQuit;
        }
    }

    private static bool WantsToQuit()
    {
        return InputSave.Instance.WantToQuit();
    }

    #endregion

    #endregion
}
