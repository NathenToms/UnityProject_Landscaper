using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.Save;
using Landscaper.Tiles;
using Landscaper.UI.CustomComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAudio;
using System;

namespace Landscaper.Players
{
	public class PlayerPointer : MonoBehaviour
	{
		[Header("References")]
		public PlayerPieceManager PieceManager;
		public Player Player;

		[Header("Materials")]
		public Material PointerHologramMat;

		[Header("Key Binds")]
		public KeyCode RightMouse = KeyCode.Mouse1;
		public KeyCode LeftMouse = KeyCode.Mouse0;

		// Private Members
		private GameManager gameManager;
		private Transform pointerAnchor;

		public bool RightMouseClick { get { return Input.GetKeyDown(RightMouse); } }
		public bool LeftMouseClick { get { return Input.GetKeyDown(LeftMouse); } }

		// Private Members
		private IRayProvider rayProvider;

		private ISelectable Selected
		{
			get { return gameManager.LandscaperBoard.SelectedTarget; }
			set { gameManager.LandscaperBoard.SelectedTarget = value; }
		}
		private ISelectable Hovered
		{
			get { return gameManager.LandscaperBoard.HoverTarget; }
			set { gameManager.LandscaperBoard.HoverTarget = value; }
		}

		private void Awake()
		{
			pointerAnchor = GameObject.FindGameObjectWithTag("Pointer").transform;

			gameManager = FindObjectOfType<GameManager>();
			rayProvider = GetComponent<IRayProvider>();
		}

		private void Update()
		{
			// Make sure we have a PieceManager and a RayProvider
			if (PieceManager == null || rayProvider == null) return;

			if (RightMouseClick) {
				ClearSelection();
			}

			if (gameManager.EnemyTurn == false) {
				CheckForClickTarget();
			}
		}

		void CheckForClickTarget()
		{
			// Raycast from the screen to the world through the cursor
			if (rayProvider.Raycast(out RaycastHit hit))
			{
				if (hit.transform.TryGetComponent<ISelectable>(out ISelectable selectable))
				{
					HandleClickedTarget(selectable, hit.transform);
					HandleHoveredTarget(selectable, hit.transform);
				}
				else
				{
					ClearHoveredTile();
				}
			}
			else
			{
				ClearHoveredTile();
			}
		}

		void HandleClickedTarget(ISelectable selectable, Transform target)
		{
			if (UIClickBlocker.HoveredTarget) return;

			// Check if we clicked on the left mouse on this frame
			bool click = LeftMouseClick;

			if (click)
			{
				// If we are clicking what is currently selected
				if (Selected == selectable)
				{
					// This counts as a double click
					// Debug.Log("Double Click!");
				}
				else
				{
					// Get a copy of the current world state
					WorldState worldState = gameManager.LandscaperBoard.WorldState;

					// UnClick the last thing selected
					Selected?.OnPointerUnClick(worldState);

					// If we clicked on a game PIECE
					if (target.TryGetComponent<Piece>(out Piece piece))
					{
						// When clicking on a game piece we want to redirect the click
						// We Get the pieces location and click on its tile instead
						// Debug.Log("Redirecting to parent Tile!");

						HandleTileClicked(piece.ParentTile);
					}
					else
					{
						// Else if we clicked on a TILE
						if (target.TryGetComponent<Tile>(out Tile tile))
						{
							HandleTileClicked(tile);
						}
						else
						{
							AudioManager.PlayClip(AudioClipName.Nope);
						}
					}
				}
			}
		}

