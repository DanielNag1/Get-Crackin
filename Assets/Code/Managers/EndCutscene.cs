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
        else if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Mouse0))
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
