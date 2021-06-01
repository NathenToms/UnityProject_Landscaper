using Landscaper.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAudio
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance;

		[Header("Save Data")]
		public SaveData SaveData;

		[Header("Audio")]
		public AudioSource AudioSource_Environmental;
		public AudioSource AudioSource_Effect;
		public AudioSource AudioSource_Music;

		[SerializeField]
		private List<AudioClipData> _audioClips = new List<AudioClipData>();

		public Dictionary<AudioClipName, AudioClipData> SoundDictionary = new Dictionary<AudioClipName, AudioClipData>();

		private void Awake()
		{
			if (Instance)
			{
				Debug.LogError("There can't be 2 'Audio Managers' in the scene!", this);
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
			}

			DontDestroyOnLoad(gameObject);

			AudioSource_Environmental.volume = SaveData.EnvironmentalVolume;
			AudioSource_Effect.volume = SaveData.EffectsVolume;
			AudioSource_Music.volume = SaveData.MusicVolume;

			BuildDictionary();
		}

		public void BuildDictionary()
		{
			foreach (AudioClipData audioClipPropertie in _audioClips)
			{
				if (SoundDictionary.ContainsKey(audioClipPropertie.AudioClipName))
				{
					Debug.LogError($"This clip was already registered: Type: {audioClipPropertie.AudioClipType} Name: {audioClipPropertie.AudioClip.name}");
				}
				else
				{
					SoundDictionary.Add(audioClipPropertie.AudioClipName, audioClipPropertie);
				}
			}
		}

		public void PlayAudioClip(AudioClipData audioClipProperties)
		{
			if (audioClipProperties.AudioClipType == AudioClipType.Environmental) {
				AudioSource_Environmental.PlayOneShot(audioClipProperties.AudioClip);
			}

			if (audioClipProperties.AudioClipType == AudioClipType.Effect) {
				AudioSource_Effect.PlayOneShot(audioClipProperties.AudioClip);
			}
		}

		#region Static Methods

		public static void PlayClip(AudioClipName audioClipName)
		{
			if (ValidateSingleton())
			{
				if (Instance.SoundDictionary.ContainsKey(audioClipName)) {
					Instance.PlayAudioClip(Instance.SoundDictionary[audioClipName]);
				}
				else
				{
					Debug.LogError($"That clip was not registered! Name: {audioClipName}");
				}
			}
		}

		public static bool ValidateSingleton()
		{
			if (Instance == null)
			{
				Debug.Log("There is no 'AudioManager' in the scene!");

				return false;
			}
			else
			{
				return true;
			}
		}

		#endregion
	}
}