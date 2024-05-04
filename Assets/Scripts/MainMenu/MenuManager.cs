using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ButtonsPanel;


    private void Start()
    {
        FindAnyObjectByType<AudioManager>()?.Play("Demo");
    }
    public void StartNewGame()
    {
        FindAnyObjectByType<AudioManager>()?.Play("ButtonClick");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }


    public void ContinueGame()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        FindAnyObjectByType<AudioManager>()?.Play("ButtonClick");
        SettingsPanel.SetActive(true);
        ButtonsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        FindAnyObjectByType<AudioManager>()?.Play("ButtonClick");
        SettingsPanel.SetActive(false);
        ButtonsPanel.SetActive(true);
    }
}
