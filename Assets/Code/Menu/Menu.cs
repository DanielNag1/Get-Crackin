using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu, mainMenu;

    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;

    public GameObject customKeybindingButton, masterVolumeButton, soundEffectsButton, musicVolumeButton;

    public GameObject backButton, newGameButton, optionsMainMenuButton, quitGameButton, continueButton;

    private bool mainMenuActive = false, optionMenuActive = false, pauseMenuActive = false;


    void Update()
    {
        PauseGame();

        if (pauseMenu.activeInHierarchy)
        {
            pauseMenuActive = true;
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (EventSystem.current.currentSelectedGameObject == pauseResumeButton)
                {
                    ResumeButton();
                }
                else if (EventSystem.current.currentSelectedGameObject == optionButton)
                {
                    OpenOptions();
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

        else if (optionsMenu.activeInHierarchy)
        {
            optionMenuActive = true;
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (EventSystem.current.currentSelectedGameObject == backButton && mainMenuActive)
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
                if (EventSystem.current.currentSelectedGameObject == newGameButton)
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

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            //pauseMenuActive = true;
            //mainMenuActive = false;
            //optionMenuActive = false;

            pauseMenu.SetActive(true);
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
            if (pauseMenu.activeInHierarchy)
            {
                Time.timeScale = 0f;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseResumeButton);
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        mainMenu.SetActive(false);
        mainMenuActive = false;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("Quit");
            pauseMenu.SetActive(false);
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Application.wantsToQuit += WantsToQuit;
        }
    }
    static bool WantsToQuit()
    {
        return InputSave.Instance.WantToQuit();

    }
    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenOptions()
    {
        optionMenuActive = true;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("MainMenu");

        mainMenuActive = true;
        pauseMenuActive = false;
        optionMenuActive = false;
        // Pause can be activeted in main menu
        // MainActive = true;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton);

    }

    public void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }
}