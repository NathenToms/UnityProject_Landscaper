using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Players
{
	public class PlayerPieceManager : MonoBehaviour
	{
		[Header("Component References")]
		public PlayerPointer PlayerPointer;
		public GameManager GameManager;
		public PiecePicker PiecePicker;

		[Header("Prefabs")]
		public Transform PieceSelected = null;
		public Piece KingPrefab = null;
		public PieceList PieceList = null;

		[Header("Properties")]
		public bool HasPieceSelected = false;

		public Dictionary<PieceType, int> PieceCountDictionary = new Dictionary<PieceType, int>();

		public Piece GetPieceSelected() { return PieceSelected.GetComponent<Piece>(); }

		private void Awake()
		{
			PlayerPointer = FindObjectOfType<PlayerPointer>();
			GameManager = FindObjectOfType<GameManager>();
			PiecePicker = FindObjectOfType<PiecePicker>();

			PiecePicker.OnPiecePicked += OnPiecePicked;
		}

		public void SetCountDictionary(PieceList pieceList)
		{
			PieceCountDictionary.Clear();

			PieceCountDictionary = new Dictionary<PieceType, int>() 
			{
				{ PieceType.King, pieceList.KingCount},

				{ PieceType.Queen, pieceList.QueenCount},

				{ PieceType.Bishop, pieceList.BishopCount},
				{ PieceType.Knight, pieceList.KnightCount},
				{ PieceType.Rook, pieceList.RookCount},

				{ PieceType.Pawn, pieceList.PawnCount},
			};

			PieceList = pieceList;
		}

		public void OnPiecePicked(Piece piecePrefab)
		{
			PlayerPointer.UpdatePointer(piecePrefab.transform);
			PieceSelected = piecePrefab.transform;
			HasPieceSelected = true;

			GameManager.LandscaperBoard.DisplayPlacementOptions();

			PiecePicker.SlidingMenu.CloseMenu();
		}

		public bool HasPiece(PieceType pieceType)
		{
			if (!PieceCountDictionary.ContainsKey(pieceType))
			{
				return false;
			}
			else
			{
				if (PieceCountDictionary[pieceType] > 0)
				{
					return true;
				}

				return false;
			}
		}
	}
}