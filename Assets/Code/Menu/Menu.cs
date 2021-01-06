﻿using UnityEngine;
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
    public GameObject backButton, backButtonPause, playButton, optionsMainMenuButton, quitGameButton, continueButton, backToMainMenu;
    public GameObject pauseMenuUI;
    public Slider optionsPauseSoundSlider;
    private bool _pauseMenuActive = false;
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
            if (optionsMenu.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1)
                    || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (EventSystem.current.currentSelectedGameObject == backButton)
                    {
                        OpenMainMenu();
                    }
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
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OpenOptionFromPause()
    {
        pauseMenu.SetActive(false);
        pauseOptionMenu.SetActive(true);
        optionsPauseSoundSlider.value = SoundEngine.Instance.SetMasterVolume;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButtonPause);
    }

    public void OpenMainMenu()
    {
        _pauseMenuActive = false;
        pauseOptionMenu.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
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

    public void BackToMainMenu()
    {
        creditsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void OpenCredits()
    {
        creditsMenu.SetActive(true);
        videoPlayer.GetComponent<VideoPlayer>().Play();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backToMainMenu);
    }
    #endregion
}
