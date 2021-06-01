using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Landscaper.Core
{
	public class WorldState
	{
		//
		public int CurrentTurnID;

		//
		public Piece SelectedPiece = null;

		// Player Selector Data
		public ISelectable Selected = null;
		public ISelectable Hovered = null;

		// Game Manager Data
		public Dictionary<Vector2Int, Tile> TileDictionary = new Dictionary<Vector2Int, Tile>();
		public Dictionary<Vector2Int, Piece> PieceDictionary = new Dictionary<Vector2Int, Piece>();
	}
}
