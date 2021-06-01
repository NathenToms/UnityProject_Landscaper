using UnityEngine;

namespace CustomAudio
{
	[System.Serializable]
	public class AudioClipData
	{
		// the Name of the audio clip we are going to play
		public AudioClipName AudioClipName;

		// The type of clip we are going to play
		public AudioClipType AudioClipType;

		// The Audio clip we play
		public AudioClip AudioClip;
	}
}