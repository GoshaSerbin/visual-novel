using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioMixer _mixer;
	public static AudioManager instance;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}
		LoadVolume();
	}

	public const string MUSIC_KEY = "MusicVolume";
	public const string SFX_KEY = "SFXVolume";

	void OnEnable()
	{
		Narrator.OnSoundPlayed += PlaySound;
		Narrator.OnSoundStoped += StopSounds;
		Narrator.OnMusicPlayed += PlayMusic;
		Narrator.OnMusicStoped += StopMusic;
	}

	void OnDisable()
	{
		Narrator.OnSoundPlayed -= Play;
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}


	//for ink, silinces previous sounds
	public void PlaySound(string sound)
	{
		StopSounds();
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void PlayMusic(string sound)
	{
		StopMusic();
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	void LoadVolume() // volume saved in volume settings.cs
	{
		float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
		float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
		_mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * VolumeSettings.multiplier);
		_mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume * VolumeSettings.multiplier));
	}

	void StopSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop();
	}

	void StopSounds()
	{
		Debug.Log("STOP ALL SOUNDS!");
		foreach (var s in sounds)
		{
			if (s.mixerGroup.name == "SFX")
			{
				s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
				s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
				s.source.Stop();
			}
		}

	}

	public void StopMusic()
	{
		foreach (var s in sounds)
		{
			if (s.mixerGroup.name == "Music")
			{
				s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
				s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

				s.source.Stop();
			}

		}

	}

}