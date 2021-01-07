﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public Animator animator;
    public float transitionTime = 1f;


    void Awake()
    {

    }


    void Update()
    {

    }

    public void LoadNextLevel()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadNewLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMainMenu()
    {
        animator.SetTrigger("Start");
        StartCoroutine(LoadNewLevel(SceneManager.GetActiveScene().buildIndex - 2));
    }

    IEnumerator LoadNewLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(levelIndex);
    }
}
