using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class EndCutscene : MonoBehaviour
{
    #region Variables
    private PlayableDirector director;
    #endregion
    #region Methods

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    private void Update()
    {
        if (director.state != PlayState.Playing)
        {
            EndScene();
        }
    }

    private void EndScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
    #endregion
}
