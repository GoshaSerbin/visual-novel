using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1f;
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelWithIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevelWithIndex(int levelIndex)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevelWithName(name));
    }

    IEnumerator LoadLevelWithName(string name)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(name);
    }
}
