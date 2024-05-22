using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System;
using UnityEditor;
using Inventory;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private GameObject PlayerDataPanel;

    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private TMP_InputField _playerDescriptionInputField;
    [SerializeField] private TMP_InputField _playerNameInputField;

    [SerializeField] private Inventory.Model.InventorySO _playerInventoryData;

    private AudioManager _audioManger;
    private AIManager _aiManager;


    [SerializeField] private GameObject ContinueButton;


    private void Awake()
    {
        _aiManager = FindObjectOfType<AIManager>();
        _audioManger = FindObjectOfType<AudioManager>();
        int SavedStoryProgress = PlayerPrefs.GetInt("SavedStoryProgress", 0);
        if (SavedStoryProgress != 0)
        {
            Debug.Log("Can continue Story");
            ContinueButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.Log("Can not continue Story");
            ContinueButton.GetComponent<Button>().interactable = false;
        }
    }

    private void Start()
    {
        float value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        _mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(value) * 20);
        value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        _mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(value) * 20);
        _audioManger?.Play("Demo");
    }
    public void StartNewGame()
    {
        if (_playerDescriptionInputField.text == "" || _playerNameInputField.text == "")
        {
            return;
        }
        DeleteAllProgress();
        PlayerPrefs.SetString("PlayerDescription", _playerDescriptionInputField.text);
        PlayerPrefs.SetString("PlayerName", _playerNameInputField.text);
        _aiManager.GenerateImage(_playerDescriptionInputField.text, 720, 1280, "ANIME", "PlayerProfilePicture", true);
        _audioManger?.Play("ButtonClick");

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("SavedStoryProgress", 0);
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void DeleteAllProgress()
    {
        // only settings that remains
        float musicVolume = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicVolume);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxVolume);

        // FileUtil.DeleteFileOrDirectory(Application.persistentDataPath);
        _playerInventoryData.Initialize();
    }

    public void ContinueGame()
    {
        string SavedScene = PlayerPrefs.GetString("SavedScene", "");
        SceneManager.LoadScene(SavedScene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        _audioManger?.Play("ButtonClick");
        SettingsPanel.SetActive(true);
        ButtonsPanel.SetActive(false);
    }

    public void OpenPlayerData()
    {
        _audioManger?.Play("ButtonClick");
        PlayerDataPanel.SetActive(true);
        ButtonsPanel.SetActive(false);
    }

    public void ClosePlayerData()
    {
        _audioManger?.Play("ButtonClick");
        PlayerDataPanel.SetActive(false);
        ButtonsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _audioManger?.Play("ButtonClick");
        SettingsPanel.SetActive(false);
        ButtonsPanel.SetActive(true);
    }

    public void StartPlayAIDungeon()
    {
        _audioManger?.Play("ButtonClick");
        SceneManager.LoadScene("AIDungeon");
    }
}
