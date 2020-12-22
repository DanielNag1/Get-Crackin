using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    #region Variables
    public GameObject pauseMenu, optionsMenu, mainMenu, pauseOptionMenu;
    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;
    public GameObject backButton, playButton, optionsMainMenuButton, quitGameButton, continueButton;
    private bool  _pauseMenuActive = false;
    public GameObject levelLoader;
    #endregion

    #region Method
    private void Update()
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
                else if (EventSystem.current.currentSelectedGameObject == backButton && _pauseMenuActive)
                {
                    BackButton();
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

    public void OpenMainMenu()
    {
        _pauseMenuActive = false;
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
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
    #endregion
}