using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.UI;


namespace Landscaper.Core
{
	public delegate void OnTurnEndDelegate();

	public class GameClock : MonoBehaviour
	{
		public GameManager GameManager;

		public float TimeRemaining = 0;
		public float TurnDuration = 30;

		private bool pauseTimer = true;

		public OnTurnEndDelegate OnTurnEnd;

		public void StartClock() => pauseTimer = false;
		public void StopClock() => pauseTimer = true;


		// Start is called before the first frame update
		void Start()
		{
			ResetClock();
		}

		// Update is called once per frame
		void Update()
		{
			if (!pauseTimer)
			{
				TimeRemaining -= Time.deltaTime;

				if (TimeRemaining < 0)
				{
					TimeRemaining = TurnDuration;
					OnTurnEnd?.Invoke();
				}
			}
		}

		public void ResetClock()
		{
			TimeRemaining = TurnDuration;
		}
	}
}