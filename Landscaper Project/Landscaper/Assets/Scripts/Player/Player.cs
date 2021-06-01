using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Core;
using Landscaper.Players;
using Landscaper.UI.HUD;
using Landscaper.Pieces;

public class Player : MonoBehaviour
{
	public PlayerResourceManager ResourceManager;
	public PlayerPieceManager PieceManager;
	public PiecePicker PiecePicker;

	public PlayerHUD HUD = null;

	public int ID;

	private void Awake()
	{
		OnNewGame();
	}

	public void OnNewGame()
	{
		PieceManager.PieceCountDictionary = new Dictionary<PieceType, int>()
		{
			{PieceType.King, 0},
			{PieceType.Queen, 1},

			{PieceType.Bishop, 2},
			{PieceType.Knight, 2},
			{PieceType.Rook, 2},

			{PieceType.Pawn, 3}
		};

		PiecePicker.Link(PieceManager.PieceCountDictionary);

		HUD.Link(ResourceManager);
	}
}
