using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Pieces;
using Landscaper.Tiles;
using Landscaper.Players;
using Landscaper.UI;
using CustomAudio;
using System;
using Landscaper.Chunks;

namespace Landscaper.Core
{
	public class LandscaperBoard : MonoBehaviour
	{
		public static int NO_OVERIDE_ID = -1;

		#region Landscaper Board Functions/Information

		// Dictionary for Tiles and Pieces

		// The Selected Piece
		// The Hovered Piece

		// What can the board do?
		// 1) Move Piece
		// 2) Take Piece
		// 3) Place Piece
		// 4) Upgrade Piece

		// Show Board information
		// 5) Display Move Options (OnHover)
		// 6) Highlight Move Options

		#endregion

		[Header("References")]
		public SelectedPieceActionsDisplay ActionsDisplay;
		public NotificationDisplay NotificationDisplay;
		public GameManager GameManager;

		// Public Members
		public Dictionary<Vector2Int, Piece> PieceDictionary = new Dictionary<Vector2Int, Piece>();
		public Dictionary<Vector2Int, Tile> TileDictionary = new Dictionary<Vector2Int, Tile>();

		public ISelectable HoverTarget, SelectedTarget;

		[Header("Landscaper Board")]
		public bool GameRunning;
		public bool OnlyActOnce;

		public List<Color> PlayerColors = new List<Color>();

		// Private Members
		private List<Tile> highlightedMoves;

		private Piece selectedPiece;

		// Properties
		public Piece SelectedPiece
		{
			get { return selectedPiece; }
			set { selectedPiece = value;
				ActionsDisplay.Init(value);
			}
		}

		// Get the highlighted move count
		public int HighlightedMoveCount { get { return highlightedMoves.Count; } }

		private void Awake() {
			foreach (Tile tile in FindObjectsOfType<Tile>()) {
				tile.Initialized(GameManager);
			}

			highlightedMoves = new List<Tile>();
		}

		public void EndGame(int winnerID)
		{
			NotificationDisplay.ShowNewMessage("GAME OVER!", $"Player {winnerID} was the winner!");

			GameManager.Clock.StopClock();
		}

		#region Public Methods

		// Properties
		public WorldState WorldState
		{
			get { return new WorldState()
			{
				CurrentTurnID = GameManager.PlayerTurn,
				PieceDictionary = PieceDictionary,
				TileDictionary = TileDictionary,
				Selected = SelectedTarget,
				Hovered = HoverTarget,
				SelectedPiece = selectedPiece,
			};}
		}

		// Click a target
		public void ClickTile(ISelectable selectable) => SelectedTarget = selectable;

		// Hover a target
		public void HoverTile(ISelectable selectable) => HoverTarget = selectable;

		// Register a Tile to join the TileDictionary
		public void RegisterTile(Tile tile)
		{
			if (!TileDictionary.ContainsKey(tile.TileCoordinates)) {
				TileDictionary.Add(tile.TileCoordinates, tile);
			}
		}

		// Place a piece on the board
		public bool PlacePiece(Player player, PieceType pieceType, Vector2Int coordinates, int ownerID, out Piece piece, bool makeSound = true)
		{
			if (PieceManager.GetPiece(pieceType, out Piece piecePrefab))
			{
				if (player && player.ResourceManager.MovePoints < piecePrefab.MoveCost)
				{
					AudioManager.PlayClip(AudioClipName.Nope);

					piece = null;
					return false;
				}

				if (player && player.PieceManager.PieceCountDictionary[pieceType] == 0)
				{
					AudioManager.PlayClip(AudioClipName.Nope);

					piece = null;
					return false;
				}

				if (player)
				{
					player.PieceManager.PieceCountDictionary[pieceType]--;
				}

				// Instantiate the prefab and select it
				SelectedPiece = Instantiate(piecePrefab);
				SelectedPiece.OwnerID = ownerID;

				piece = selectedPiece;

				SetPieceColor(SelectedPiece, ownerID);

				// If we took the target tiles, could we fit if we moved there..
				if (CanFitPiece(coordinates, piecePrefab.CoordinatesRequired, false))
				{
					ValidateRequiredCoordinates(piecePrefab.CoordinatesRequired);
					foreach (Vector2Int coordinateOffset in piecePrefab.CoordinatesRequired) {
						PieceDictionary.Add(coordinates + coordinateOffset, SelectedPiece);
					}

					SelectedPiece.transform.SetParent(TileDictionary[coordinates].ChildAnchor, false);

					if (makeSound) {
						AudioManager.PlayClip(AudioClipName.Poof);
					}

					if (player)
					{
						player.ResourceManager.MovePoints -= piece.MoveCost;
					}

					return true;
				}
				else
				{
					// If there is no room, destroy it
					Destroy(SelectedPiece);
				}
			}

			piece = null;
			return false;
		}

