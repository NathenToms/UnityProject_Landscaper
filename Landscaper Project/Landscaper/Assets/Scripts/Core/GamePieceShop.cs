using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.UI.CustomComponents;



public class GamePieceShop : MonoBehaviour
{
	public static int MAX_COST = 999;

	[SerializeField] private SlidingMenu _slidingMenu = null;
	[SerializeField] private GameManager _gameManager = null;

	public void Open() => _slidingMenu.OpenMenu();
	public void Close() => _slidingMenu.CloseMenu();

	public void OnItemBuy(Piece piecePrefab)
	{
		int cost = MAX_COST;

		switch (piecePrefab.PieceType)
		{
			case PieceType.Queen:	cost = 10; break;
			case PieceType.Pawn:	cost = 2; break;
			case PieceType.Rook:	cost = 5; break;
			case PieceType.Bishop:	cost = 4; break;
			case PieceType.Knight:	cost = 4; break;

			default: break;
		}

		Player player = FindObjectOfType<Player>();

		_gameManager.ButItem(player, piecePrefab, cost);
	}
}
