using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Pieces
{
	public class PieceManager : MonoBehaviour
	{
		public static PieceManager Instance;

		[SerializeField] 
		private List<Piece> _piecePrefabs = new List<Piece>();

		public Dictionary<PieceType, Piece> PieceDictionary;

		private void Awake()
		{
			if (Instance)
			{
				Debug.LogError("There can't be 2 'Piece Manager' in the scene!", this);
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
			}

			BuildDictionary();
		}

		void BuildDictionary()
		{
			PieceDictionary = new Dictionary<PieceType, Piece>();

			foreach (Piece piece in _piecePrefabs)
			{
				if (PieceDictionary.ContainsKey(piece.PieceType))
				{
					Debug.LogError("This piece has already been registered!");
				}
				else
				{
					PieceDictionary.Add(piece.PieceType, piece);
				}
			}
		}

		#region Static Methods

		public static bool GetPiece(PieceType pieceType, out Piece piece)
		{
			piece = null;

			if (ValidateSingleton())
			{
				if (Instance.PieceDictionary.ContainsKey(pieceType))
				{
					piece = Instance.PieceDictionary[pieceType];

					return true;
				}
				else
				{
					Debug.LogError($"That piece was not registered! Name: {pieceType.ToString()}");

					return false;
				}
			}

			return false;
		}

		public static bool ValidateSingleton()
		{
			if (Instance == null)
			{
				Debug.Log("There is no 'PieceManager' in the scene!");

				return false;
			}
			else
			{
				return true;
			}
		}

		#endregion
	}
}