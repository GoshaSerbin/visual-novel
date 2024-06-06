using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    public static float multiplier = 50;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        _musicSlider?.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider?.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, _musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, _sfxSlider.value);
    }

    private void SetMusicVolume(float value)
    {
        _mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * multiplier);
    }

    private void SetSFXVolume(float value)
    {
        _mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * multiplier);
    }
}