		// Public Methods
		// Move a game Piece to a target Tile.
		public void MovePiece(Player player, Piece piece, Tile targetTile)
		{
			SelectedPiece = piece;

			ResetBoardState();

			if (player && player.ResourceManager.MovePoints < piece.MoveCost)
			{
				AudioManager.PlayClip(AudioClipName.Nope);
				return;
			}

			if (OnlyActOnce && piece.HasActed)
			{
				return;
			}

			// Check if this is a possible move
			if (CanMovePiece(targetTile.TileCoordinates, piece.CoordinatesRequired))
			{
				piece.HasActed = true;
				piece.MovePiece(targetTile, WorldState);

				if (player)
				{
					player.ResourceManager.MovePoints -= piece.MoveCost;
				}
			}
		}

		// Take a game Piece at a target Tile.
		public void CapturePiece(Player player, Piece attackingPiece, Tile targetTile)
		{
			SelectedPiece = attackingPiece;

			if (player && player.ResourceManager.MovePoints < attackingPiece.MoveCost)
			{
				AudioManager.PlayClip(AudioClipName.Nope);
				return;
			}

			if (OnlyActOnce && attackingPiece.HasActed) return;

			// If we took the target tiles, could we fit if we moved there..
			if (CanFitPiece(targetTile.TileCoordinates, attackingPiece.CoordinatesRequired))
			{
				PieceType targetType = PieceDictionary[targetTile.TileCoordinates].PieceType;

				ResetBoardState();
				attackingPiece.HasActed = true;
				attackingPiece.TakePiece(targetTile, WorldState);

				if (targetType == PieceType.King) {
					EndGame(attackingPiece.OwnerID);
				}

				if (player)
				{
					player.ResourceManager.MovePoints -= attackingPiece.MoveCost;
				}
			}
		}

		// Upgrade a game piece
		public void UpgradePiece(Player player, Piece target)
		{
			if (player && player.ResourceManager.MovePoints < target.MoveCost)
			{
				AudioManager.PlayClip(AudioClipName.Nope);
				return;
			}

			if (target.UpgradePiecePrefab.PieceType == PieceType.Mine)
			{
				if (!TileDictionary[target.ParentTile.TileCoordinates].IsMineable)
				{
					AudioManager.PlayClip(AudioClipName.Nope);

					return;
				}
			}

			if (OnlyActOnce && target.HasActed) return;

			if (CanFitPiece(target.ParentTile.TileCoordinates, target.UpgradePiecePrefab.CoordinatesRequired))
			{
				ResetBoardState();

				// If so first we remove the current piece from the board
				RemoveFromBoard(target.ParentTile.TileCoordinates, target.CoordinatesRequired, false);

				target.HasActed = true;

				// Place the Upgraded piece and hide it
				PlacePiece(null, target.UpgradePiecePrefab.PieceType, target.ParentTile.TileCoordinates, player.ID, out Piece newPiece);
				newPiece.PieceModel.gameObject.SetActive(false);

				if (player)
				{
					player.ResourceManager.MovePoints -= target.MoveCost;
				}

				newPiece.OwnerID = target.OwnerID;

				newPiece.ColorController.InitialColor = target.ColorController.InitialColor;

				Animator animator = newPiece.Animator;

				AudioManager.Instance.AudioSource_Environmental.clip = AudioManager.Instance.SoundDictionary[AudioClipName.ChunkRumbling].AudioClip;
				AudioManager.Instance.AudioSource_Environmental.Play();

				if (animator) animator.SetBool("Selected", false);

				AudioManager.PlayClip(AudioClipName.ChunkThud);

				// Play the destroy old piece animation
				Tween.Instance.Ease_Scale_LinerY(target.PieceModel, 1, 0, 1.5f, 0, () =>
				{
					target.PieceModel.transform.gameObject.SetActive(false);

					AudioManager.PlayClip(AudioClipName.ChunkThud);

					newPiece.PieceModel.gameObject.SetActive(true);
					Tween.Instance.Ease_Scale_LinerY(newPiece.transform, 0, 1, 1.5f, 0, () =>
					{

						AudioManager.Instance.AudioSource_Environmental.Stop();
						AudioManager.PlayClip(AudioClipName.ChunkThud);

					});
				});
			}
			else
			{
				AudioManager.PlayClip(AudioClipName.Nope);
			}
		}

