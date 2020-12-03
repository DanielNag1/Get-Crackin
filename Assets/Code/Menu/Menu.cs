using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu, mainMenu, pauseOptionMenu;
    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;
    //public GameObject customKeybindingButton, masterVolumeButton, soundEffectsButton, musicVolumeButton;
    public GameObject backButton, playButton, optionsMainMenuButton, quitGameButton, continueButton;
    private bool mainMenuActive = false, optionMenuActive = false, pauseMenuActive = false;
    public GameObject levelLoader;

    void Update()
    {
        if (optionsMenu.activeInHierarchy)
        {
            optionMenuActive = true;
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (EventSystem.current.currentSelectedGameObject == backButton)
                {
                    OpenMainMenu();
                }
                else if (EventSystem.current.currentSelectedGameObject == backButton && pauseMenuActive)
                {
                    BackButton();
                }
            }
        }

        else if (mainMenu.activeInHierarchy)
        {
            mainMenuActive = true;
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
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

    //public void PauseGame()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6)) //OBS! All input should be handled by the InputManager
    //    {
    //        //pauseMenuActive = true;
    //        mainMenuActive = false;
    //        optionMenuActive = false;
    //        //pauseMenu.SetActive(true);
    //        mainMenu.SetActive(false);
    //        optionsMenu.SetActive(false);
    //        //if (pauseMenu.activeInHierarchy)
    //        //{
    //        //    Time.timeScale = 0f;
    //        //    EventSystem.current.SetSelectedGameObject(null);
    //        //    EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    //        //}
    //    }
    //}

    public void PlayGame()
    {
        levelLoader.GetComponent<LoadLevel>().LoadNextLevel();
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("Quit");
            //pauseMenu.SetActive(false);
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Application.wantsToQuit += WantsToQuit;
        }
    }
    static bool WantsToQuit()
    {
        return InputSave.Instance.WantToQuit();
    }

    //public void ResumeButton()
    //{
    //    pauseMenu.SetActive(false);
    //    Time.timeScale = 1f;
    //}

    public void OpenOptions()
    {
        optionMenuActive = true;
        mainMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OpenMainMenu()
    {


        //if (pauseMenu.activeInHierarchy)
        //{
        //    Time.timeScale = 0;
        //    SceneManager.LoadSceneAsync("MainMenu");
        //}


        mainMenuActive = true;
        pauseMenuActive = false;
        optionMenuActive = false;
        // Pause can be activeted in main menu
        // MainActive = true;
        //pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);

    }

    public void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        //pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }


}
