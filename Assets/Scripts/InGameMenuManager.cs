using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InGameMenuManager : MonoBehaviour
{

    private bool IsMenuOpened = false;
    private AudioManager _audioManger;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;

    private LvlLoader _lvlLoader;
    void Awake()
    {
        _audioManger = FindObjectOfType<AudioManager>();
        _lvlLoader = FindObjectOfType<LvlLoader>();
    }

    public void OpenMenu()
    {
        IsMenuOpened = true;
        _audioManger?.Play("ButtonClick");
        _menuPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        IsMenuOpened = false;
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
        FindObjectOfType<Narrator>()?.SaveStoryProgress();
        _audioManger?.Play("ButtonClick");
        IsMenuOpened = false;
        if (_lvlLoader != null)
        {
            _lvlLoader.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            if (IsMenuOpened)
            {
                ContinueGame();
            }
            else
            {
                OpenMenu();
            }
        }
    }
}
