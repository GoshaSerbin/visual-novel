using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartNewGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }


    public void ContinueGame()
    {

    }

    public void OpenSettings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
