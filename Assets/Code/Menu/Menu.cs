using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    public GameObject pauseMenu, optionsMenu;

    public GameObject pauseResumeButton, optionButton, mainMenuButton, quitToDesktopButton;

    public GameObject customKeybindingButton, masterVolumeButton, soundEffectsButton, musicVolumeButton, backButton;

   

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire3"))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;

            //Clear seletion
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(pauseResumeButton);

        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            optionsMenu.SetActive(false);
        }

    }

    public void OpenOptions()

    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(customKeybindingButton);
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }


   
}
