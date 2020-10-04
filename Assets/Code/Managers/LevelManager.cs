using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Variables
    private TriggerComponent triggerComp;
    public NavMeshSurface navMeshSurface;
    public Animator animator;
    public float transitionDelayTime = 1.0f;
    #endregion
    #region Methods

    private void Awake()
    {
        animator = GameObject.Find("Transition").GetComponent<Animator>();
        triggerComp = GameObject.FindGameObjectWithTag("Trigger").GetComponent<TriggerComponent>();
    }

    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }
    private void Update()
    {
        if (triggerComp.isTriggered == true) // Checks if the player has activated the triggercomponent.
        {
            LoadNextLevel();
        }
    }

    /// <summary>
    /// Loading in the next level.
    /// </summary>
    void LoadNextLevel()
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
    #endregion
}
