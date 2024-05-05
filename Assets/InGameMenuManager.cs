using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenuManager : MonoBehaviour
{
    private AudioManager _audioManger;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    void Awake()
    {
        _audioManger = FindObjectOfType<AudioManager>();
    }

    public void OpenMenu()
    {
        _audioManger?.Play("ButtonClick");
        _menuPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        _audioManger?.Play("ButtonClick");
        _menuPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        _audioManger?.Play("ButtonClick");
        _settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        _audioManger?.Play("ButtonClick");
        _settingsPanel.SetActive(false);
    }

    public void SaveAndQuit()
    {
        // save 
        //to do maybe we can load and save with num of turns variable in ink story + scene number
        _audioManger?.Play("ButtonClick");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
