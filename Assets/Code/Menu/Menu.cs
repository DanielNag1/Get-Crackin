﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    public GameObject pauseMenu, optionsMenu, mainMenu;

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
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);

            Time.timeScale = 0f;

            //Clear seletion
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(pauseResumeButton);
                
        }
        else
        {
            pauseMenu.SetActive(false);
            //mainMenu.SetActive(false);
            //optionsMenu.SetActive(false);
            Time.timeScale = 1f;

        }

    }

    public void OpenOptions()

    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(customKeybindingButton);
    }
    public void CloseOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(false);
        //Clears selection
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(pauseResumeButton);
    }

    public void MainMenuButton()
    {

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

    }

    public void ResumeButton()
    {
        PauseUnpause();
    
    
    }
    public void BackButton()
    {
        PauseUnpause();

    }
}