		// Change the height of a chunk
		public void ChangeChunkHeight(Player player, Piece target, int direction)
		{
			if (player && player.ResourceManager.MovePoints < target.MoveCost)
			{
				AudioManager.PlayClip(AudioClipName.Nope);
				return;
			}

			if (target.HasActed) return;

			ResetBoardState();

			Chunk chunk = target.GetComponentInParent<Chunk>();

			foreach (Piece piece in PieceDictionary.Values)
			{
				if (IsOnChunkBoarder(chunk, piece)) {
					RemoveFromBoard(piece.ParentTile.TileCoordinates, piece.CoordinatesRequired);
				}
			}

			// set the height of chunk and clap it between its min and amx value
			chunk.SetHeight(Mathf.Clamp(chunk.Height + direction, chunk.MinHeight, chunk.MaxHeight));

			if (player)
			{
				player.ResourceManager.MovePoints -= target.MoveCost;
			}

			target.HasActed = true;
		}

		// Display the given pieces moves
		public void DisplayMoves(Piece piece)
		{
			SelectedPiece = piece;

			Chunk currentChunk = piece.GetParentChunk();

			// Loop through all of the given pieces moves
			foreach (MoveParameters move in piece.Moves) {
				for (int step = 0; step < move.Distance; step++)
				{
					// Calculate the next position to check
					Vector2Int coordinates = piece.ParentTile.TileCoordinates + (move.Direction * (step + 1));

					// If that tile is real
					if (TileDictionary.ContainsKey(coordinates))
					{
						// Add this move to the list of highlighted moves.
						highlightedMoves.Add(TileDictionary[coordinates]);

						Chunk tileChunk = TileDictionary[coordinates].GetComponentInParent<Chunk>();

						if (Math.Abs(currentChunk.Height - tileChunk.Height) <= piece.JumpRange)
						{
							// Check if there is a piece occupying the next move
							if (PieceDictionary.ContainsKey(coordinates))
							{
								// If there is already a piece there
								// Make this as an action Tile

								if (PieceDictionary[coordinates] == this)
								{
									// The tile is US
									TileDictionary[coordinates].SetTileState(TileType.Self);
								}
								else if (PieceDictionary[coordinates].OwnerID == piece.OwnerID)
								{
									// The tile is an ally
									TileDictionary[coordinates].SetTileState(TileType.AllyInfo);
								}
								else
								{
									// The tile is an enemy
									TileDictionary[coordinates].SetTileState(TileType.EnemyPiece);
								}

								if (!move.AbsoluteMove) break;
							}
							else
							{
								// If there is not a piece occupying this tile...
								// This means this is a Move options for this piece
								TileDictionary[coordinates].SetTileState(TileType.Move);
							}
						}
					}
				}
			}

			highlightedMoves.Add(TileDictionary[piece.ParentTile.TileCoordinates]);
			TileDictionary[piece.ParentTile.TileCoordinates].SetTileState(TileType.Self);
		}

