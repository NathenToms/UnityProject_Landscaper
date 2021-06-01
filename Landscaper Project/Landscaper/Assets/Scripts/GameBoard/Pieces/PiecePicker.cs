using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.UI.CustomComponents;


namespace Landscaper.Pieces
{
	public delegate void OnPiecePickedDelegate(Piece piece);

	public class PiecePicker : MonoBehaviour
	{
		public static bool IsOpen = false;

		#region Piece Buttons

		[SerializeField] private GamePieceShopButton _pawnButton = null;
		[SerializeField] private GamePieceShopButton _rookButton = null;
		[SerializeField] private GamePieceShopButton _knightButton = null;
		[SerializeField] private GamePieceShopButton _bishopButton = null;
		[SerializeField] private GamePieceShopButton _queenButton = null;
		[SerializeField] private GamePieceShopButton _kingButton = null;

		#endregion

		// Public Members
		public OnPiecePickedDelegate OnPiecePicked;
		public SlidingMenu SlidingMenu;

		// Private Members
		private Dictionary<PieceType, int> pieceCountDictionary;

		public bool IsLinked = false;

		public void Link(Dictionary<PieceType, int> PieceCountDictionary)
		{
			pieceCountDictionary = PieceCountDictionary;

			IsLinked = true;
		}

		private void FixedUpdate()
		{
			if (IsLinked)
			{
				_kingButton.Init(this, pieceCountDictionary[PieceType.King], "King");
				_queenButton.Init(this, pieceCountDictionary[PieceType.Queen], "Queen");

				_bishopButton.Init(this, pieceCountDictionary[PieceType.Bishop], "Bishop");
				_knightButton.Init(this, pieceCountDictionary[PieceType.Knight], "Knight");
				_rookButton.Init(this, pieceCountDictionary[PieceType.Rook], "Rook");

				_pawnButton.Init(this, pieceCountDictionary[PieceType.Pawn], "Pawn");
			}
		}

		public void OnPick(Piece piecePrefab)
		{
			OnPiecePicked.Invoke(piecePrefab);
		}
	}
}