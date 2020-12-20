//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using static UnityEngine.Rendering.DebugUI;

//public class PauseMenu : MonoBehaviour
//{ 
//    #region singleton
//    public static PauseMenu Instance { get; private set; }
//    #endregion

//    #region variables
//    public static bool gameIsPaused = false;
//    public GameObject pauseMenuUI;
//    public GameObject resumeButtonUI;

//    #endregion
//    #region Methods
//    private void Awake()
//    {
//        Instance = this;
//    }
//    public void Resume()
//    {
//        pauseMenuUI.SetActive(false);
//        Time.timeScale = 1f;
//        gameIsPaused = false;
//        GameObject.Find("Player").GetComponent<Move>().enabled = true;
//    }

//    public void Pause()
//    {
//        pauseMenuUI.SetActive(true);
//        Time.timeScale = 0f;
//        GameObject.Find("Player").GetComponent<Move>().enabled = false;
//        gameIsPaused = true;
//        EventSystem.current.SetSelectedGameObject(null);
//        //EventSystem.current.SetSelectedGameObject(pauseResumeButton);
//    }

//    public void LoadMainMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadSceneAsync("MainMenu");
//    }
//    #endregion
//}