		// Show the placement options for the player
		public void DisplayPlacementOptions()
		{
			ResetBoardState();

			// Loop through all the pieces in the game
			foreach (Piece piece in PieceDictionary.Values)
			{
				// When a piece does not have a placement range we skip it
				if (piece.PlaceRange == 0) continue;

				// If the piece is on the team of the current player
				if (piece.OwnerID == GameManager.PlayerTurn)
				{

					// Loop thorough all the tiles that fall into the placeRange around this piece
					for (int y = -piece.PlaceRange; y <= piece.PlaceRange; y++) {
						for (int x = -piece.PlaceRange; x <= piece.PlaceRange; x++)
						{
							Vector2Int pos = piece.ParentTile.TileCoordinates + new Vector2Int(x, y);

							if (TileDictionary.ContainsKey(pos)) {

								TileDictionary[pos].TileType = TileType.MoveOption;
								highlightedMoves.Add(TileDictionary[pos]);

								if (PieceDictionary.ContainsKey(pos))
								{
									TileDictionary[pos].HighlightController.SetColor(HighlightType.Attack, HighlightController.PLACEMENT_OPTION);
								}
								else
								{
									TileDictionary[pos].HighlightController.SetColor(HighlightType.PlacementOption, HighlightController.PLACEMENT_OPTION);
								}
							}
						}
					}
				}
			}
		}

		// Reset the state of the Board to default
		// This means setting SelectedTarget & HoverTarget to null
		// And looping through the highlightedMoves and resetting them.
		public void ResetBoardState()
		{
			SelectedPiece = null;

			SelectedTarget?.OnPointerUnClick(WorldState);
			SelectedTarget = null;

			HoverTarget?.OnPointerUnClick(WorldState);
			HoverTarget = null;

			GameManager.TowerShop.Close();
			GameManager.FactoryShop.Close();

			// Loop through all the tiles that are currently being highlighted on the board
			foreach (Tile tile in highlightedMoves)
			{
				// Return the tile to a 'Normal' tile
				tile.ClearTileState();
			}

			foreach (Piece piece in PieceDictionary.Values)
			{
				// Return the tile to a 'Normal' tile
				piece.OnPointerUnClick(WorldState);
			}
		}

		#region (Public) Utility Methods

		// Check if the Piece located at the given coordinates can exist.
		// Meaning it can move here without overlapping with an existing piece.
		// NOTE: If this is a multi tile structure is will NOT be able to move.
		public bool CanMovePiece(Vector2Int coordinates, List<Vector2Int> coordinatesRequired = null)
		{
			ValidateRequiredCoordinates(coordinatesRequired);

			foreach (Vector2Int coordinateOffset in coordinatesRequired)
			{
				Vector2Int targetCoordinates = coordinates + coordinateOffset;

				// If wither the tile does not exists or there is already a piece here
				// Return false..
				if (!CheckForTile(targetCoordinates)) return false;
				if (CheckForPiece(targetCoordinates)) return false;
			}

			return true;
		}

		// CanFitPiece Is similar to the CanMovePiece method.
		// However we only return false if...
		// 1) We can't fit on the tiles in this area.
		// 2) We are going to attack an ally piece.
		public bool CanFitPiece(Vector2Int coordinates, List<Vector2Int> coordinatesRequired = null, bool ignoreEnemies = true)
		{
			ValidateRequiredCoordinates(coordinatesRequired);

			foreach (Vector2Int coordinateOffset in coordinatesRequired)
			{
				Vector2Int targetCoordinates = coordinates + coordinateOffset;

				// If wither the tile does not exists or there is already a piece here
				// Return false..
				if (!CheckForTile(targetCoordinates)) return false;
				if (CheckForPiece(targetCoordinates))
				{
					// There is a piece here..
					// If it is OUR piece, we want to return false	
					if (PieceDictionary[targetCoordinates].OwnerID == SelectedPiece.OwnerID &&
						PieceDictionary[targetCoordinates] != SelectedPiece)
					{
						return false;
					}
					else
					{
						// Else this is just an enemy in our way :9
						// If we want to 'ignoreEnemies' we just continue.
						if (ignoreEnemies)
						{
							continue;
						}
						else
						{
							// But is were stopping when we encounter an enemy
							// Return false	
							return false;
						}
					}
				}
				else
				{
					// There is no piece here
					continue;
				}
			}

			return true;
		}

