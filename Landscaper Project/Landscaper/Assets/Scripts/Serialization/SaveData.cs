using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Save
{
	[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData", order = 1)]

	public class SaveData : ScriptableObject
	{
		[Range(0, 1f)] public float MasterVolume = 1;

		[SerializeField, Range(0, 1f)] private float evironmentalVolume = 1;
		[SerializeField, Range(0, 1f)] private float effectsVolume = 1;
		[SerializeField, Range(0, 1f)] private float musicVolume = 1;

		public float EffectsVolume
		{
			get { return effectsVolume * MasterVolume; }
			set { effectsVolume = value; }
		}

		public float EnvironmentalVolume
		{
			get { return evironmentalVolume * MasterVolume; }
			set { evironmentalVolume = value; }
		}

		public float MusicVolume
		{
			get { return musicVolume * MasterVolume; }
			set { musicVolume = value; }
		}
	}
}