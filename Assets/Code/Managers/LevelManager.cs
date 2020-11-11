using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    public Animator animator;
    public float transitionDelayTime = 1.0f;


    #endregion

    #region Singleton
    public static LevelManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Methods

    private void Update()
    {
        LoadLevelFromCutscene();
    }

    /// <summary>
    /// Loading in the next level.
    /// </summary>
    public void LoadNextLevel()
    {
        StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// Enumertator that makes the transition look smooth.
    /// </summary>
    /// <param name="index"></param> 
    /// <returns></returns>
    IEnumerator DelayLoadLevel(int index)
    {
        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(transitionDelayTime);
        SceneManager.LoadScene(index);
    }

    private void LoadLevelFromCutscene()
    {
        Scene cutScene1 = SceneManager.GetSceneByName("Cutscene_1");
        Scene cutScene2 = SceneManager.GetSceneByName("Cutscene_2");
        // Check if the name of the current Active Scene is your first Scene.
        if (SceneManager.GetActiveScene() == cutScene1)
        {
            CheckInput();
        }
        if (SceneManager.GetActiveScene() == cutScene2)
        {
            CheckInput();
        }

    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
    #endregion
}
