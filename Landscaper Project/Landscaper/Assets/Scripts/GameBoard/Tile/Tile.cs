using CustomAudio;
using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Tiles
{

	public class Tile : MonoBehaviour, ISelectable
	{
		[Header("Component References")]
		public TileDisplay TileDisplay;
		public HighlightController HighlightController;
		public Transform OreTransform;
		public GameManager GameManager;
		public Transform ChildAnchor;

		[Header("Tile Properties")]
		public bool Selected = false;
		public bool IsMineable = false;

		public TileType TileType;

		[Header("Tile Coordinates")]
		public Vector2Int TileCoordinates;

		public bool IsActionTile { get { return TileType != TileType.Normal; } }

		public void Initialized(GameManager gameManager)
		{
			// Get this tiles coordinates in the world			
			// This is used as its ID
			TileCoordinates = new Vector2Int((int)transform.position.x, (int)transform.position.z);

			GameManager = gameManager;
			GameManager.LandscaperBoard.RegisterTile(this);

			OreTransform.gameObject.SetActive(IsMineable);

			TileDisplay.Init(this);
		}

		#region Tile Actions

		// Update this tiles OnClick action
		public void SetTileState(TileType tileType)
		{
			TileType = tileType;

			switch (tileType)
			{
				case TileType.EnemyPiece:	
					HighlightController.SetColor(HighlightType.Attack, HighlightController.ATTACK);
					break;

				case TileType.Move:		
					HighlightController.SetColor(HighlightType.Move, HighlightController.MOVE);	
					break;

				case TileType.AllyInfo:
					HighlightController.SetColor(HighlightType.Ally, HighlightController.ALLY);	
					break;

				case TileType.Self:		
					HighlightController.SetColor(HighlightType.Self, HighlightController.SELF);	
					break;

				default:break;
			}

			TileDisplay.Init(this);
		}

		// Clear the action from this tile
		public void ClearTileState()
		{
			TileType = TileType.Normal;

			HighlightController.Clear(HighlightController.RESET);
		}

		#endregion

		#region ISelectable Interface

		public void OnPointerClick(WorldState worldState)
		{
			// We have clicked on a tile
			// This Tile is now selected for the sake of the color controller
			Selected = true;


			// Play the Tile Click sound
			AudioManager.PlayClip(AudioClipName.Click);

			// Check if this tile is occupied
			if (TileIsOccupied() ) {
				GameManager.LandscaperBoard.PieceDictionary[TileCoordinates].OnPointerClick(worldState);
			}

			HighlightController.SetColor(HighlightType.Selected, HighlightController.SELECTED);

			TileDisplay.Init(this);
		}
		public void OnPointerUnClick(WorldState worldState)
		{          
			// We have UnClicked this tile	
			// This Tile is now unselected for the sake of the color controller
			Selected = false;

			if (!IsActionTile)
			{
				HighlightController.Clear(HighlightController.RESET);
			}

			if (TileIsOccupied()) {
				GameManager.LandscaperBoard.PieceDictionary[TileCoordinates].OnPointerUnClick(worldState);
			}

			TileDisplay.Init(this);
		}

		public void OnPointerEnter(WorldState worldState)
		{
			if (!Selected)
			{
				if (IsActionTile)
				{
					HighlightController.SetColor(HighlightType.HighlightedMove, HighlightController.HIGHLIGHTED_MOVE);
				}
				else
				{
					HighlightController.SetColor(HighlightType.Highlighted, HighlightController.HIGHLIGHTED);
				}

				if (GameManager.LandscaperBoard.PieceDictionary.ContainsKey(TileCoordinates)) {
					GameManager.LandscaperBoard.PieceDictionary[TileCoordinates].OnPointerEnter(worldState);
				}
			}
		}
		public void OnPointerExit(WorldState worldState)
		{
			// Ad long as we are not selected
			if (!Selected)
			{
				if (IsActionTile)
				{
					// This reset lets highlighting a move option work
					if (TileType == TileType.Move) {
						HighlightController.Clear(HighlightController.RESET);
					}

					if (TileType == TileType.MoveOption)
					{
					}
					else
					{
						HighlightController.SetColor(HighlightType.Move, HighlightController.MOVE);
					}
				}
				else
				{
					HighlightController.Clear(HighlightController.RESET);
				}

				// If this tile is occupied, Click on the occupier too
				if (GameManager.LandscaperBoard.PieceDictionary.ContainsKey(TileCoordinates)) {
					GameManager.LandscaperBoard.PieceDictionary[TileCoordinates].OnPointerExit(worldState);
				}
			}
		}

		#endregion

		#region Private Members

		bool TileIsOccupied()
		{
			var pieceDictionary = GameManager.LandscaperBoard.PieceDictionary;

			// Does this tile have a piece on it?
			if (pieceDictionary.ContainsKey(TileCoordinates))
			{
				return true;
			}

			return false;
		}

		#endregion
	}
}