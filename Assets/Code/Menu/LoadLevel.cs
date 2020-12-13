using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Animator animator;
    public float transitionTime = 1f;
    //Do we want to be able to load a specific level as well maby?
    public void LoadNextLevel()
    {
        StartCoroutine(LoadNewLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadNewLevel(int levelIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(levelIndex);
    }
}
