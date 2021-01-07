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
    public GameObject backButton, backButtonPause, playButton, optionsMainMenuButton, quitGameButton, continueButton, backToMainMenu, creditButton;
    public GameObject pauseMenuUI;
    public GameObject optionsPauseSoundSlider;
    private bool _pauseMenuActive = false;
    public GameObject levelLoader;
    public GameObject videoPlayer;
    private GameObject _lastSelectedGameObject, _currentSelectedGameObject;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = this;
    }

    private GameObject GetTheLastSelectedGameobject()
    {
        if (EventSystem.current.currentSelectedGameObject != _currentSelectedGameObject)
        {
            _lastSelectedGameObject = _currentSelectedGameObject;
            _currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            return _lastSelectedGameObject;
        }
        return null;
    }

    private void Update()
    {
        if (pauseMenu.activeInHierarchy)
        {
            GameObject.Find("Player").GetComponent<Move>().enabled = false;
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {

                GameObject.Find("Player").GetComponent<Move>().enabled = false;
                Time.timeScale = 0f;
                //if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                //    || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                //{
                //    if (EventSystem.current.currentSelectedGameObject == pauseResumeButton)
                //    {
                //        ResumeGame();
                //    }
                //    else if (EventSystem.current.currentSelectedGameObject == optionButton)
                //    {
                //        OpenOptionFromPause();
                //    }
                //    else if (EventSystem.current.currentSelectedGameObject == mainMenuButton)
                //    {
                //        OpenMainMenu();
                //    }
                //}
            }
        }
        if (pauseOptionMenu.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (EventSystem.current.currentSelectedGameObject == backButton)
                {
                    BackToPause();
                }
            }
        }

        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            Debug.Log("current selected: " + EventSystem.current.currentSelectedGameObject);

            if (optionsMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    OpenMainMenu();
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
                else if (EventSystem.current.currentSelectedGameObject == continueButton)
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

    private void CloseCredits()
    {
        Debug.Log("back to main");
        BackToMainMenu();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseOptionMenu.SetActive(false);
        _pauseMenuActive = true;
    }

    public void PlayGame()
    {
        levelLoader.GetComponent<LoadLevel>().LoadNextLevel();
        Time.timeScale = 1f;
        _pauseMenuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);

    }

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Quit");
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Application.wantsToQuit += WantsToQuit;
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameObject.Find("Player").GetComponent<Move>().enabled = true;
        Time.timeScale = 1f;
        _pauseMenuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    private static bool WantsToQuit()
    {
        return InputSave.Instance.WantToQuit();
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsPauseSoundSlider);
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
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetTrigger("Highlighted");
    }

    public void BackButton()
    {
        optionsMenu.SetActive(false);
        //mainMenu.SetActive(true);
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

    public void BackToMainMenu()
    {
        creditsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetTrigger("Highlighted");
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);
        videoPlayer.GetComponent<VideoPlayer>().Play();
    }

    private IEnumerator WaitToSelectBack()
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(2);
    }
    #endregion
}
