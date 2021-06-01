using System;
using System.Collections;
using System.Collections.Generic;
using Landscaper.UI;
using Landscaper.Core;
using Landscaper.Players;
using Landscaper.Save;
using Landscaper.Tiles;
using UnityEngine;
using Landscaper.Chunks;

namespace Landscaper.Pieces
{
	[System.Serializable]
	public class Piece : MonoBehaviour, ISelectable
	{
		[Header("Component References")]
		public PieceActions PieceActions;
		public ColorController ColorController;
		public Animator Animator;
		public Transform PieceModel;

		#region Piece Properties

		[Header("Piece Properties")]

		// The Type of Piece this is
		public PieceType PieceType;

		// Has this piece made an action this turn
		public bool HasActed = false;

		// The owner of this piece
		public int OwnerID = -1;

		public int MoveCost = 1;
		public int PlaceCost = 1;
		public int PlaceRange = 0;
		public int JumpRange = 0;

		// A List of actions this piece can make
		public PieceAbilities Abilities;

		#endregion

		#region Prefabs

		[Header("Prefabs")]

		// This is what this piece upgrades into.
		public Piece UpgradePiecePrefab;

		#endregion

		#region Movement Information

		[Header("Piece Moves")]

		// The Moves this piece cam make
		public List<MoveParameters> Moves = new List<MoveParameters>();

		#endregion

		#region Coordinates Required

		[Header("Coordinates Required")]

		// The CoordinatesRequired this piece takes up
		// This is used for multi tile pieces
		public List<Vector2Int> CoordinatesRequired = new List<Vector2Int>() { new Vector2Int(0, 0) };

		#endregion

		 public Tile ParentTile { get { return GetComponentInParent<Tile>(); } }

		private void Awake()
		{
			PieceActions = GetComponent<PieceActions>();
		}

		private void Start()
		{
			if (TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
			{
				Vector2Int tileCoordinates = new Vector2Int((int)transform.position.x, (int)transform.position.z);

				var gamemanager = FindObjectOfType<GameManager>();

				if (!gamemanager.LandscaperBoard.PieceDictionary.ContainsKey(tileCoordinates))
				{
					foreach (Vector2Int offset in CoordinatesRequired)
					{
						gamemanager.LandscaperBoard.PieceDictionary.Add(tileCoordinates + offset, this);
					}

					transform.SetParent(gamemanager.LandscaperBoard.TileDictionary[tileCoordinates].ChildAnchor, true);

					var encounterController = FindObjectOfType<EncounterController>();
					if (encounterController)
					{
						encounterController.AddPiece(enemyBehaviour);
					}
				}
			}
		}

		private void OnValidate()
		{
			if (TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
			{
				name = $"Piece (Evil {PieceType.ToString()})";
			}
		}

		public void MovePiece(Tile targetTile, WorldState worldState)
		{
			PieceActions.MovePiece(targetTile, worldState);
		}
		public void TakePiece(Tile targetTile, WorldState worldState)
		{
			PieceActions.TakePiece(targetTile, worldState);
		}

		#region ISelectable Interface

		public void OnPointerClick(WorldState worldState)
		{
			if (ColorController) ColorController.OnPointerClick();
			if (Animator) Animator.SetBool("Selected", true);
		}
		public void OnPointerUnClick(WorldState worldState)
		{
			if (ColorController) ColorController.OnPointerUnClick();
			if (Animator) Animator.SetBool("Selected", false);
		}

		public void OnPointerEnter(WorldState worldState)
		{
			if (ColorController) ColorController.OnPointerEnter();
		}
		public void OnPointerExit(WorldState worldState)
		{
			if (ColorController) ColorController.OnPointerExit();
		}

		public Chunk GetParentChunk()
		{
			return GetComponentInParent<Chunk>();
		}

		#endregion
	}
}