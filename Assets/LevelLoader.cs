using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f;

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        transitionAnimator.SetTrigger("Start");

        // Wait for the animation to complete
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}