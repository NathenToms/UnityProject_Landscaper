using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Pieces;
using Landscaper.UI;
using CustomAudio;


namespace Landscaper.Core
{
	public class GameManager : MonoBehaviour
	{
		[Header("References")]
		// Other
		public LandscaperBoard LandscaperBoard;
		public EncounterController EncounterController;
		public PiecePicker PiecePicker;

		// Camera
		public Transform CameraTarget;
		public Transform CameraTarget_ZoomOut;

		[Header("Notification Display")]
		public NotificationDisplay NotificationDisplay;

		[Header("Shop Menus")]
		public GamePieceShop TowerShop;
		public GamePieceShop FactoryShop;

		[Header("Timer")]
		public GameClock Clock;

		[Header("Key Binds")]
		public KeyCode EndTurnKey = KeyCode.Return;

		[Header("Game Manager")]
		public int PlayerTurn;
		public int TurnCount;
		public int PlayerCount = 2;

		public bool EnemyTurn =false;

		public void OpenTowerShop() => TowerShop.Open();
		public void OpenFactoryShop() => FactoryShop.Open();

		private void Awake()
		{
			Clock.OnTurnEnd += EndTurn;
		}

		private void Start()
		{
			StartGame();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				EndTurn();
			}
		}

		void StartGame()
		{
			Clock.StartClock();

			LandscaperBoard.PlacePiece(null, PieceType.King, new Vector2Int(5, -1), 0, out Piece pieceA);
		}

		void EndTurn()
		{
			NotificationDisplay.ShowMessage("Turn Ended", "Your turn has ended, the next player will now begin their turn.");
			LandscaperBoard.ResetBoardState();
			Clock.StopClock();

			float angle = 360f / PlayerCount;
			if (angle != 360) Tween.Instance.EaseOut_Rotation_ElasticY(CameraTarget, CameraTarget.eulerAngles.y, CameraTarget.eulerAngles.y + angle, 1.0f);

			CameraTarget_ZoomOut.gameObject.SetActive(true);

			EnemyTurn = true;
			EncounterController.TakeTurn(() =>
			{
				TurnCount++;

				PlayerTurn = (PlayerTurn + 1) % PlayerCount;

				Clock.ResetClock();
				Clock.StartClock();

				var player = FindObjectOfType<Player>();

				if (player.ID == PlayerTurn) {
					player.ResourceManager.MaxMovePoints++;

					NotificationDisplay.ShowMessage($"Its Your Turn", $"You started a new turn, You now have {player.ResourceManager.MovePoints} movement points.");
				}

				player.ResourceManager.ResetPoints();

				FactoryShop.Close();
				TowerShop.Close();

				foreach (Piece piece in LandscaperBoard.PieceDictionary.Values)
				{
					piece.HasActed = false;

					if (piece.PieceType == PieceType.Mine)
					{
						player.ResourceManager.MovePoints++;
					}
				}

				EnemyTurn = false;
				CameraTarget_ZoomOut.gameObject.SetActive(false);
			});
		}

		public void ButItem(Player player, Piece piecePrefab, int cost)
		{
			if (player.ResourceManager.MovePoints >= cost)
			{
				player.ResourceManager.MovePoints -= cost;
				player.PieceManager.PieceCountDictionary[piecePrefab.PieceType]++;

				AudioManager.PlayClip(AudioClipName.Ding);
			}
			else
			{
				AudioManager.PlayClip(AudioClipName.Nope);
			}
		}
	}
}