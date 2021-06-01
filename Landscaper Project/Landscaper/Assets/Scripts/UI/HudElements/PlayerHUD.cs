using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Landscaper.Core;
using Landscaper.Players;


namespace Landscaper.UI.HUD
{
	public class PlayerHUD : MonoBehaviour
	{
		[Header("TMP References")]
		[SerializeField] private TextMeshProUGUI _timeRemainingText = null;
		[SerializeField] private TextMeshProUGUI _turnNumberText = null;
		[SerializeField] private TextMeshProUGUI _movePointsText = null;

		[Header("Percentage Anchor References")]
		[SerializeField] private Image _percentOfMovePointsRemaining = null;
		[SerializeField] private Image _percentOfTimeRemaining = null;

		[Header("Object References")]
		[SerializeField] private GameClock _gameClock = null;

		[Header("Player HUD")]
		public bool Linked = false;

		private PlayerResourceManager resourceManager;

		public void Link(PlayerResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;

			Linked = true;
		}

		private void FixedUpdate()
		{
			if (Linked)
			{
				// TIMER
				UpdateTimerProperties();

				// MOVE POINTS
				UpdateMovePointProperties();
			}
		}

		void UpdateMovePointProperties()
		{
			float percent = (float)resourceManager.MovePoints / resourceManager.MaxMovePoints;
			float currentPoints = resourceManager.MovePoints;
			float maximumPoints = resourceManager.MaxMovePoints;

			Color min = Color.red;
			Color max = (Color.blue + Color.white) / 2;

			// MOVE POINTS
			_movePointsText.text = GetMovePointsText(currentPoints, maximumPoints);

			// PERCENTS
			_percentOfMovePointsRemaining.transform.localScale = new Vector3(percent, 1, 1);

			// COLORS
			_percentOfMovePointsRemaining.color = Color.Lerp(min, max, percent);
		}

		void UpdateTimerProperties()
		{
			float timeRemaininPercent = (float)_gameClock.TimeRemaining / _gameClock.TurnDuration;

			Color minColor = (Color.red);
			Color maxColor = (Color.green + Color.white) / 2;

			// TURN NUMBER
			_turnNumberText.text = GetTurnNumberText(_gameClock.GameManager.TurnCount);

			// TIMER
			_timeRemainingText.text = GetTimeRemainingText(_gameClock.TimeRemaining);

			// PERCENTS
			_percentOfTimeRemaining.transform.localScale = new Vector3(timeRemaininPercent, 1, 1);

			// COLORS
			_percentOfTimeRemaining.color = Color.Lerp(minColor, maxColor, timeRemaininPercent);
		}

		string GetMovePointsText(float currentPoints, float maximumPoints) => $"{currentPoints} / {maximumPoints}";

		string GetTimeRemainingText(float timeRamining) => $"Timer: {timeRamining.ToString("#0:00")}s";

		string GetTurnNumberText(int turnNumber) => $"Turn: {turnNumber}";

	}
}