		void HandleTileClicked(Tile target)
		{
			if (target == null)
			{
				// Something went very wrong
			}
			else
			{
				// Get a copy of the current world state
				WorldState worldState = gameManager.LandscaperBoard.WorldState;


				// Else this is a TILE click, NOT a place action
				gameManager.LandscaperBoard.ClickTile(target);

				// Check if the player have take a piece from the piece picker
				// If they have we want to place that piece down
				if (PieceManager.HasPieceSelected)
				{
					if (target.TileType == TileType.MoveOption)
					{
						// If the tile we clicked on was a Move options..
						// (Move options are highlighted in green abouts specific pieces)

						// We want to place the piece
						gameManager.LandscaperBoard.PlacePiece(Player, PieceManager.GetPieceSelected().PieceType, target.TileCoordinates, Player.ID, out Piece piece);
		
						ClearPointer();
					}
					else
					{
						AudioManager.PlayClip(AudioClipName.Nope);
					}

					gameManager.LandscaperBoard.ResetBoardState();
				}
				else
				{
					Piece piece = gameManager.LandscaperBoard.SelectedPiece;

					// Check what kind of tile we are clicking
					if (target.TileType == TileType.Normal)
					{
						// If we are clicking on a normal tile..
						// If that tile is occupied..
						if (gameManager.LandscaperBoard.PieceDictionary.ContainsKey(target.TileCoordinates))
						{
							// If we clicked on an ally piece
							if (gameManager.LandscaperBoard.PieceDictionary[target.TileCoordinates].OwnerID == Player.ID) {
								gameManager.LandscaperBoard.DisplayMoves(gameManager.LandscaperBoard.PieceDictionary[target.TileCoordinates]);
								gameManager.LandscaperBoard.PieceDictionary[target.TileCoordinates].OnPointerClick(worldState);

								// If we clicked on an ally piece
								Selected?.OnPointerClick(worldState);
							}
							else
							{
								// Else we Clicked on an enemy piece
								// Debug.Log("That is an enemy!");
							}
						}
						else
						{
							// We clicked on a tile with nothing on it
							Selected?.OnPointerClick(worldState);
						}
					}
					else
					{
						ClickActionTile(piece, target);

						gameManager.LandscaperBoard.ResetBoardState();
					}
				}
			}
		}

		void ClickActionTile(Piece piece, Tile tile)
		{
			if (tile.TileType == TileType.Move)
			{
				// Debug.Log("Move Click");
				gameManager.LandscaperBoard.MovePiece(Player, piece, tile);
			}

			if (tile.TileType == TileType.EnemyPiece)
			{
				// Debug.Log("Enemy Click");
				gameManager.LandscaperBoard.CapturePiece(Player, piece, tile);
			}

			if (tile.TileType == TileType.AllyInfo)
			{
				// Debug.Log("Ally Click");
			}

			if (tile.TileType == TileType.Self)
			{
				// Debug.Log("Self Click");
			}
		}

		void HandleHoveredTarget(ISelectable selectable, Transform target)
		{
			if (Hovered == selectable)
			{
				// This is what we are hovering
			}
			else
			{
				Hovered?.OnPointerExit(gameManager.LandscaperBoard.WorldState);
				gameManager.LandscaperBoard.HoverTile(selectable);
				Hovered?.OnPointerEnter(gameManager.LandscaperBoard.WorldState);
			}

			HandlePointer(target);
		}

		void HandlePointer(Transform target)
		{
			if (PieceManager.HasPieceSelected)
			{
				// If we clicked on a tile
				if (target.TryGetComponent<Tile>(out Tile tile))
				{
					pointerAnchor.gameObject.SetActive(true);
					pointerAnchor.transform.position = tile.ChildAnchor.position;
				}
				else
				{
					pointerAnchor.gameObject.SetActive(false);
				}
			}
			else
			{
				pointerAnchor.gameObject.SetActive(false);
			}
		}

		public void UpdatePointer(Transform piecePrefab)
		{
			foreach (Transform child in pointerAnchor) Destroy(child.gameObject);

			Transform pointer = Instantiate(piecePrefab);
			pointer.SetParent(pointerAnchor, false);

			foreach (Collider collider in pointer.GetComponentsInChildren<Collider>()) collider.enabled = false;
			foreach (Animator animator in pointer.GetComponentsInChildren<Animator>()) animator.enabled = false;
			foreach (Renderer renderer in pointer.GetComponentsInChildren<Renderer>())
			{
				renderer.material = PointerHologramMat;
				renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}
		}

		void ClearSelection()
		{
			if (gameManager.LandscaperBoard.HighlightedMoveCount > 0 ||
				gameManager.LandscaperBoard.SelectedTarget != null)
			{
				// Else the play sound
				AudioManager.PlayClip(AudioClipName.DeSelect);
			}

			gameManager.LandscaperBoard.ResetBoardState();

			PieceManager.HasPieceSelected = false;
		}

		void ClearPointer()
		{
			foreach (Transform child in pointerAnchor)
			{
				Destroy(child.gameObject);
			}

			PieceManager.PieceSelected = null;
			PieceManager.HasPieceSelected = false;
		}

		void ClearHoveredTile()
		{
			Hovered?.OnPointerExit(gameManager.LandscaperBoard.WorldState);
			gameManager.LandscaperBoard.HoverTile(null);
		}

		Player GetActivePlayer()
		{
			return Player;
		}
	}
}