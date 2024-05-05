using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ButtonsPanel;
    [SerializeField] private GameObject PlayerDataPanel;

    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private TMP_InputField _heroDescriptionInputField;

    private AudioManager _audioManger;
    private AIManager _aiManager;


    private void Awake()
    {
        _aiManager = FindObjectOfType<AIManager>();
        _audioManger = FindObjectOfType<AudioManager>();
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
        if (_heroDescriptionInputField.text == "")
        {
            _heroDescriptionInputField.text = "стильная брутальная девушка в очках и черном смокинге";
        }
        PlayerPrefs.SetString("PlayerDescription", _heroDescriptionInputField.text);
        _aiManager.GenerateImage(_heroDescriptionInputField.text, 720, 1280, "ANIME", "PlayerProfilePicture");
        _audioManger?.Play("ButtonClick");

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
}
