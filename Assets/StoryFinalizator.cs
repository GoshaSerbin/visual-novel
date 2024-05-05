using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryFinalizator : MonoBehaviour
{
    public string NextSceneName;

    void OnEnable()
    {
        Narrator.OnStoryEnded += LoadScene;
    }

    void OnDisable()
    {
        Narrator.OnStoryEnded -= LoadScene;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(NextSceneName, LoadSceneMode.Single);
    }
}