		// Returns true if the piece is on a chunk boarder
		public bool IsOnChunkBoarder(Chunk chunk, Piece piece)
		{
			List<Vector2Int> coordinatesRequired = piece.CoordinatesRequired;

			ValidateRequiredCoordinates(coordinatesRequired);

			// Look for the a parent chunk that matches the chunk we are moving
			foreach (Vector2Int coordinateOffset in coordinatesRequired) {
				Vector2Int targetCoordinates = piece.ParentTile.TileCoordinates + coordinateOffset;

				Chunk parentChunk = PieceDictionary[targetCoordinates].GetComponentInParent<Chunk>();

				// If we found a piece that sharsed the save chunk parent
				if (chunk == parentChunk)
				{
					// Loop through again and make sure its all the same parrent
					foreach (Vector2Int offset in coordinatesRequired)
					{
						Chunk targetChunk = PieceDictionary[piece.ParentTile.TileCoordinates + offset].GetComponentInParent<Chunk>(); ;

						if (targetChunk != chunk) {
							return true;
						}
					}
				}
			}

			return false;
		}

		// Remove a piece from the board
		public void RemoveFromBoard(Vector2Int coordinates, List<Vector2Int> coordinatesRequired = null, bool destroyTarget = true)
		{
			ValidateRequiredCoordinates(coordinatesRequired);

			foreach (Vector2Int coordinateOffset in coordinatesRequired)
			{
				Vector2Int targetCoordinates = coordinates + coordinateOffset;

				if (PieceDictionary.ContainsKey(targetCoordinates)) {
					if (PieceDictionary[targetCoordinates] && destroyTarget)
					{
						Destroy(PieceDictionary[targetCoordinates].gameObject);
					}

					PieceDictionary.Remove(targetCoordinates);
				}
			}
		}

		public List<Tile> GetSurroundingTiles(Vector2Int coordinates, int range)
		{
			List<Tile> tiles = new List<Tile>();

			// Loop thorough all the tiles that fall into the placeRange around this piece
			for (int y = -range; y <= range; y++) {
				for (int x = -range; x <= range; x++)
				{
					Vector2Int pos = coordinates + new Vector2Int(x, y);

					if (TileDictionary.ContainsKey(pos)) {
						tiles.Add(TileDictionary[pos]);
					}
				}
			}

			return tiles;
		}

		public bool CheckForTile(Vector2Int coordinates)
		{
			return TileDictionary.ContainsKey(coordinates);
		}

		public bool CheckForPiece(Vector2Int coordinates)
		{
			return PieceDictionary.ContainsKey(coordinates);
		}

		#endregion

		#endregion

		#region Private Methods

		// Make sure the player has enough move points to make a move.
		// Reduce the players move points.
		bool ValidateMovePoints(Player player, int cost)
		{
			var resourceManager = player.ResourceManager;

			if (resourceManager.MovePoints >= cost) {
				resourceManager.MovePoints -= cost;

				return true;
			}
			else
			{
				AudioManager.PlayClip(AudioClipName.Nope);

				return false;
			}
		}

		// Make sure this is not am empty list.
		// Each piece should at least require the tile its standing on.
		void ValidateRequiredCoordinates(List<Vector2Int> coordinatesRequired )
		{
			if (coordinatesRequired == null || coordinatesRequired.Count == 0) {
				coordinatesRequired = new List<Vector2Int>() { new Vector2Int(0, 0) };
			}
		}

		// Set the given pieces color
		void SetPieceColor(Piece piece, int ownerID)
		{
			if (PlayerColors == null) return;

			ColorController colorController = piece.ColorController;

			if (ownerID < PlayerColors.Count)
			{
				Color pieceColor = PlayerColors[ownerID];

				colorController.InitialColor = pieceColor;
				colorController.SetColor(pieceColor);
			}
			else
			{
				Debug.LogError($"There are not enough player colors for this ownerID {ownerID}");

				Color pieceColor = Color.red;

				colorController.InitialColor = pieceColor;
				colorController.SetColor(pieceColor);
			}
		}

		#endregion
	}
}